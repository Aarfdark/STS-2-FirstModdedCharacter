using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MyFirstCharacter.MyFirstCharacterCode.Relics;

namespace MyFirstCharacter.MyFirstCharacterCode.Relics;

public class DeckManipulation() : MyFirstCharacterRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Starter;
    
    bool _isCardMarked = false;
    private CardModel? _markedCard;
    public override Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
    {
        // grab first card drawn and mark it, flag future card draws on this turn
        if(!_isCardMarked)
            _markedCard = card;
        _isCardMarked = true;
        return base.AfterCardDrawn(choiceContext, card, fromHandDraw);
    }

    public override Task AfterAutoPrePlayPhaseEntered(PlayerChoiceContext choiceContext, Player player)
    {
        // give it retain
        CardCmd.ApplyKeyword(_markedCard!, CardKeyword.Retain);
        return base.AfterAutoPrePlayPhaseEntered(choiceContext, player);
    }

    public override Task AfterAutoPostPlayPhaseEntered(PlayerChoiceContext choiceContext, Player player)
    {
        // unflag for next turn
        _isCardMarked = false;
        return base.AfterAutoPostPlayPhaseEntered(choiceContext, player);
    }
}