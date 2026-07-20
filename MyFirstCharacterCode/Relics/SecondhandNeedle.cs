using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;
using MyFirstCharacter.MyFirstCharacterCode.Powers;
using MyFirstCharacter.MyFirstCharacterCode.Relics;

namespace MyFirstCharacter.MyFirstCharacterCode.Relics;

public class SecondhandNeedle() : MyFirstCharacterRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Uncommon;

    public override async Task AfterDeath(PlayerChoiceContext choiceContext, Creature creature, bool wasRemovalPrevented, float deathAnimLength)
    {
        if (Owner.Creature.CombatState == null || !creature.IsMonster || !creature.HasPower<CharmPower>())
            return;

        var damageToDeal = creature.GetPower<CharmPower>()!.Amount;
        await CreatureCmd.Damage(
            choiceContext,
            Owner.Creature.CombatState.HittableEnemies,
            damageToDeal,
            ValueProp.Unpowered,
            Owner.Creature);
    }
}