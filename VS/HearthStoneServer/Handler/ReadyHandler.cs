using Photon.SocketServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Allpurpose.Enum;
using Allpurpose.Utils;

namespace HearthStoneServer.Handler
{
    public class ReadyHandler : BaseHandler
    {
        public ReadyHandler() {
            this.opCode = OPCode.Ready;
        }

        public override void doResponse(OperationRequest operationRequest, SendParameters sendParameters, MyClientPeer peer)
        {
            //信息提取
            Dictionary<byte, object> parameter = operationRequest.Parameters;
            String userName = (String)DictionaryUtils.getValue<byte, object>(parameter, (byte)ParameterCode.UserName);

            bool has = false;
            foreach(String s in Application.readyList)
            {
                if (s.Equals(userName)) {
                    has = true;
                }
            }
            if (!has)
            {
                //将用户名添加到List之中
                Application.readyList.Add(userName);
            }

            //转化准备好的userNameList字符串
            String all = "";
            foreach (String s in Application.readyList) {
                all += s + " ";
            }

            Dictionary<byte, object> responseParameter = new Dictionary<byte, object>();
            responseParameter.Add((byte)ParameterCode.ReadyUserNameList, all);

            //通知其他所有准备好的客户端有新的客户端加入
            foreach (String s in Application.readyList)
            {
                if (!s.Equals(userName))
                {
                    Dictionary<byte, object> dict = new Dictionary<byte, object>();
                    dict.Add((byte)ParameterCode.ReadyUserNameList, all);
                    EventData data = new EventData((byte)EventCode.Ready, dict);
                    DictionaryUtils.getValue<String, MyClientPeer>(Application.clientPeerDict, s).SendEvent(data, sendParameters);
                }
            }

            //响应客户端
            OperationResponse operationResponse = new OperationResponse((byte)OPCode.Ready, responseParameter);
            peer.SendOperationResponse(operationResponse, sendParameters);
        }
    }
}
