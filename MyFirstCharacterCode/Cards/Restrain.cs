using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MyFirstCharacter.MyFirstCharacterCode.Cards;
using MyFirstCharacter.MyFirstCharacterCode.Powers;

namespace MyFirstCharacter.MyFirstCharacterCode.Cards;


public class Restrain() : MyFirstCharacterCard(1,
    CardType.Skill, CardRarity.Common,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<CharmPower>(4), new PowerVar<VulnerablePower>(1)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<RestrainPower>(choiceContext, play.Target!, DynamicVars["CharmPower"].BaseValue,
            Owner.Creature, this);
        await PowerCmd.Apply<VulnerablePower>(choiceContext, play.Target!, DynamicVars["VulnerablePower"].BaseValue,
            Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["CharmPower"].UpgradeValueBy(2);
        DynamicVars["VulnerablePower"].UpgradeValueBy(1);
    }
}