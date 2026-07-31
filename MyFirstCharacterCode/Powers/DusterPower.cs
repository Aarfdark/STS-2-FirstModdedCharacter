using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using MyFirstCharacter.MyFirstCharacterCode.Powers;

namespace MyFirstCharacter.MyFirstCharacterCode.Powers;


public class DusterPower() : MyFirstCharacterPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(0, ValueProp.Unpowered)];

    public void SetBlock(Decimal block)
    {
        DynamicVars.Block.BaseValue = block;
    }
    
    public override async Task AfterBlockCleared(Creature creature)
    {
        if (creature != Owner)
            return;
        Flash();
        await CreatureCmd.GainBlock(Owner, DynamicVars.Block,null);
        await PowerCmd.Decrement(this);
    }
}