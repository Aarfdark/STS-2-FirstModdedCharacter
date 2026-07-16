using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Potions;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.TestSupport;
using MyFirstCharacter.MyFirstCharacterCode.Cards;

namespace MyFirstCharacter.MyFirstCharacterCode.Cards;

public class Shots() : MyFirstCharacterCard(0,
    CardType.Skill, CardRarity.Rare,
    TargetType.AllEnemies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<DemisePower>(8), new IntVar("PlayMax", 0)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
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
        // check if it's the first card played this turn
        if (CombatManager.Instance.History.CardPlaysFinished.Count((
            e => e.HappenedThisTurn(CombatState) && 
            e.CardPlay.Card.Owner == Owner)) > DynamicVars["PlayMax"].IntValue)
        {
            return;
        }

        if (CombatState == null)
            return;
        
        foreach (Creature hittableEnemy in CombatState.HittableEnemies)
        {
            await PowerCmd.Apply<DemisePower>(choiceContext, hittableEnemy, DynamicVars.Power<DemisePower>().BaseValue, Owner.Creature, this);
        }

        foreach (CardModel card in PileType.Hand.GetPile(Owner).Cards.Where((c => !c.EnergyCost.CostsX)))
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
        DynamicVars.Power<DemisePower>().UpgradeValueBy(4);
    }
}