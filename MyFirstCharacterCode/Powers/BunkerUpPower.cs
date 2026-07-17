using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MyFirstCharacter.MyFirstCharacterCode.Powers;

namespace MyFirstCharacter.MyFirstCharacterCode.Powers;

public class BunkerUpPower() : MyFirstCharacterPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    public override CardLocation ModifyCardPlayResultLocation(CardModel card, bool isAutoPlay, ResourceInfo resources,
        CardLocation cardLocation)
    {
        if (card.Owner.Creature != Owner || cardLocation.pileType != PileType.Discard)
            return cardLocation;
        cardLocation.pileType = PileType.Draw;
        cardLocation.position = CardPilePosition.Top;
        return cardLocation;
    }
    
    public override async Task AfterModifyingCardPlayResultLocation(
        CardModel card,
        CardLocation location)
    {
        if (card.Owner.Creature != Owner)
            return;
        Flash();
        await PowerCmd.Decrement(this);
    }

    public override async Task AfterSideTurnEnd(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IEnumerable<Creature> participants)
    {
        if (!participants.Contains(Owner))
            return;
        await PowerCmd.Remove(this);
    }
}