using Allpurpose.Enum;
using Allpurpose.Utils;
using Photon.SocketServer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HearthStoneServer.Entity;
using System.Xml.Serialization;
using HearthStoneServer.NHibernateManager;
using Allpurpose.Emun;

namespace HearthStoneServer.Handler
{
    class UseCardPackageHandler : BaseHandler
    {
        public UseCardPackageHandler() {
            this.opCode = OPCode.UseCardPackage;
        }

        public override void doResponse(OperationRequest operationRequest, SendParameters sendParameters, MyClientPeer peer)
        {
            Dictionary<byte, object> dict = operationRequest.Parameters;
            Dictionary<byte, object> responseParameter = new Dictionary<byte, object>();

            try
            {
                //获取参数
                string userName = (string)DictionaryUtils.getValue<byte, object>(dict, (byte)ParameterCode.UserName);
                string packageString = (string)DictionaryUtils.getValue<byte, object>(dict, (byte)ParameterCode.CardPackage);
                Series series = (Series)DictionaryUtils.getValue<byte, object>(dict, (byte)ParameterCode.Series);

                Application.logger.Info("====================" + userName + packageString + "========================");

                //反序列化
                CardPackage package = null;
                using (StringReader reader = new StringReader(packageString))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(CardPackage));
                    package = (CardPackage)serializer.Deserialize(reader);
                }

                //修改user字典的信息
                User user = DictionaryUtils.getValue<String, User>(Application.loginUserDict, userName);
                int number = DictionaryUtils.getValue<Series, int>(user.MyCardsPackage, series);
                number--;
                if (number < 0) {
                    responseParameter.Add((byte)ParameterCode.UseCardPackageResult, 0);
                    goto End;
                }
                user.MyCardsPackage.Remove(series);
                if (number != 0)
                {
                    user.MyCardsPackage.Add(series, number);
                }
                for (int i = 0; i < 5; i++) {
                    if (package.isGolden[i])
                    {
                        int cardNumber = DictionaryUtils.getValue<String, int>(user.MyCards, package.cards[i]);
                        user.MyCards.Remove(package.cards[i]);
                        user.MyCards.Add(package.cards[i], cardNumber + 1000);
                    }
                    else {
                        int cardNumber = DictionaryUtils.getValue<String, int>(user.MyCards, package.cards[i]);
                        user.MyCards.Remove(package.cards[i]);
                        user.MyCards.Add(package.cards[i], cardNumber + 1);
                    }
                }
                Application.loginUserDict.Remove(userName);
                Application.loginUserDict.Add(userName, user);

                //修改文件数据库的信息
                UserManager.disassembleCardInfo(userName, user.MyCards);
                UserManager.disassembleCardPackageInfo(userName, user.MyCardsPackage);

                //响应信息
                responseParameter.Add((byte)ParameterCode.UseCardPackageResult, 1);
            }
            catch (Exception e) {
                Application.logger.Info("=====================" + e.ToString() + "==========================");
                responseParameter.Add((byte)ParameterCode.UseCardPackageResult, 0);
            }

            //响应客户端
            End: OperationResponse operationResponse = new OperationResponse((byte)OPCode.UseCardPackage, responseParameter);
            peer.SendOperationResponse(operationResponse, sendParameters);
        }
    }
}
