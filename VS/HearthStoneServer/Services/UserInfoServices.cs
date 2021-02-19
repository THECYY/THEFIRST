using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using HearthStoneServer.Entity;
using HearthStoneServer.NHibernateManager;
using Allpurpose.Utils;
using Photon.SocketServer;

namespace HearthStoneServer.Services
{
    public class UserInfoServices
    {
        private static String DataRootPath = Path.Combine(Application.rootPath, "HearthStoneServer\\Data");

        //验证登录信息
        public static int verify(String userName, String password) {

            User user = UserManager.getByUserName((String)userName);
            if (user == null)
            {
                return 2;
            }
            else if (!user.Password.Equals((String)password))
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }

        

        //新用户注册
        public static int register(String userName, String password){

            //保证没有重复注册
            int hasExit = verify(userName, password);
            if (hasExit != 2) {
                return 0;
            } else {

                try
                {
                    //创建数据库映射对象
                    User user = new User() { UserName = userName, Password = password, Money = 1000, ArcaneDust = 1600 };
                    UserManager.Save(user);

                    //创建用户文件夹
                    String path = Path.Combine(DataRootPath, "UserInfo\\" + userName);
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    //创建用户三个文件
                    if (!File.Exists(Path.Combine(path, "CardInfo.txt")))
                    {
                        File.Create(Path.Combine(path, "CardInfo.txt")).Close();
                    }
                    if (!File.Exists(Path.Combine(path, "CardPackageInfo.txt")))
                    {
                        File.Create(Path.Combine(path, "CardPackageInfo.txt")).Close();
                    }
                    if (!File.Exists(Path.Combine(path, "CardSetsInfo.txt")))
                    {
                        File.Create(Path.Combine(path, "CardSetsInfo.txt")).Close();
                    }

                    //复制初始卡牌
                    FileUtils.copy(Path.Combine(DataRootPath, "NewUserCardInfo.txt"), Path.Combine(path, "CardInfo.txt"));
                    return 1;
                }
                catch (Exception e) {
                    return 2;
                }
            }
        }

        //封装用户文件
        public static User encapsulateAllUserInfo(String userName) {
            User user = UserManager.getByUserName(userName);
            if (user != null)
            {
                user.MyCards = UserManager.encapsulateCardInfo(user.UserName);
                user.MyCardsPackage = UserManager.encapsulateCardPackageInfo(user.UserName);
                user.CardSets = UserManager.encapsulateCardSetsInfo(user.UserName);
            }
            return user;
        }
    }
}
