using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Text.Encodings.Web;
using MysqlManager.Entity;
using MysqlManager.NHibernateManager;
using MysqlManager.Emun;

namespace MysqlManager.Utils
{
    public class SqlFileUtile
    {
        private static String DataRootPath = "E:\\photon\\Photon-OnPremise-Server-SDK_v4-0-29-11263\\deploy\\HearthStoneServer\\Data";

        public static void praseCardMysqlToFile() {
            String path = Path.Combine(DataRootPath, "CardRepertory.txt");
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            try {
                File.WriteAllText(path, String.Empty);
                if (File.Exists(path))
                {
                    using (StreamWriter writer = new StreamWriter(new FileStream(path, FileMode.Open), Encoding.GetEncoding("GB2312")))
                    {
                        IList<Card> cards = CardManager.getAllCard();
                        foreach (Card card in cards)
                        {
                            writer.WriteLine(card.ToDataString());
                        }
                    }
                }
            }
            catch (Exception e) {
                Console.WriteLine(e.ToString());
            }
        }

        public static void praseFileToCardMysql()
        {
            String path = Path.Combine(DataRootPath, "CardRepertory.txt");
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            try
            {
                int counter = 0;
                if (File.Exists(path))
                {
                    using (StreamReader reader = new StreamReader(new FileStream(path, FileMode.Open), Encoding.GetEncoding("GB2312")))
                    {
                        List<Card> cards = new List<Card>();
                        String line = null;
                        while ((line = reader.ReadLine()) != null)
                        {
                            counter++;
                            String[] s = line.Split(" ");
                            Card card = new Card();

                            card.Series = (Series)int.Parse(s[0]);
                            card.Profession = (Profession)int.Parse(s[1]);
                            card.Rarity = (Rarity)int.Parse(s[2]);
                            card.CardType = (CardType)int.Parse(s[3]);
                            card.Expend = int.Parse(s[4]);
                            card.Name = s[5];
                            card.Description = s[6];
                            card.Power = int.Parse(s[7]);
                            card.Blood = int.Parse(s[8]);
                            card.Species = (Species)int.Parse(s[9]);

                            Console.WriteLine(card.ToDataString());
                            Console.WriteLine(card.Description);
                            cards.Add(card);
                            Console.WriteLine(counter);
                        }
                        CardManager.SaveAll(cards);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        //玩家初始卡牌信息
        public static void NewUserCardInfo()
        {
            IList<Card> cards = CardManager.getAllCard();
            String path = "E:\\photon\\Photon-OnPremise-Server-SDK_v4-0-29-11263\\deploy\\HearthStoneServer\\Data\\NewUserCardInfo.txt";
            try
            {
                File.WriteAllText(path, String.Empty);
                using (StreamWriter writer = new StreamWriter(new FileStream(path, FileMode.Open), Encoding.GetEncoding("gb2312")))
                {
                    foreach (Card c in cards)
                    {
                        if (c.Series.Equals(Series.Basic))
                        {
                            writer.WriteLine(c.Name + " " + 2002);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
