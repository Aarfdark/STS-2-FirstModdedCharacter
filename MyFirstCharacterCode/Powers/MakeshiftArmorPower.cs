using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;
using MyFirstCharacter.MyFirstCharacterCode.Cards;
using MyFirstCharacter.MyFirstCharacterCode.Powers;
using MyFirstCharacter.MyFirstCharacterCode.Tags;

namespace MyFirstCharacter.MyFirstCharacterCode.Powers;

public class MakeshiftArmorPower() : MyFirstCharacterPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (!(cardPlay.Card is Scrap) || cardPlay.Card.Owner.Creature != Owner || Owner.Player == null || Amount <= 0)
            return;
        await CreatureCmd.GainBlock(Owner, Amount, ValueProp.Unpowered, null, true);
    }
}