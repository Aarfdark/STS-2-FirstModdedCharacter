using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.TestSupport;
using MegaCrit.Sts2.Core.ValueProps;
using MyFirstCharacter.MyFirstCharacterCode.Cards;

namespace MyFirstCharacter.MyFirstCharacterCode.Cards;

public class Scatter() : MyFirstCharacterCard(3,
    CardType.Attack, CardRarity.Common,
    TargetType.AllEnemies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(10, ValueProp.Move)];
    
    private int _testEnergyCostOverride = -1;
    public int TestEnergyCostOverride
    {
        get => _testEnergyCostOverride;
        set
        {
            TestMode.AssertOn();
            AssertMutable();
            _testEnergyCostOverride = value;
        }
    }
    
    private int NextEnergyCost()
    {
        return TestEnergyCostOverride >= 0 ? TestEnergyCostOverride : Owner.RunState.Rng.CombatEnergyCosts.NextInt(4);
    }
    
    public override async Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
    {
        if (card != this)
            return;
        card.EnergyCost.SetThisTurnOrUntilPlayed(NextEnergyCost());
        NCard.FindOnTable(card)?.PlayRandomizeCostAnim();
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (CombatState == null)
            return;
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this, play)
            .TargetingAllOpponents(CombatState).WithHitFx("vfx/vfx_attack_blunt")
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(4);
    }
}