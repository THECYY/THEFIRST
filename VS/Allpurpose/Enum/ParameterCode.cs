using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Allpurpose.Enum
{
    [Serializable]
    public enum ParameterCode : byte
    {
        LoginResult,
        User,
        RegisterResult,
        UserName,
        Password,
        CardsSetId,
        CardsSet,
        UserCardsDictionary,
        CardName,
        IsColden,
        ArcaneDust,
        Series,
        PackageNumber,
        CardPackage,
        PurchaseCardPackageResult,
        UseCardPackageResult,
        CardOperationResult,
        CardsSetOperationResult,
        ReadyUserNameList,
        FightInvitResponseResult,
        AnotherUserName
    }
}
