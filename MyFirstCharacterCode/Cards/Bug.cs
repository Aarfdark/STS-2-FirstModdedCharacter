using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MyFirstCharacter.MyFirstCharacterCode.Cards;
using MyFirstCharacter.MyFirstCharacterCode.Powers;

namespace MyFirstCharacter.MyFirstCharacterCode.Cards;


public class Bug() : MyFirstCharacterCard(0,
    CardType.Skill, CardRarity.Common,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<CharmPower>(1)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<CharmPower>(choiceContext, play.Target!, DynamicVars["CharmPower"].BaseValue, Owner.Creature, this);
        var scrap = await Scrap.CreateInHand(Owner, 1, CombatState!);
        if (IsUpgraded)
            foreach (var card in scrap)
                CardCmd.Upgrade(card);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["CharmPower"].UpgradeValueBy(1);
    }
}