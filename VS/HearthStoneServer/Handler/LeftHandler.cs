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
    public class LeftHandler : BaseHandler
    {
        public LeftHandler() {
            this.opCode = OPCode.Left;
        }

        public override void doResponse(OperationRequest operationRequest, SendParameters sendParameters, MyClientPeer peer)
        {
            //信息提取
            Dictionary<byte, object> parameter = operationRequest.Parameters;
            String userName = (String)DictionaryUtils.getValue<byte, object>(parameter, (byte)ParameterCode.UserName);

            //将用户名在List之中删除
            bool i = false;
            foreach (string s in Application.readyList) {
                if (s.Equals(userName)) {
                    i = true;
                    break;
                }
            }
            if (i)
            {
                Application.readyList.Remove(userName);

                //转化准备好的userNameList字符串
                String all = "";
                foreach (String s in Application.readyList)
                {
                    all += s + " ";
                }

                //通知其他所有准备好的客户端有新的客户端加入
                foreach (String s in Application.readyList)
                {
                    Dictionary<byte, object> dict = new Dictionary<byte, object>();
                    dict.Add((byte)ParameterCode.ReadyUserNameList, all);
                    EventData data = new EventData((byte)EventCode.Left, dict);
                    DictionaryUtils.getValue<String, MyClientPeer>(Application.clientPeerDict, s).SendEvent(data, sendParameters);
                }
            }
        }
    }
}
