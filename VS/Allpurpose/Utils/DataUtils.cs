using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Allpurpose.Emun;
using Allpurpose.Enum;
using Allpurpose.Entity;
using System.IO;
using System.Xml.Serialization;

namespace Allpurpose.Utils
{
    public class DataUtils
    {
        public static String praseSpeciesToString(Species species) {
            switch (species) {
                case (Species.All): return "全部";
                case (Species.Beast): return "野兽";
                case (Species.Devil): return "恶魔";
                case (Species.Dragon): return "龙";
                case (Species.Element): return "元素";
                case (Species.FishMen): return "鱼人";
                case (Species.Mechanical): return "机械";
                case (Species.Pirate): return "海盗";
                case (Species.Totme): return "图腾";
                default: return "";
            }
        }

        public static String praseProfessionToString(Profession profession) {
            switch (profession) {
                case (Profession.Combatant): return "战士";
                case (Profession.DemonHunter): return "恶魔猎手";
                case (Profession.Druids): return "德鲁伊";
                case (Profession.Necromancer): return "法师";
                case (Profession.Ninmeod): return "猎人";
                case (Profession.Paladin): return "圣骑士";
                case (Profession.Pastor): return "牧师";
                case (Profession.Shaman): return "萨满";
                case (Profession.Stalker): return "潜行者";
                case (Profession.Warlock): return "术士";
                default: return "中立";
            }
        }

        public static String praseSeriesToString(Series series)
        {
            switch (series)
            {
                case (Series.AdventurersAssociation): return "探险者协会";
                case (Series.AGLOR): return "勇闯安戈洛";
                case (Series.Basic): return "经典";
                case (Series.BlackStoneMountain): return "黑石山的火焰";
                case (Series.Classic): return "经典";
                case (Series.ComingDragon): return "巨龙降临";
                case (Series.DarkMoon): return "暗月马戏团";
                case (Series.Gadgetzan): return "龙争虎斗加基森";
                case (Series.GoblinsVSGnomes): return "地精大战侏儒";
                case (Series.Hastahar): return "哈斯塔哈大乱斗";
                case (Series.Karazan): return "卡拉赞之夜";
                case (Series.Kobold): return "狗头人与地下世界";
                case (Series.Naxsamas): return "纳克萨玛斯";
                case (Series.Odum): return "奥丹姆奇兵";
                case (Series.OldGods): return "上古之神的低语";
                case (Series.OutlandAshes): return "外语的灰烬";
                case (Series.PopPlain): return "砰砰计划";
                case (Series.PTSL): return "冠军的试炼";
                case (Series.RiseShadow): return "暗影崛起";
                case (Series.Scholomance): return "通灵学院";
                case (Series.WitchForest): return "女巫森林";
                default: return null;
            }
        }

        public static int getNumberOfArcaneDustWhenManufacture(Rarity rarity, bool isgolden) {
            switch (rarity) {
                case Rarity.Ordinary:
                    if (isgolden)
                    {
                        return 400;
                    }
                    else {
                        return 40;
                    }
                case Rarity.Rare:
                    if (isgolden)
                    {
                        return 800;
                    }
                    else
                    {
                        return 100;
                    }
                case Rarity.Epic:
                    if (isgolden)
                    {
                        return 1600;
                    }
                    else
                    {
                        return 400;
                    }
                case Rarity.Legend:
                    if (isgolden)
                    {
                        return 3200;
                    }
                    else
                    {
                        return 1600;
                    }
                case Rarity.Chaos:
                    return 3200;
                default: return 0;
            }
        }

        public static int getNumberOfArcaneDustWhenDecompose(Rarity rarity, bool isgolden)
        {
            switch (rarity)
            {
                case Rarity.Ordinary:
                    if (isgolden)
                    {
                        return 50;
                    }
                    else
                    {
                        return 5;
                    }
                case Rarity.Rare:
                    if (isgolden)
                    {
                        return 100;
                    }
                    else
                    {
                        return 20;
                    }
                case Rarity.Epic:
                    if (isgolden)
                    {
                        return 400;
                    }
                    else
                    {
                        return 100;
                    }
                case Rarity.Legend:
                    if (isgolden)
                    {
                        return 1600;
                    }
                    else
                    {
                        return 400;
                    }
                case Rarity.Chaos:
                    return 1600;
                default: return 0;
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
