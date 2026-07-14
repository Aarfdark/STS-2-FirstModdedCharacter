using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MyFirstCharacter.MyFirstCharacterCode.Cards;
using MyFirstCharacter.MyFirstCharacterCode.Keywords;

namespace MyFirstCharacter.MyFirstCharacterCode.Cards;


public class Climb() : MyFirstCharacterCard(1,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<StrengthPower>(1), new DynamicVar("Increase", 1)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<StrengthPower>(choiceContext, Owner.Creature, DynamicVars["StrengthPower"].BaseValue, Owner.Creature, this);
        var incStrength = DynamicVars["Increase"];
        var strengthValue = DynamicVars["StrengthPower"];
        strengthValue.BaseValue += incStrength.BaseValue;
        incStrength.BaseValue *= 2;
        EnergyCost.AddThisCombat(1);
    }

    protected override void OnUpgrade()
    {
        AddKeyword(OctaviaDangerKeywords.Ashbound);
    }
}