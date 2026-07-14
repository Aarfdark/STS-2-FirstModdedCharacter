using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Random;
using MyFirstCharacter.MyFirstCharacterCode.Cards;

namespace MyFirstCharacter.MyFirstCharacterCode.Cards;

[Pool(typeof(ColorlessCardPool))]
public class Research() : MyFirstCharacterCard(2,
    CardType.Skill, CardRarity.Rare,
    TargetType.Self)
{
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (IsUpgraded)
        {
            CardSelectorPrefs prefs = new CardSelectorPrefs(CardSelectorPrefs.ExhaustSelectionPrompt, 1);
            CardModel? card = (await CardSelectCmd.FromHand(choiceContext, Owner, prefs, null, this)).FirstOrDefault();
            if (card == null)
                return;
            await CardCmd.Exhaust(choiceContext, card);
        }
        else
        {
            CardPile pile = PileType.Hand.GetPile(Owner);
            CardModel? card = Owner.RunState.Rng.CombatCardSelection.NextItem(pile.Cards);
            if (card == null)
                return;
            await CardCmd.Exhaust(choiceContext, card);
        }
        
        Rng combatCardSelection = Owner.RunState.Rng.CombatCardSelection;
        IReadOnlyList<CardModel> cardsInHand = PileType.Hand.GetPile(Owner).Cards;
        List<CardModel> validCards = cardsInHand.Where((Func<CardModel, bool>) (c => c.EnergyCost.GetWithModifiers(CostModifiers.None) > 0 || c.BaseStarCost > 0)).ToList();
        (((combatCardSelection.NextItem(validCards.Where((c => c.CostsEnergyOrStars(true)))) ?? combatCardSelection.NextItem(validCards.Where((c => c.CostsEnergyOrStars(true))))) ?? combatCardSelection.NextItem(validCards)) ?? combatCardSelection.NextItem(validCards))?.SetToFreeThisCombat();
        
    }
}