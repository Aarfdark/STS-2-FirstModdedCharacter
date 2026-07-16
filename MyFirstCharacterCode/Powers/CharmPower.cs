using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using MyFirstCharacter.MyFirstCharacterCode.Powers;

namespace MyFirstCharacter.MyFirstCharacterCode.Powers;

public class CharmPower() : MyFirstCharacterPower
{
    public override PowerType Type =>
        PowerType.Debuff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    public override async Task AfterDamageGiven(PlayerChoiceContext choiceContext,
        Creature? dealer, DamageResult result, ValueProp props,
        Creature target, CardModel? cardSource)
    {
        if (target != Applier || dealer == null || !dealer.HasPower<CharmPower>())
            return;
        Flash();
        await CreatureCmd.Damage(choiceContext, dealer!, Amount, ValueProp.Unblockable | ValueProp.SkipHurtAnim, null, null);
    }
    
    
}