using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MysqlManager.Emun
{
    [Serializable]
    public enum Species : int
    {
        Mechanical,    //机械
        Pirate,        //海盗
        Dragon,        //龙
        Element,       //元素 
        Beast,         //野兽
        Totme,         //图腾
        Devil,         //恶魔
        FishMen,       //鱼人
        All,           //全部
        Null           //没有种族
    }
}
