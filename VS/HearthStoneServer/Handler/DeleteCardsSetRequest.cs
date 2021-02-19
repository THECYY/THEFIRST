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
using System.Xml.Serialization;
using System.IO;
using HearthStoneServer.NHibernateManager;

namespace HearthStoneServer.Handler
{
    public class DeleteCardsSetHandler : BaseHandler
    {
        public DeleteCardsSetHandler() {
            this.opCode = OPCode.DeleteCardsSet;
        }

        public override void doResponse(OperationRequest operationRequest, SendParameters sendParameters, MyClientPeer peer)
        {
            Dictionary<byte, object> responseParameter = new Dictionary<byte, object>();
            try
            {
                //获取并查询用户
                Dictionary<byte, object> parameters = operationRequest.Parameters;
                String userName = (String)DictionaryUtils.getValue<byte, object>(parameters, (byte)ParameterCode.UserName);
                int cardsSetId = (int)DictionaryUtils.getValue<byte, object>(parameters, (byte)ParameterCode.CardsSetId);

                //用户卡组变化
                User user = DictionaryUtils.getValue<String, User>(Application.loginUserDict, userName);
                user.CardSets.Remove(cardsSetId);

                //将变化保存到文件数据库
                UserManager.disassembleCardSetsInfo(user.UserName, user.CardSets);

                //保存用户
                Application.loginUserDict.Remove(user.UserName);
                Application.loginUserDict.Add(user.UserName, user);

                //封装信息
                responseParameter.Add((byte)ParameterCode.CardsSetOperationResult, 1);
            }
            catch (Exception e)
            {
                Application.logger.Info(e.ToString());
                responseParameter.Add((byte)ParameterCode.CardsSetOperationResult, 0);
            }

            OperationResponse response = new OperationResponse((byte)OPCode.DeleteCardsSet, responseParameter);
            peer.SendOperationResponse(response, sendParameters);
        }
    }
}
