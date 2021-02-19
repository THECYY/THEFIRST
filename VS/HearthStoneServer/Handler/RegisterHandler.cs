using Photon.SocketServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Allpurpose.Enum;
using HearthStoneServer.Services;
using Allpurpose.Utils;


namespace HearthStoneServer.Handler
{
    class RegisterHandler : BaseHandler
    {
        public RegisterHandler()
        {
            this.opCode = OPCode.Register;
        }

        public override void doResponse(OperationRequest operationRequest, SendParameters sendParameters, MyClientPeer peer)
        {
            //信息提取
            Dictionary<byte, object> parameter = operationRequest.Parameters;
            String userName = (String)DictionaryUtils.getValue<byte, object>(parameter, (byte)ParameterCode.UserName);
            String password = (String)DictionaryUtils.getValue<byte, object>(parameter, (byte)ParameterCode.Password);
            Application.logger.Info("===================" + userName + " " + password + "尝试登录===========================");

            //检查注册结果
            Dictionary<byte, object> responseParameter = new Dictionary<byte, object>();
            int result = UserInfoServices.register(userName, password);
            if (result == 0) {
                responseParameter.Add((byte)ParameterCode.RegisterResult, 0);
                Application.logger.Info("===================" + userName + " " + password + "注册失败===========================");
            } else if (result == 1) {
                responseParameter.Add((byte)ParameterCode.RegisterResult, 1);
                Application.logger.Info("===================" + userName + " " + password + "注册成功===========================");
            } else {
                responseParameter.Add((byte)ParameterCode.RegisterResult, 2);
                Application.logger.Info("===================" + userName + " " + password + "注册失败===========================");
            }

            //响应客户端
            OperationResponse operationResponse = new OperationResponse((byte)OPCode.Register, responseParameter);
            peer.SendOperationResponse(operationResponse, sendParameters);
        }
    }
}
