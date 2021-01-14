﻿using QueryReport.Abstraction.BaseClass;
using QueryReport.Entity.Enumerate;
using System;
using System.Linq;
using System.Text;

namespace QueryReport.Entity.Structure.Request
{
    internal sealed class HeartBeatRequest : QRRequestBase
    {
        public string ServerData { get; private set; }
        public string PlayerData { get; private set; }
        public string TeamData { get; private set; }
        public string DataPartition { get; private set; }
        public HeartBeatReportType ReportType { get; set; }

        public HeartBeatRequest(object rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            base.Parse();
            int playerPos, teamPos;
            int playerLenth, teamLength;
            DataPartition = Encoding.ASCII.GetString(RawRequest.Skip(5).ToArray());

            playerPos = DataPartition.IndexOf("player_\0", StringComparison.Ordinal);
            teamPos = DataPartition.IndexOf("team_t\0", StringComparison.Ordinal);

            if (playerPos != -1 && teamPos != -1)
            {
                ReportType = HeartBeatReportType.ServerTeamPlayerData;
                playerLenth = teamPos - playerPos;
                teamLength = DataPartition.Length - teamPos;

                ServerData = DataPartition.Substring(0, playerPos - 4);
                PlayerData = DataPartition.Substring(playerPos - 1, playerLenth - 2);
                TeamData = DataPartition.Substring(teamPos - 1, teamLength);
            }
            else if (playerPos != -1)
            {
                //normal heart beat
                ReportType = HeartBeatReportType.ServerPlayerData;
                playerLenth = DataPartition.Length - playerPos;
                ServerData = DataPartition.Substring(0, playerPos - 4);
                PlayerData = DataPartition.Substring(playerPos - 1, playerLenth);
            }
            else if (playerPos == -1 && teamPos == -1)
            {
                ReportType = HeartBeatReportType.ServerData;
                ServerData = DataPartition;
            }
            else
            {
                ErrorCode = QRErrorCode.Parse;
                return;
            }
        }
    }
}
