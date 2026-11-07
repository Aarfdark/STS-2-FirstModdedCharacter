using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using MyFirstCharacter.MyFirstCharacterCode.Cards;
using MyFirstCharacter.MyFirstCharacterCode.Powers;

namespace MyFirstCharacter.MyFirstCharacterCode.Powers;

public class HolyScrapPower() : MyFirstCharacterPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        DynamicVars["StrengthApplied"].IntValue != 0 ? PowerStackType.Counter : PowerStackType.None;

    public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<StrengthPower>(1),
        new DynamicVar("StrengthApplied", 0)
    ];
    
    protected override object InitInternalData() => new Data();
    private class Data
    {
        public readonly Dictionary<CardModel, int> AmountsForPlayedCards = new Dictionary<CardModel, int>();
    }
    
    public override int DisplayAmount => DynamicVars["StrengthApplied"].IntValue;
    
    public override Task BeforeCardPlayed(CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner.Creature != Owner)
            return Task.CompletedTask;
        GetInternalData<Data>().AmountsForPlayedCards.Add(cardPlay.Card, DynamicVars.Strength.IntValue);
        return Task.CompletedTask;
    }
    
    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        int amount;
        if (!(cardPlay.Card is Scrap) || cardPlay.Card.Owner.Creature != Owner || Owner.Player == null || Amount <= 0 || !GetInternalData<Data>().AmountsForPlayedCards.Remove(cardPlay.Card, out amount))
            return;
        Flash();
        await PowerCmd.Apply<StrengthPower>(choiceContext, Owner,
            amount, Owner, null);
        DynamicVars["StrengthApplied"].BaseValue += DynamicVars.Strength.IntValue;
        InvokeDisplayAmountChanged();
    }

    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (!participants.Contains(Owner))
            return;
        await PowerCmd.Apply<StrengthPower>(choiceContext,
            Owner,
            -DynamicVars["StrengthApplied"].BaseValue,
            Owner,
            null, true);
        
        // reset 
        DynamicVars["StrengthApplied"].BaseValue = 0;
        GetInternalData<Data>().AmountsForPlayedCards.Clear();
        InvokeDisplayAmountChanged();
    }
}