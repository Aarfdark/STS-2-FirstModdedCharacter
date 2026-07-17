using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace MyFirstCharacter.MyFirstCharacterCode.Keywords;

public class Sempiternal() : CustomSingletonModel(HookType.Combat)
{
    public override async Task AfterAutoPostPlayPhaseEntered(PlayerChoiceContext choiceContext, Player? player)
    {
        if (player == null || PileType.Exhaust.GetPile(player).IsEmpty)
            return;
        
        List<CardModel> sempiternalCards = [];
        foreach (var card in PileType.Exhaust.GetPile(player).Cards)
            if (card.Keywords.Contains(OctaviaDangerKeywords.Sempiternal))
                sempiternalCards.Add(card);
        
        foreach (var card in sempiternalCards)
            await CardCmd.AutoPlay(choiceContext, card, null);
    }
}