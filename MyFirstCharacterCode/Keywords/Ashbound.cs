using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace MyFirstCharacter.MyFirstCharacterCode.Keywords;

public class Ashbound() : CustomSingletonModel(HookType.Combat)
{
    
    private readonly List<CardModel> _cardsPlayedViaAshbound = new(); // to store cards played
    private bool _ashboundWasPlayed = false;
    
    public override async Task AfterCardExhausted(
        PlayerChoiceContext choiceContext,
        CardModel card, 
        bool causedByEthereal)
    {
        if (causedByEthereal || card.CombatState == null)
            return;
        
        if (_ashboundWasPlayed)
        {
            _ashboundWasPlayed = false;
            return;
        }
        
        // TODO: fix exhaust bug

        if (card.Keywords.Contains(OctaviaDangerKeywords.Ashbound) && !_cardsPlayedViaAshbound.Contains(card))
        {
            _ashboundWasPlayed = true;
            _cardsPlayedViaAshbound.Add(card);
            await CardCmd.AutoPlay(choiceContext, card, null);
        }
    }

    public override CardLocation ModifyCardPlayResultLocation(CardModel card, bool isAutoPlay, ResourceInfo resources,
        CardLocation cardLocation)
    {
        if (_cardsPlayedViaAshbound.Contains(card))
        {
            cardLocation.pileType = PileType.Exhaust;
            cardLocation.position = CardPilePosition.Top;
        }
        return cardLocation;
    }

    public async override Task AfterModifyingCardPlayResultLocation(CardModel card, CardLocation cardLocation)
    {
        _cardsPlayedViaAshbound.Remove(card);
    }

    // public override (PileType, CardPilePosition) ModifyCardPlayResultPileTypeAndPosition(
    //     CardModel card,
    //     bool isAutoPlay,
    //     ResourceInfo resources,
    //     PileType pileType,
    //     CardPilePosition position)
    // {
    //     if (_cardsPlayedViaAshbound.Contains(card))
    //     {
    //         return (PileType.Exhaust, CardPilePosition.Top);
    //     }
    //
    //     return (pileType, position);
    // }
}