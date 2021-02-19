using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HearthStoneServer.Handler;
using Photon.SocketServer;
using Allpurpose.Enum;
using Allpurpose.Emun;
using HearthStoneServer.Services;
using HearthStoneServer.Entity;
using Allpurpose.Utils;
using System.Xml.Serialization;
using System.IO;
using HearthStoneServer.NHibernateManager;

namespace HearthStoneServer.Handler
{
    class LoginHandler : BaseHandler
    {
        public LoginHandler() {
            this.opCode = OPCode.Login;
        }

        public override void doResponse(OperationRequest operationRequest, SendParameters sendParameters, MyClientPeer peer)
        {
            //获取并查询用户
            Dictionary<byte, object> parameters = operationRequest.Parameters;
            String userName = (String)DictionaryUtils.getValue<byte, object>(parameters, (byte)ParameterCode.UserName);
            String password = (String)DictionaryUtils.getValue<byte, object>(parameters, (byte)ParameterCode.Password);
            Application.logger.Info("===================" + userName + " " + password + "尝试登录===========================");

            //检查登录结果
            int result = UserInfoServices.verify(userName, password);
            if (result == 2)
            {
                Application.logger.Info("===================" + userName + " " + password + "登录失败(账号不存在)===========================");
            }
            else if (result == 1)
            {
                Application.logger.Info("===================" + userName + " " + password + "登录成功===========================");
            }
            else {
                Application.logger.Info("===================" + userName + " " + password + "登录失败(密码错误)===========================");
            }

            //封装所有信息
            User user = null;
            String userString = null;
            if (result == 1) {
                user = UserInfoServices.encapsulateAllUserInfo(userName);

                //存放在loginUser字典中
                if (DictionaryUtils.getValue<String, User>(Application.loginUserDict, userName) != null) {
                    Application.loginUserDict.Remove(userName);
                }
                Application.loginUserDict.Add(user.UserName, user);

                //尝试序列化
                userString = UserManager.praseUserToUserString(user);
            }

            //开始封装返回参数
            Dictionary<byte, object> responseParameters = new Dictionary<byte, object>();
            responseParameters.Add((byte)ParameterCode.LoginResult, result);
            responseParameters.Add((byte)ParameterCode.User, userString);

            //将用户名保存到客户端对象之中
            if (user != null)
            {
                peer.userName = user.UserName;
                if (DictionaryUtils.getValue<String, MyClientPeer>(Application.clientPeerDict, user.UserName) != null)
                {
                    Application.clientPeerDict.Remove(userName);
                }
                Application.clientPeerDict.Add(userName, peer);
            }

            //发送响应
            OperationResponse operationResponse = new OperationResponse((byte)this.opCode, responseParameters);
            peer.SendOperationResponse(operationResponse, sendParameters);
        }

    }
}
