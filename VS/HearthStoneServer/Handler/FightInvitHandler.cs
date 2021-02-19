using Photon.SocketServer;
using System;
using System.Collections.Generic;
using System.Linq;
using Allpurpose.Enum;
using Allpurpose.Emun;
using HearthStoneServer.Services;
using HearthStoneServer.Entity;
using Allpurpose.Utils;

namespace HearthStoneServer.Handler
{

    class FightInvitHandler : BaseHandler
    {
        public FightInvitHandler() {
            this.opCode = OPCode.FightInvit;
        }

        public override void doResponse(OperationRequest operationRequest, SendParameters sendParameters, MyClientPeer peer)
        {
            //信息提取
            Dictionary<byte, object> parameter = operationRequest.Parameters;
            String userName = (String)DictionaryUtils.getValue<byte, object>(parameter, (byte)ParameterCode.UserName);
            String anotherUserName = (String)DictionaryUtils.getValue<byte, object>(parameter, (byte)ParameterCode.AnotherUserName);

            //发送消息
            Dictionary<byte, object> dict = new Dictionary<byte, object>();
            dict.Add((byte)ParameterCode.UserName, userName);
            EventData data = new EventData((byte)EventCode.FightInvit, dict);
            DictionaryUtils.getValue<String, MyClientPeer>(Application.clientPeerDict, anotherUserName).SendEvent(data, sendParameters);

            //添加信息
            Application.Waiting.Add(userName, anotherUserName);
        }
    }
}
