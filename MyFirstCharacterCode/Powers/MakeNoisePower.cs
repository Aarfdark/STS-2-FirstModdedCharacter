using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MyFirstCharacter.MyFirstCharacterCode.Cards;
using MyFirstCharacter.MyFirstCharacterCode.Powers;

namespace MyFirstCharacter.MyFirstCharacterCode.Powers;

public class MakeNoisePower() : TemporaryStrengthPower, ICustomModel
{
    public override AbstractModel OriginModel => (AbstractModel) ModelDb.Card<MakeNoise>();
    
    protected override bool IsPositive => false;
}