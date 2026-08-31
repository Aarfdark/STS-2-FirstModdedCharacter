using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MyFirstCharacter.MyFirstCharacterCode.Cards;

namespace MyFirstCharacter.MyFirstCharacterCode.Cards;

public class Reduce() : MyFirstCharacterCard(0,
    CardType.Skill, CardRarity.Rare,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("Reduction", 1)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        var powersList = Owner.Creature.Powers.ToList();
        foreach (var power in powersList)
        {
            if (power is { StackType: PowerStackType.Counter, Type: PowerType.Debuff })
                await PowerCmd.ModifyAmount(choiceContext, power,
                    DynamicVars["Reduction"].BaseValue*-1, Owner.Creature, this);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Reduction"].UpgradeValueBy(1);
    }
}