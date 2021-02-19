using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Allpurpose.Emun;


namespace HearthStoneServer.Entity
{
    [Serializable]
    public class User
    {
        public virtual int Id{ get; set; }
        public virtual String UserName { get; set; }
        public virtual String Password { get; set; }
        public virtual int Money { get; set; }
        public virtual int ArcaneDust { get; set; }
        public virtual SerializableDictionary<Series, int> MyCardsPackage { get; set; }
        public virtual SerializableDictionary<String, int> MyCards { get; set; }
        public virtual SerializableDictionary<int, CardsSet> CardSets { get; set; }

    }
}
