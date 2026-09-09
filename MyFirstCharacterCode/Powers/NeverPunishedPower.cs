using System.Reflection;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;
using MyFirstCharacter.MyFirstCharacterCode.Powers;

namespace MyFirstCharacter.MyFirstCharacterCode.Powers;

public class NeverPunishedPower() : MyFirstCharacterPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;
    
    public async override Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (Owner.Player == null)
            return;

        var allPowers = ModelDb.AllPowers.Where(p => p.IsCanonical).ToList();

        Flash();
        // player & allies
        foreach (Creature creature in CombatState.GetTeammatesOf(Owner).Where(c => c.IsAlive && c.IsPlayer))
        {
            var randInt = Owner.Player.RunState.Rng.Niche.NextInt(allPowers.Count);
            var randPowerModel = allPowers.ElementAtOrDefault(randInt);
            if (randPowerModel == null)
                return;
            var randPower = randPowerModel.ToMutable();
            await PowerCmd.Apply(choiceContext, randPower, creature, Amount, Owner, null);
        }
        // enemies
        foreach (Creature creature in CombatState.HittableEnemies)
        {
            var randInt = Owner.Player.RunState.Rng.Niche.NextInt(allPowers.Count);
            var randPowerModel = allPowers.ElementAtOrDefault(randInt);
            if (randPowerModel == null)
                return;
            var randPower = randPowerModel.ToMutable();
            await PowerCmd.Apply(choiceContext, randPower, creature, Amount, Owner, null);
        }
    }
}