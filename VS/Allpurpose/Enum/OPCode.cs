using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Allpurpose.Enum
{
    [Serializable]
    public enum OPCode : byte
    {
        Login,
        Register,
        NewCardsSet,
        ChangeCardsSet,
        DeleteCardsSet,
        ManufactureCard,
        DecomposeCard,
        DecomposeAllUselessCard,
        PurchaseCardPackage,
        UseCardPackage,
        Ready,
        Left,
        FightInvit,
        FightInvitResponse,
        CancelFightInvit
    }
}
