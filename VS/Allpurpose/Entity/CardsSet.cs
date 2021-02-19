using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Allpurpose.Enum;
using Allpurpose.Emun;

namespace Allpurpose.Entity
{
    [Serializable]
    public class CardsSet
    {

        public String Name { get; set; }

        public int CardCapable { get; set; }

        public bool Changeable { get; set; }

        public int Number { get; set; }

        public Profession profession;

        public GameMode gameMode;

        public String[] cards;

        public bool[] isGolden;

        public override string ToString()
        {
            String result = this.Name + " " + this.CardCapable + " " + this.Changeable + " " + this.Number + " " + (byte)this.profession + " " + (byte)this.gameMode;
            for (int i = 0; i < Number; i++) {
                result += " " + cards[i] + " ";
                if (isGolden[i])
                {
                    result += "true";
                }
                else
                {
                    result += "false";
                }
            }
            return result;
        }

        public CardsSet()
        {
            Changeable = false;
            cards = new String[30];
            isGolden = new bool[30];
            CardCapable = 30;
            Number = 0;
        }

        public CardsSet(int cardCapable, bool changeable)
        {
            CardCapable = cardCapable;
            this.Changeable = changeable;
            cards = new String[cardCapable];
            isGolden = new bool[cardCapable];
            Number = 0;
        }

        public CardsSet(CardsSet cardsSet, bool changeable)
        {
            for (int i = 0; i < cardsSet.CardCapable; i++)
            {
                cards[i] = cardsSet.cards[i];
                isGolden[i] = cardsSet.isGolden[i];
            }
            Number = cardsSet.Number;
            Changeable = changeable;
            Name = cardsSet.Name;
            CardCapable = cardsSet.CardCapable;
            profession = cardsSet.profession;
            gameMode = cardsSet.gameMode;
        }

        public void addCard(String cardName, bool golden)
        {
            if (Number < CardCapable)
            {
                cards[Number] = cardName;
                isGolden[Number] = golden;
                Number++;
            }
            else if (Number >= CardCapable && Changeable)
            {
                CardCapable++;
                String[] c = cards;
                bool[] g = isGolden;
                cards = new String[CardCapable];
                isGolden = new bool[CardCapable];
                for (int i = 0; i < CardCapable - 1; i++)
                {
                    cards[i] = c[i];
                    isGolden[i] = g[i];
                }
                cards[Number] = cardName;
                isGolden[Number] = golden;
                Number++;
            }
        }

        public void removeCard(String cardName, bool golden)
        {
            for (int i = 0; i < Number; i++)
            {
                if (cards[i].Equals(cardName) && isGolden[i].Equals(golden))
                {
                    for (int k = i; k < Number - 1; k++)
                    {
                        cards[k] = cards[k + 1];
                        isGolden[k] = isGolden[k + 1];
                    }
                    cards[Number - 1] = null;
                    isGolden[Number - 1] = false;
                    Number--;
                    return;
                }
            }
        }
    }
}
