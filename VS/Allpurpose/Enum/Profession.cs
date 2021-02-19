using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Allpurpose.Emun
{
    [Serializable]
    public enum Profession : int
    {
        Combatant,  //战士
        Warlock,    //术士
        Paladin,    //圣骑士
        Stalker,    //潜行者
        Pastor,     //牧师
        Shaman,     //萨满
        Druids,     //德鲁伊
        Ninmeod,    //猎人
        Necromancer,//法师
        DemonHunter,//恶魔猎手
        Neutral     //中立
    }
}
