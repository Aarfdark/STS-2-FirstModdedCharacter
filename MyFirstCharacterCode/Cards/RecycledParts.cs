using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using MyFirstCharacter.MyFirstCharacterCode.Cards;

namespace MyFirstCharacter.MyFirstCharacterCode.Cards;

public class RecycledParts() : MyFirstCharacterCard(1,
    CardType.Attack, CardRarity.Rare,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new CalculationBaseVar(4),
        new ExtraDamageVar(4),
        new CalculatedDamageVar(ValueProp.Move).WithMultiplier(
            ( card, _) =>
            {
                return card.Owner.PlayerCombatState != null ? card.Owner.PlayerCombatState.ExhaustPile.Cards.Count( c => c is Scrap) : 0;
            }
        )
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await DamageCmd.Attack(DynamicVars.CalculatedDamage).FromCard(this, play)
            .Targeting(play.Target!).WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["ExtraDamage"].UpgradeValueBy(3);
    }
}