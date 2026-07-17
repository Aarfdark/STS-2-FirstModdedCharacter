using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace MyFirstCharacter.MyFirstCharacterCode.Keywords;

public class Rigged() : CustomSingletonModel(HookType.Combat)
{
    public override async Task AfterAutoPostPlayPhaseEntered(PlayerChoiceContext choiceContext, Player? player)
    {
        if (player == null || PileType.Draw.GetPile(player).IsEmpty)
            return;
        if (PileType.Draw.GetPile(player).Cards.FirstOrDefault()!.Keywords.Contains(OctaviaDangerKeywords.Rigged))
            await CardPileCmd.AutoPlayFromDrawPile(choiceContext, player, 1, CardPilePosition.Top, false);
    }
}