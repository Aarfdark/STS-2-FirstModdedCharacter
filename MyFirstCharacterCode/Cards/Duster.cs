using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using MyFirstCharacter.MyFirstCharacterCode.Cards;
using MyFirstCharacter.MyFirstCharacterCode.Hooks;
using MyFirstCharacter.MyFirstCharacterCode.Keywords;
using MyFirstCharacter.MyFirstCharacterCode.Powers;

namespace MyFirstCharacter.MyFirstCharacterCode.Cards;

public class Duster() : MyFirstCharacterCard(1,
    CardType.Skill, CardRarity.Common,
    TargetType.Self), IOnCardPlayedViaAshbound
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(6, ValueProp.Move), new DynamicVar("Turns", 2)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [OctaviaDangerKeywords.Ashbound];
    private Decimal _blockAmount;
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        _blockAmount = await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, play);
    }

    public async Task OnCardPlayedViaAshbound(ICombatState combatState, PlayerChoiceContext choiceContext,
        CardModel card)
    {
        if (card != this)
            return;
        (await PowerCmd.Apply<DusterPower>(choiceContext, Owner.Creature, DynamicVars["Turns"].BaseValue, Owner.Creature, this))?.SetBlock(_blockAmount);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(3);
    }
}