using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MysqlManager.Emun
{
    [Serializable]
    public enum CardType : int
    {
        Servants,    //随从
        Spell,       //法术
        Weapons,     //武器
        Figure,      //人物
        Arcanum      //奥秘
    }
}
