using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HearthStoneServer.Entity
{

    [Serializable]
    public class CardPackage
    {

        public String[] cards = new String[5];

        public bool[] isGolden = new bool[5];

        private int cardNumber = 0;

        public void addCard(String cardName, bool isGolden)
        {
            cards[cardNumber] = cardName;
            this.isGolden[cardNumber] = isGolden;
            cardNumber++;
        }

        public void removeCard()
        {
            if (cardNumber >= 1)
            {
                cards[cardNumber] = null;
                cardNumber--;
            }
        }
    }
}
