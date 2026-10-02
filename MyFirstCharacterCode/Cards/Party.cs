using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.TestSupport;
using MyFirstCharacter.MyFirstCharacterCode.Cards;
using MyFirstCharacter.MyFirstCharacterCode.Powers;

namespace MyFirstCharacter.MyFirstCharacterCode.Cards;

public class Party() : MyFirstCharacterCard(1,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<CharmPower>(5)];

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
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (CombatState == null)
            return;
        foreach (Creature hittableEnemy in CombatState.HittableEnemies)
            await PowerCmd.Apply<PartyPower>(choiceContext, hittableEnemy, DynamicVars["CharmPower"].BaseValue, Owner.Creature, this);
        
        foreach (CardModel card in PileType.Hand.GetPile(Owner).Cards.Where(c => !c.EnergyCost.CostsX))
        {
            if (card.EnergyCost.GetWithModifiers(CostModifiers.None) >= 0)
            {
                card.EnergyCost.SetThisTurnOrUntilPlayed(NextEnergyCost());
                NCard.FindOnTable(card)?.PlayRandomizeCostAnim();
            }
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars["CharmPower"].UpgradeValueBy(2);
    }
}