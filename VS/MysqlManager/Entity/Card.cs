using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MysqlManager.Emun;


namespace MysqlManager.Entity
{
    [Serializable]
    public class Card
    {
        public virtual int Id { set; get; }
        public virtual Series Series { set; get; }
        public virtual Profession Profession { set; get; }
        public virtual Rarity Rarity { set; get; }
        public virtual CardType CardType { set; get; }
        public virtual int Expend { set; get; }
        public virtual String Name { set; get; }
        public virtual String Description { set; get; }
        public virtual int Power { set; get; }
        public virtual int Blood { set; get; }
        public virtual Species Species { set; get; }
        public virtual bool IsGolden {set; get;}

        public Card() { }

        public Card(Series series, Profession profession, Rarity rarity, CardType cardType, int expend, String name, String description, int power, int blood, Species species) {
            this.Series = series; this.Profession = profession; this.Rarity = rarity; 
            this.CardType = cardType; this.Expend = expend; this.Name = name; this.Description = description;
            this.Power = power; this.Blood = blood; this.Species = species; 
        }

        public override string ToString()
        {
            return this.Id + " " + this.Series + " " + this.Profession + " " + this.Rarity + " " + this.CardType + " " + this.Expend + " " + 
                this.Name + " " + this.Description + " " + this.Power + " " + this.Blood + " " + this.Species;
        }

        public virtual string ToDataString()
        {
            return  (byte)this.Series + " " + (byte)this.Profession + " " + (byte)this.Rarity + " " + (byte)this.CardType + " " + this.Expend + " " +
                this.Name + " " + this.Description + " " + this.Power + " " + this.Blood + " " + (byte)this.Species;
        }

    }
}
