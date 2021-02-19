using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Allpurpose.Emun
{
    [Serializable]
    public enum CardType : int
    {
        Servant,     //随从
        Spell,       //法术
        Weapon,      //武器
        Figure,      //人物
        Arcanum,     //奥秘
        Task         //任务
    }
}
