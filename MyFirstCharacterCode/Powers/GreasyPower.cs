using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using MyFirstCharacter.MyFirstCharacterCode.Powers;

namespace MyFirstCharacter.MyFirstCharacterCode.Powers;

public class GreasyPower() : MyFirstCharacterPower
{
    public override PowerType Type =>
        PowerType.Debuff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        Flash();
        var numKeywords = cardPlay.Card.Keywords.Count;
        if (numKeywords == 0 || cardPlay.Card.Owner != Applier!.Player)
            return;
        for (int i = 0; i < numKeywords; i++)
            await CreatureCmd.Damage(choiceContext, Owner, Amount,ValueProp.Unblockable | ValueProp.Unpowered, null, cardPlay);
        
    }
}