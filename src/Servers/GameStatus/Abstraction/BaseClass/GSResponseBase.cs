﻿using GameStatus.Entity.Enumerate;
using System;
using UniSpyLib.Abstraction.BaseClass;

namespace GameStatus.Abstraction.BaseClass
{
    internal abstract class GSResponseBase : UniSpyResponseBase
    {
        protected new GSRequestBase _request => (GSRequestBase)base._request;
        protected new GSResultBase _result => (GSResultBase)base._result;
        protected new string SendingBuffer
        {
            get { return (string)base.SendingBuffer; }
            set { base.SendingBuffer = value; }
        }
        public GSResponseBase(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            if (_result.ErrorCode != GSErrorCode.NoError)
            {
                BuildErrorResponse();
            }
            else
            {
                BuildNormalResponse();
            }
        }

        protected override void BuildErrorResponse()
        {
            throw new NotImplementedException();
        }
    }
}
