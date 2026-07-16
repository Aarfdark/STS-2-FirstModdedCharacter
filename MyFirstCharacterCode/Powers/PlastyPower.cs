using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MyFirstCharacter.MyFirstCharacterCode.Powers;

namespace MyFirstCharacter.MyFirstCharacterCode.Powers;

public class PlastyPower() : MyFirstCharacterPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    public override async Task AfterAutoPrePlayPhaseEntered(PlayerChoiceContext choiceContext, Player player)
    {
        if (player != Owner.Player || PileType.Draw.GetPile(Owner.Player).IsEmpty)
            return;
        CardModel topCard = PileType.Draw.GetPile(Owner.Player).Cards[0];
        await CardCmd.TransformToRandom(topCard, player.RunState.Rng.CombatCardSelection);
        await Cmd.CustomScaledWait(1.5f, 2.5f);
        
        await CardPileCmd.AutoPlayFromDrawPile(choiceContext, Owner.Player, Amount, CardPilePosition.Top, false);
    }
}