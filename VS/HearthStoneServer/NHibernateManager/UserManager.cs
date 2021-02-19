using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Criterion;
using HearthStoneServer.Entity;
using System.IO;
using Allpurpose.Emun;
using Allpurpose.Enum;
using Allpurpose.Utils;
using System.Xml.Serialization;

namespace HearthStoneServer.NHibernateManager
{
    class UserManager
    {
        private static String DataRootPath = Path.Combine(Application.rootPath, "HearthStoneServer\\Data");

        //useful
        public static void Save(User user)
        {
            using (ISession session = NHibernateHelper.getFactory().OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Save(user);
                    transaction.Commit();
                }
            }
        }

        //useful
        public static void Update(User user)
        {

            using (ISession session = NHibernateHelper.getFactory().OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    if (UserManager.getById(user.Id) != null)
                    {
                        session.Update(user);
                    }
                    transaction.Commit();
                }
            }
        }

        public static void Delete(User user)
        {
            using (ISession session = NHibernateHelper.getFactory().OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    if (UserManager.getById(user.Id) != null)
                    {
                        session.Delete(user);
                    }
                    transaction.Commit();
                }
            }
        }

        public static User getById(int id)
        {
            using (ISession session = NHibernateHelper.getFactory().OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    User user = session.Get<User>(id);
                    transaction.Commit();
                    return user;
                }
            }
        }


        public static IList<User> getAllUser()
        {
            using (ISession session = NHibernateHelper.getFactory().OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    ICriteria criteria = session.CreateCriteria(typeof(User));
                    IList<User> users = criteria.List<User>();
                    transaction.Commit();
                    return users;
                }
            }
        }


        //Useful
        public static User getByUserName(String name)
        {
            using (ISession session = NHibernateHelper.getFactory().OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    ICriteria criteria = session.CreateCriteria(typeof(User)).Add(Restrictions.Eq("UserName", name));
                    User users = (User)criteria.UniqueResult();
                    transaction.Commit();
                    return users;
                }
            }
        }

        //封装用户Card信息
        public static SerializableDictionary<String, int> encapsulateCardInfo(String userName)
        {
            SerializableDictionary<String, int> dict = null;
            String path = Path.Combine(DataRootPath, "UserInfo\\" + userName + "\\CardInfo.txt");
            if (File.Exists(path))
            {
                using (StreamReader reader = new StreamReader(new FileStream(path, FileMode.Open), Encoding.GetEncoding("Gb2312"))) {

                    dict = new SerializableDictionary<string, int>();
                    String line;
                    while ((line = reader.ReadLine()) != null && line != " ") 
                    {
                        String[] cardNameAndNumber = line.Split(' ');
                        if (!cardNameAndNumber[0].Equals("") && cardNameAndNumber[0] != null)
                        {
                            dict.Add(cardNameAndNumber[0], int.Parse(cardNameAndNumber[1]));
                        }
                    }
                
                }
            }
            return dict;
        }

        //封装用户CardPackage信息
        public static SerializableDictionary<Series, int> encapsulateCardPackageInfo(String userName)
        {
            SerializableDictionary<Series, int> dict = new SerializableDictionary<Series, int>();
            int counter = 0;
            String path = Path.Combine(DataRootPath, "UserInfo\\" + userName + "\\CardPackageInfo.txt");
            if (File.Exists(path))
            {
                using (StreamReader reader = new StreamReader(new FileStream(path, FileMode.Open), Encoding.GetEncoding("Gb2312")))
                {

                    String line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        counter++;
                        String[] cardNameAndNumber = line.Split(' ');
                        dict.Add((Series)int.Parse(cardNameAndNumber[0]), int.Parse(cardNameAndNumber[1]));
                    }

                }
            }
            if (counter == 0) {
                dict.Add(Series.Classic, 0);
            }
            return dict;
        }

        //封装用户CardSets信息
        public static SerializableDictionary<int, CardsSet> encapsulateCardSetsInfo(String userName)
        {
            SerializableDictionary<int, CardsSet> dict = new SerializableDictionary<int, CardsSet>();
            int number = 1;
            String path = Path.Combine(DataRootPath, "UserInfo\\" + userName + "\\CardSetsInfo.txt");
            if (File.Exists(path))
            {
                using (StreamReader reader = new StreamReader(new FileStream(path, FileMode.Open), Encoding.GetEncoding("Gb2312")))
                {
                    CardsSet cardsSet = null;
                    String line = null;
                    while ((line = reader.ReadLine()) != null) {
                        String[] cardNameAndNumber = line.Split(' ');
                        if (cardNameAndNumber[0].Equals("CardsSet"))
                        {
                            cardsSet = new CardsSet(30, true);
                            cardsSet.Name = cardNameAndNumber[1];
                            cardsSet.profession = (Profession)int.Parse(cardNameAndNumber[2]);
                            cardsSet.gameMode = (GameMode)int.Parse(cardNameAndNumber[3]);
                        }
                        else if (cardNameAndNumber[0].Equals("*CardsSet")) {
                            if (cardsSet.Number <= 30) {
                                cardsSet.Changeable = false;
                            }
                            dict.Add(number, cardsSet);
                            number++;
                        } else {
                            cardsSet.addCard(cardNameAndNumber[0], bool.Parse(cardNameAndNumber[1]));
                        }
                    }
                }
            }
            if (number == 1)
            {
                dict.Add(1, null);
            }
            return dict;
        }

        //拆解卡包信息
        public static bool disassembleCardPackageInfo(String userName, SerializableDictionary<Series, int> dict) {
            String path = Path.Combine(DataRootPath, "UserInfo\\" + userName + "\\CardPackageInfo.txt");
            try
            {
                File.WriteAllText(path, String.Empty);
                using (StreamWriter writer = new StreamWriter(new FileStream(path, FileMode.Open), Encoding.GetEncoding("gb2312")))
                {
                    foreach (Series series in dict.Keys) {
                        String data = (byte)series + " " + DictionaryUtils.getValue<Series, int>(dict, series);
                        writer.WriteLine(data);
                    }
                }
                return true;
            }
            catch (Exception e) {
                Application.logger.Info(e.ToString());
                return false;
            }
        }

        //重新封装卡牌信息
        public static bool disassembleCardInfo(String userName, SerializableDictionary<String, int> dict)
        {
            String path = Path.Combine(DataRootPath, "UserInfo\\" + userName + "\\CardInfo.txt");
            try
            {
                File.WriteAllText(path, String.Empty);
                using (StreamWriter writer = new StreamWriter(new FileStream(path, FileMode.Open), Encoding.GetEncoding("gb2312")))
                {
                    foreach (String cardName in dict.Keys)
                    {
                        String data = cardName + " " + DictionaryUtils.getValue<String, int>(dict, cardName);
                        writer.WriteLine(data);
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                Application.logger.Info(e.ToString());
                return false;
            }
        }

        //重新封装卡组
        public static bool disassembleCardSetsInfo(String userName, SerializableDictionary<int, CardsSet> dict)
        {
            String path = Path.Combine(DataRootPath, "UserInfo\\" + userName + "\\CardSetsInfo.txt");
            try
            {
                File.WriteAllText(path, String.Empty);
                using (StreamWriter writer = new StreamWriter(new FileStream(path, FileMode.Open), Encoding.GetEncoding("gb2312")))
                {
                    foreach (int cardSetNumber in dict.Keys)
                    {
                        CardsSet cardsSet = DictionaryUtils.getValue<int, CardsSet>(dict, cardSetNumber);
                        writer.WriteLine("CardsSet" + " " + cardsSet.Name + " " + (byte)cardsSet.profession + " " + (byte)cardsSet.gameMode);
                        for (int i = 0; i < cardsSet.CardCapable; i++) {
                            if (cardsSet.cards[i] != null && cardsSet.cards[i] != "")
                            {
                                writer.WriteLine(cardsSet.cards[i] + " " + cardsSet.isGolden[i]);
                            }
                            else {
                                break;
                            }
                        }
                        writer.WriteLine("*CardsSet");
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                Application.logger.Info(e.ToString());
                return false;
            }
        }

        //User字符串化
        public static String praseUserToUserString(User user)
        {
            String userString = user.UserName + ";" + user.Password + ";" + user.Money + ";" + user.ArcaneDust + ";";
            foreach (Series s in user.MyCardsPackage.Keys)
            {
                userString += (byte)s + ":" + DictionaryUtils.getValue<Series, int>(user.MyCardsPackage, s) + ",";
            }
            userString += ";";
            foreach (String s in user.MyCards.Keys)
            {
                userString += s + ":" + DictionaryUtils.getValue<String, int>(user.MyCards, s) + ",";
            }
            userString += ";";
            foreach (int s in user.CardSets.Keys)
            {
                CardsSet cardsSet = DictionaryUtils.getValue<int, CardsSet>(user.CardSets, s);
                if (cardsSet != null)
                {
                    userString += s + ":" + cardsSet.ToString() + ",";
                }
            }
            return userString;
        }

        //字符串User化
        public static User praseUserStringToUser(String userString)
        {
            User user = new User();
            String[] info = userString.Split(';');

            user.UserName = info[0];
            user.Password = info[1];
            user.Money = int.Parse(info[2]);
            user.ArcaneDust = int.Parse(info[3]);
            user.MyCardsPackage = new SerializableDictionary<Series, int>();
            user.MyCards = new SerializableDictionary<string, int>();
            user.CardSets = new SerializableDictionary<int, CardsSet>();

            if (info[4] != null && !info[4].Equals(""))
            {
                String[] cardsPackage = info[4].Split(',');
                for (int i = 0; i < cardsPackage.Length - 1; i++)
                {
                    String[] p = cardsPackage[i].Split(':');
                    user.MyCardsPackage.Add((Series)int.Parse(p[0]), int.Parse(p[1]));
                }
            }

            String[] cards = info[5].Split(',');
            for (int i = 0; i < cards.Length - 1; i++)
            {
                String[] p = cards[i].Split(':');
                user.MyCards.Add(p[0], int.Parse(p[1]));
            }

            if (info[6] != null && !info[6].Equals(""))
            {
                String[] cardsSet = info[6].Split(',');
                for (int i = 0; i < cardsSet.Length - 1; i++)
                {
                    String[] p = cardsSet[i].Split(':');
                    String[] s = p[1].Split(' ');
                    CardsSet set = new CardsSet();
                    set.Name = s[0];
                    set.CardCapable = int.Parse(s[1]);
                    set.Changeable = bool.Parse(s[2]);
                    set.Number = int.Parse(s[3]);
                    set.profession = (Profession)int.Parse(s[4]);
                    set.gameMode = (GameMode)int.Parse(s[5]);
                    for (int k = 0; k < set.Number; k++)
                    {
                        set.cards[k] = s[6 + 2 * k];
                        set.isGolden[k] = bool.Parse(s[7 + 2 * k]);
                    }
                    user.CardSets.Add(int.Parse(p[0]), set);
                }
            }

            return user;
        }
    }
}
