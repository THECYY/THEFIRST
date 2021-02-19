using Photon.SocketServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Allpurpose.Enum;
using Allpurpose.Emun;
using HearthStoneServer.Services;
using HearthStoneServer.Entity;
using Allpurpose.Utils;

namespace HearthStoneServer.Handler
{
    public class FightInvitResponseHandler : BaseHandler
    {
        public FightInvitResponseHandler() {
            this.opCode = OPCode.FightInvitResponse;
        }

        public override void doResponse(OperationRequest operationRequest, SendParameters sendParameters, MyClientPeer peer)
        {
            //信息提取
            Dictionary<byte, object> parameter = operationRequest.Parameters;
            String userName = (String)DictionaryUtils.getValue<byte, object>(parameter, (byte)ParameterCode.UserName);
            String anotherUserName = (String)DictionaryUtils.getValue<byte, object>(parameter, (byte)ParameterCode.AnotherUserName);
            bool fightInvitResponseResult = (bool)DictionaryUtils.getValue<byte, object>(parameter, (byte)ParameterCode.FightInvitResponseResult);

            //waiting应当删除键值对
            if (DictionaryUtils.getValue<String, String>(Application.Waiting, anotherUserName) != null) {
                Application.Waiting.Remove(anotherUserName);
            }

            if (fightInvitResponseResult)
            {
                //Fighting应当添加,List应当去除
                try
                {
                    Application.Fighting.Add(anotherUserName, userName);
                    Application.Fighting.Add(userName, anotherUserName);
                    Application.readyList.Remove(anotherUserName);
                    Application.readyList.Remove(userName);
                }
                catch (Exception e) {
                    e.ToString();
                }
            }
            else {
                
            }

            //发送消息
            Dictionary<byte, object> dict = new Dictionary<byte, object>();
            dict.Add((byte)ParameterCode.FightInvitResponseResult, fightInvitResponseResult);
            EventData data = new EventData((byte)EventCode.FightInvitResponse, dict);
            DictionaryUtils.getValue<String, MyClientPeer>(Application.clientPeerDict, anotherUserName).SendEvent(data, sendParameters);
        }
    }
}
