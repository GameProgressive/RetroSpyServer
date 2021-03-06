﻿using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Database.DatabaseModel.MySql;
using UniSpyLib.Encryption;
using UniSpyLib.Logging;
using PresenceConnectionManager.Entity.Enumerate;
using PresenceConnectionManager.Entity.Structure;
using PresenceConnectionManager.Entity.Structure.Request.General;
using PresenceConnectionManager.Structure;
using PresenceConnectionManager.Structure.Data;
using PresenceSearchPlayer.Entity.Enumerate;
using Serilog.Events;
using System.Collections.Generic;
using System.Linq;
using PresenceConnectionManager.Network;
using PresenceConnectionManager.Abstraction.BaseClass;

namespace PresenceConnectionManager.Handler.CommandHandler
{
    internal class LoginDBResult
    {
        public uint UserID;
        public uint ProfileID;
        public string Nick;
        public string Email;
        public string UniqueNick;
        public string PasswordHash;
        public bool EmailVerifiedFlag;
        public bool BannedFlag;
        public uint NamespaceID;
        public uint SubProfileID;
    }

    public class LoginHandler : PCMCommandHandlerBase
    {
        protected new LoginRequest _request;
        private LoginDBResult _result;
        public LoginHandler(ISession session, IRequest request) : base(session, request)
        {
            _request = (LoginRequest)request;
        }

        protected override void CheckRequest()
        {
            switch (_request.LoginType)
            {
                case LoginType.NickEmail:
                    _session.UserData.Nick = _request.Nick;
                    _session.UserData.Email = _request.Email;
                    _session.UserData.NamespaceID = _request.NamespaceID;
                    break;
                case LoginType.UniquenickNamespaceID:
                    _session.UserData.UniqueNick = _request.Uniquenick;
                    _session.UserData.NamespaceID = _request.NamespaceID;
                    break;
                case LoginType.AuthToken:
                    _session.UserData.AuthToken = _request.AuthToken;
                    break;
                default:
                    LogWriter.ToLog(LogEventLevel.Error, "Unknown login method detected!");
                    break;
            }

        }

        protected override void DataOperation()
        {
            try
            {
                switch (_request.LoginType)
                {
                    case LoginType.NickEmail:
                        NickEmailLogin();
                        break;

                    case LoginType.UniquenickNamespaceID:
                        UniquenickLogin();
                        break;

                    case LoginType.AuthToken:
                        AuthtokenLogin();
                        break;
                }
            }
            catch
            {
                _errorCode = GPError.DatabaseError;
            }

            //Arves is correct
            if (_errorCode != GPError.NoError)
            {
                return;
            }

            if (!IsChallengeCorrect())
            {
                _errorCode = GPError.LoginBadPassword;
                return;
            }

            if (!_result.EmailVerifiedFlag)
            {
                _errorCode = GPError.LoginBadProfile;
                return;
            }

            // Check the status of the account.
            // If the single profile is banned, the account or the player status.
            if (_result.BannedFlag)
            {
                _errorCode = GPError.LoginProfileDeleted;
                return;
            }
        }

        protected override void Response()
        {
            base.Response();
            //Arves is correct we need to check this
            if (_errorCode != GPError.NoError)
            {
                return;
            }

            if (_result == null)
            {
                return;
            }

            _session.UserData.UserStatus = GPStatus.Online;
            _session.UserData.UserID = _result.UserID;
            _session.UserData.ProfileID = _result.ProfileID;
            _session.UserData.SubProfileID = _result.SubProfileID;
            //_session.UserData.ProductID =
            _session.UserData.GameName = _request.GameName;
            _session.UserData.GamePort = _request.GamePort;
            _session.UserData.LoginStatus = LoginStatus.Completed;
            _session.UserData.SDKRevision = _request.SDKType;

            PCMServer.LoggedInSession.GetOrAdd(_session.Id, _session);
            SDKRevision.ExtendedFunction(_session);
        }

