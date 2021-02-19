using Photon.SocketServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Allpurpose.Emun;
using Allpurpose.Enum;
using Allpurpose.Utils;
using HearthStoneServer.Entity;
using HearthStoneServer.NHibernateManager;

namespace HearthStoneServer.Handler
{
    class PurchaseCardPackageHandler : BaseHandler
    {
        public PurchaseCardPackageHandler() {
            this.opCode = OPCode.PurchaseCardPackage;
        }

        public override void doResponse(OperationRequest operationRequest, SendParameters sendParameters, MyClientPeer peer)
        {
            Dictionary<byte, object> requestParameter = operationRequest.Parameters;

            //拿到用户名，卡包类型，卡包数量
            String userName = (String)DictionaryUtils.getValue<byte, object>(requestParameter, (byte)ParameterCode.UserName);
            Series packageSeries = (Series)DictionaryUtils.getValue<byte, object>(requestParameter, (byte)ParameterCode.Series);
            int packageNumber = (int)DictionaryUtils.getValue<byte, object>(requestParameter, (byte)ParameterCode.PackageNumber);

            //操作用户的卡包字典
            User user = DictionaryUtils.getValue<String, User>(Application.loginUserDict, userName);
            SerializableDictionary<Series, int> packageInfoDict = user.MyCardsPackage;
            int allPackageNumber = packageNumber + DictionaryUtils.getValue<Series, int>(packageInfoDict, packageSeries);
            packageInfoDict.Remove(packageSeries);
            packageInfoDict.Add(packageSeries, allPackageNumber);

            //重新封装包信息
            bool success = UserManager.disassembleCardPackageInfo(userName, packageInfoDict);
            Dictionary<byte, object> responseParameter = new Dictionary<byte, object>();
            if (success)
            {
                DictionaryUtils.getValue<String, User>(Application.loginUserDict, userName).MyCardsPackage = packageInfoDict;

                //减少钱
                user.Money -= packageNumber * 100;
                UserManager.Update(user);

                //改变user
                user.MyCardsPackage = packageInfoDict;
                Application.loginUserDict.Remove(user.UserName);
                Application.loginUserDict.Add(user.UserName, user);

                //封装返回信息
                responseParameter.Add((byte)ParameterCode.PurchaseCardPackageResult, 1);
                
            }
            else {
                responseParameter.Add((byte)ParameterCode.PurchaseCardPackageResult, 0);
            }

            Application.logger.Info("===================" + user.UserName + "买了" + packageNumber + "包" + packageSeries + "系列的卡包，还剩" + user.Money + "=====================");

            //响应客户端
            OperationResponse operationResponse = new OperationResponse((byte)OPCode.PurchaseCardPackage, responseParameter);
            peer.SendOperationResponse(operationResponse, sendParameters);
        }
    }
}
