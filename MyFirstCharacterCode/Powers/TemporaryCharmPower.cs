using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Potions;
using MegaCrit.Sts2.Core.Models.Powers;
using MyFirstCharacter.MyFirstCharacterCode.Powers;

namespace MyFirstCharacter.MyFirstCharacterCode.Powers;

public abstract class TemporaryCharmPower() : PowerModel, ITemporaryPower
{
    public override PowerType Type =>
        PowerType.Debuff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;
    
    public abstract AbstractModel OriginModel { get; }

    public PowerModel InternallyAppliedPower => ModelDb.Power<CharmPower>();

    protected virtual bool IsPositive => true;
    private int Sign => !this.IsPositive ? -1 : 1;
    
    public override async Task BeforeApplied(
        Creature target, 
        decimal amount, 
        Creature? applier, 
        CardModel? cardSource)
    {
        await PowerCmd.Apply<CharmPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), target, Sign * amount, applier, cardSource, true);
    }

    public override async Task AfterPowerAmountChanged(
        PlayerChoiceContext choiceContext,
        PowerModel power, 
        decimal amount, 
        Creature? applier,
        CardModel? cardSource)
    {
        if (amount == Amount || power != this)
            return;
        await PowerCmd.Apply<CharmPower>(choiceContext, Owner, Sign * amount, applier, cardSource, true);
    }

    public override async Task AfterSideTurnEnd(
        PlayerChoiceContext choiceContext, 
        CombatSide side, 
        IEnumerable<Creature> participants)
    {
        if (!participants.Contains(Owner))
            return;
        Flash();
        await PowerCmd.Remove(this);
        await PowerCmd.Apply<CharmPower>(choiceContext, Owner, -Sign * Amount, Owner, null);
    }
}