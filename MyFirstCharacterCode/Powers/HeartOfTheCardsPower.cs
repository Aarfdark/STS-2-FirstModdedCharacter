using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using MyFirstCharacter.MyFirstCharacterCode.Powers;

namespace MyFirstCharacter.MyFirstCharacterCode.Powers;

public class HeartOfTheCardsPower() : MyFirstCharacterPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (Owner.Player == null)
            return;
        int energySpent = cardPlay.Card.EnergyCost.GetResolved();
        var originalEnergy = ModelDb.AllCards.First(
                c => c.Id == cardPlay.Card.Id
            ).EnergyCost.GetResolved();
            
        if (energySpent != originalEnergy)
        {
            await PlayerCmd.GainEnergy(Amount, Owner.Player);
            await CreatureCmd.GainBlock(Owner, Amount, ValueProp.Move, cardPlay);
        }
    }
}