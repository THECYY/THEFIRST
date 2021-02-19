using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MysqlManager.Emun
{
    [Serializable]
    public enum Rarity : int 
    {
        Ordinary,  //普通
        Rare,      //稀有
        Epic,      //史诗
        Legend,    //传说
        Chaos      //混沌
    }
}
