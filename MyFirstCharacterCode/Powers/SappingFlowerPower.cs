using MegaCrit.Sts2.Core.Entities.Powers;
using MyFirstCharacter.MyFirstCharacterCode.Powers;

namespace MyFirstCharacter.MyFirstCharacterCode.Powers;

public class SappingFlowerPower() : MyFirstCharacterPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    
}