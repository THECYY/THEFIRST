using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Allpurpose.Enum;
using Allpurpose.Emun;
using Allpurpose.Entity;
using Photon.SocketServer;

namespace HearthStoneServer.Handler
{
    public abstract class BaseHandler
    {

        public OPCode opCode;

        public abstract void doResponse(OperationRequest operationRequest, SendParameters sendParameters, MyClientPeer peer);

    }
}
