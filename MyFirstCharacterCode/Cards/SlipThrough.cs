using MegaCrit.Sts2.Core.CardSelection;
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

public class SlipThrough() : MyFirstCharacterCard(1,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(8, ValueProp.Move), new CardsVar(1)];
    
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

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, play);

        CardModel? card;
        if (IsUpgraded)
        {
            CardSelectorPrefs prefs = new CardSelectorPrefs(SelectionScreenPrompt, DynamicVars.Cards.IntValue);
            card = (await CardSelectCmd.FromHand(choiceContext, Owner, prefs, 
                null, this)).FirstOrDefault();
        }
        else
        {
            card = Owner.RunState.Rng.CombatCardSelection.NextItem(PileType.Hand.GetPile(Owner).Cards);
        }
        
        if (card == null)
            return;
        
        if (card.EnergyCost.GetWithModifiers(CostModifiers.None) >= 0)
        {
            card.EnergyCost.SetThisTurnOrUntilPlayed(NextEnergyCost());
            NCard.FindOnTable(card)?.PlayRandomizeCostAnim();
        }
    }
    
    private int NextEnergyCost()
    {
        return TestEnergyCostOverride >= 0 ? TestEnergyCostOverride : Owner.RunState.Rng.CombatEnergyCosts.NextInt(4);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(2);
    }
}