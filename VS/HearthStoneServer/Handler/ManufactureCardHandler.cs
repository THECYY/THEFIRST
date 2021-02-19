using Photon.SocketServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HearthStoneServer.Handler;
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
    public class ManufactureCardHandler : BaseHandler
    {
        public ManufactureCardHandler() {
            this.opCode = OPCode.ManufactureCard;
        }

        public override void doResponse(OperationRequest operationRequest, SendParameters sendParameters, MyClientPeer peer)
        {
            Dictionary<byte, object> responseParameter = new Dictionary<byte, object>();
            try
            {
                //获取并查询用户
                Dictionary<byte, object> parameters = operationRequest.Parameters;
                String userName = (String)DictionaryUtils.getValue<byte, object>(parameters, (byte)ParameterCode.UserName);
                String cardString = (String)DictionaryUtils.getValue<byte, object>(parameters, (byte)ParameterCode.CardName);
                String[] info = cardString.Split(' ');
                Card card = new Card((Series)int.Parse(info[0]), (Profession)int.Parse(info[1]), (Rarity)int.Parse(info[2]), (CardType)int.Parse(info[3])
                    , int.Parse(info[4]), info[5], info[6], int.Parse(info[7]), int.Parse(info[8]), (Species)int.Parse(info[9]));
                bool isgolden = (bool)DictionaryUtils.getValue<byte, object>(parameters, (byte)ParameterCode.IsColden);

                //用户卡牌变化
                User user = DictionaryUtils.getValue<String, User>(Application.loginUserDict, userName);
                int cardNumber = DictionaryUtils.getValue<String, int>(user.MyCards, card.Name);
                if (cardNumber == default(int))
                {
                    cardNumber = 0;
                }
                else
                {
                    user.MyCards.Remove(card.Name);
                }
                if (isgolden)
                {
                    cardNumber += 1000;
                }
                else
                {
                    cardNumber += 1;
                }
                user.MyCards.Add(card.Name, cardNumber);

                //将变化保存到文件数据库
                UserManager.disassembleCardInfo(user.UserName, user.MyCards);

                //用户奥术之尘变化
                user.ArcaneDust -= DataUtils.getNumberOfArcaneDustWhenManufacture(card.Rarity, isgolden);

                //将变化保存到数据库
                UserManager.Update(user);

                //保存用户
                Application.loginUserDict.Remove(user.UserName);
                Application.loginUserDict.Add(user.UserName, user);

                //封装信息
                responseParameter.Add((byte)ParameterCode.CardOperationResult, 1);
            }
            catch (Exception e) {
                Application.logger.Info(e.ToString());
                responseParameter.Add((byte)ParameterCode.CardOperationResult, 0);
            }

            OperationResponse response = new OperationResponse((byte)OPCode.ManufactureCard, responseParameter);
            peer.SendOperationResponse(response, sendParameters);
        }
    }
}
