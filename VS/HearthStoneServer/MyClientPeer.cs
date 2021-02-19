using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photon.SocketServer;
using PhotonHostRuntimeInterfaces;
using HearthStoneServer;
using HearthStoneServer.Handler;
using Allpurpose.Utils;
using Allpurpose.Emun;
using Allpurpose.Entity;
using Allpurpose.Enum;

namespace HearthStoneServer
{
    public class MyClientPeer : ClientPeer
    {
        public String userName;

        public MyClientPeer(InitRequest initRequest) : base(initRequest) {
            this.userName = null;
        }

        protected override void OnDisconnect(DisconnectReason reasonCode, string reasonDetail)
        {
            try
            {
                Application.loginUserDict.Remove(this.userName);
                Application.clientPeerDict.Remove(this.userName);
            }
            catch (Exception e) {
                e.ToString();
            }
        }

        protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters)
        {
            BaseHandler handler = DictionaryUtils.getValue<OPCode, BaseHandler>(Application.handlerDict, (OPCode)operationRequest.OperationCode);
            if (handler != null)
            {
                handler.doResponse(operationRequest, sendParameters, this);
            }
        }
    }
}