        protected override void BuildNormalResponse()
        {
            //string checkSumStr = _result.Nick + _result.UniqueNick + _result.NamespaceID;

            //_session.UserData.SessionKey = _crc.ComputeChecksum(checkSumStr);

            ChallengeProofData proofData = new ChallengeProofData(
              _request.UserData,
              _request.LoginType,
              _request.PartnerID,
              ChallengeProofData.ServerChallenge,
              _request.UserChallenge,
              _result.PasswordHash);

            string responseProof =
                ChallengeProof.GenerateProof(proofData);

            _sendingBuffer = @"\lc\2\sesskey\" + UserData.SessionKey;
            _sendingBuffer += @"\proof\" + responseProof;
            _sendingBuffer += @"\userid\" + _result.UserID;
            _sendingBuffer += @"\profileid\" + _result.ProfileID;

            if (_request.LoginType != LoginType.NickEmail)
            {
                _sendingBuffer += @"\uniquenick\" + _result.UniqueNick;
            }
            _sendingBuffer += $@"\lt\{UserData.LoginTicket}";
            _sendingBuffer += $@"\id\{_request.OperationID}\final\";

            _session.UserData.LoginStatus = LoginStatus.Completed;
        }

        private void NickEmailLogin()
        {
            //Check email existence
            using (var db = new retrospyContext())
            {
                var email = from u in db.Users
                            where u.Email == _session.UserData.Email
                            select u.Userid;

                if (email.Count() == 0)
                {
                    _errorCode = GPError.LoginBadEmail;
                    return;
                }

                //Grab information from database
                // default namespace id = 0
                var info = from u in db.Users
                           join p in db.Profiles on u.Userid equals p.Userid
                           join n in db.Subprofiles on p.Profileid equals n.Profileid
                           where u.Email == _session.UserData.Email
                           && p.Nick == _session.UserData.Nick
                           && n.Namespaceid == _request.NamespaceID
                           select new LoginDBResult
                           {
                               Email = u.Email,
                               UserID = u.Userid,
                               ProfileID = p.Profileid,
                               SubProfileID = n.Subprofileid,
                               Nick = p.Nick,
                               UniqueNick = n.Uniquenick,
                               PasswordHash = u.Password,
                               NamespaceID = n.Namespaceid,
                               EmailVerifiedFlag = (bool)u.Emailverified,
                               BannedFlag = u.Banned
                           };

                if (info.Count() != 1)
                {
                    _errorCode = GPError.LoginBadProfile;
                    return;
                }
                _result = info.First();
            }
        }

        private void UniquenickLogin()
        {
            using (var db = new retrospyContext())
            {
                var info = from n in db.Subprofiles
                           join p in db.Profiles on n.Profileid equals p.Profileid
                           join u in db.Users on p.Userid equals u.Userid
                           where n.Uniquenick == _session.UserData.UniqueNick
                           && n.Namespaceid == _session.UserData.NamespaceID
                           select new LoginDBResult
                           {
                               Email = u.Email,
                               UserID = u.Userid,
                               ProfileID = p.Profileid,
                               SubProfileID = n.Subprofileid,
                               Nick = p.Nick,
                               UniqueNick = n.Uniquenick,
                               PasswordHash = u.Password,
                               NamespaceID = n.Namespaceid,
                               EmailVerifiedFlag = (bool)u.Emailverified,
                               BannedFlag = u.Banned
                           };

                if (info.Count() != 1)
                {
                    _errorCode = GPError.LoginBadUniquenick;
                    return;
                }
                _result = info.First();
            }
        }

        private void AuthtokenLogin()
        {
            using (var db = new retrospyContext())
            {
                var info = from u in db.Users
                           join p in db.Profiles on u.Userid equals p.Userid
                           join n in db.Subprofiles on p.Profileid equals n.Profileid
                           where n.Authtoken == _session.UserData.AuthToken
                           && n.Partnerid == _session.UserData.PartnerID
                           && n.Namespaceid == _session.UserData.NamespaceID
                           select new LoginDBResult
                           {
                               Email = u.Email,
                               UserID = u.Userid,
                               ProfileID = p.Profileid,
                               SubProfileID = n.Subprofileid,
                               Nick = p.Nick,
                               UniqueNick = n.Uniquenick,
                               PasswordHash = u.Password,
                               NamespaceID = n.Namespaceid,
                               EmailVerifiedFlag = (bool)u.Emailverified,
                               BannedFlag = u.Banned
                           };

                if (info.Count() != 1)
                {
                    _errorCode = GPError.LoginBadPreAuth;
                    return;
                }
                _result = info.First();
            }
        }

        protected bool IsChallengeCorrect()
        {
            ChallengeProofData proofData = new ChallengeProofData(
                _request.UserData,
                _request.LoginType,
                _request.PartnerID,
               _request.UserChallenge,
               ChallengeProofData.ServerChallenge,
               _result.PasswordHash);

            string response = ChallengeProof.GenerateProof(proofData);

            if (_request.Response == response)
            {
                return true;
            }
            return false;
        }
    }
}
