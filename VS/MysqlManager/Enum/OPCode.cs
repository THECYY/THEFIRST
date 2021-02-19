using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MysqlManager.Enum
{
    [Serializable]
    public enum OPCode : byte
    {
        Login,
        Register,
        CheckCardSets,
        CheckCardsSet,
        ChangeCardsSet,
        DeleteCardsSet,
        CheckMyCards,
        ManufactureCard,
        DecomposeCard,
        DecomposeAllUselessCard,
        PurchaseCardPackage,
        UseCardPackage,
        Ready
    }
}
