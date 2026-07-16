using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MyFirstCharacter.MyFirstCharacterCode.Powers;

namespace MyFirstCharacter.MyFirstCharacterCode.Cards;

public class Flirt() : MyFirstCharacterCard(1,
    CardType.Skill, CardRarity.Common,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<WeakPower>(1), new PowerVar<CharmPower>(4)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (play.Target == null)
            return;
        await PowerCmd.Apply<WeakPower>(choiceContext, play.Target, DynamicVars.Weak.BaseValue, Owner.Creature, (CardModel) this);
        await PowerCmd.Apply<FlirtPower>(choiceContext, play.Target, DynamicVars["CharmPower"].BaseValue, Owner.Creature, (CardModel) this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["CharmPower"].UpgradeValueBy(3);
    }
}