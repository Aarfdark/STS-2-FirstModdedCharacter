using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using MyFirstCharacter.MyFirstCharacterCode.Powers;

namespace MyFirstCharacter.MyFirstCharacterCode.Powers;

public class BreakItDownPower() : MyFirstCharacterPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        int energySpent = cardPlay.Card.EnergyCost.GetResolved();
        var originalEnergy = ModelDb.AllCards.First(
            c => c.Id == cardPlay.Card.Id
            ).EnergyCost.GetResolved();
        
        Log.Info("SPENT: " + energySpent);
        Log.Info("ORIGI: " + originalEnergy);
        
        if (energySpent != originalEnergy)
        {
            await CreatureCmd.GainBlock(Owner, Amount, ValueProp.Move, cardPlay);
        }
    }
}