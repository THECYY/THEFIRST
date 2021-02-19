using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Allpurpose.Enum
{
    [Serializable]
    public enum EventCode : byte
    {
        Ready,
        Left,
        FightInvit,
        FightInvitResponse,
        CancelFightInvit
    }
}
