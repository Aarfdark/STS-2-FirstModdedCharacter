using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.DevConsole.ConsoleCommands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Afflictions;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using MyFirstCharacter.MyFirstCharacterCode.Cards;

namespace MyFirstCharacter.MyFirstCharacterCode.Cards;

[Pool(typeof(CurseCardPool))]
public class Rot() : MyFirstCharacterCard(-1,
    CardType.Curse, CardRarity.Curse,
    TargetType.None)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<DemisePower>(1)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Unplayable];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DemisePower>()];
    public override int MaxUpgradeLevel => 0;
    public override bool HasTurnEndInHandEffect => true;
    
    protected override async Task OnTurnEndInHand(PlayerChoiceContext choiceContext)
    {
        await PowerCmd.Apply<DemisePower>(choiceContext, Owner.Creature, DynamicVars["DemisePower"].BaseValue, null, this);
    }
}