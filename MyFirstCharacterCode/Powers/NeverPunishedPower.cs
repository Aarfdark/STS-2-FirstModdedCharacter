using System.Reflection;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Monsters;
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

        var allPowers = ModelDb.AllPowers
            .Where(p => p.IsCanonical && !p.IsMock)
            .Select(p => p.ToMutable())
            .ToList();

        Flash();
        // player & allies
        foreach (Creature creature in CombatState.GetTeammatesOf(Owner).Where(c => c.IsAlive && c.IsPlayer))
        {
            var curatedPowers = allPowers
                .Where(p => 
                    p.GetType() != typeof(StockPower)
                    && p.GetType() != typeof(SandpitPower)
                    && p.GetType() != typeof(MinionPower)
                    && p.GetType() != typeof(SteamEruptionPower)
                    && p.GetType() != typeof(BackAttackLeftPower)
                    && p.GetType() != typeof(BackAttackRightPower)
                    && p.GetType() != typeof(AdaptablePower)
                    && p.GetType() != typeof(SurprisePower)
                    && p.GetType() != typeof(BurrowedPower)
                    && p.GetType() != typeof(RampartPower)
                    && p.GetType() != typeof(ReattachPower)
                    && p.GetType() != typeof(AsleepPower)
                    && p.GetType() != typeof(SlumberPower)
                    && p.GetType() != typeof(ImbalancedPower)
                    && p.GetType() != typeof(IllusionPower)
                    && p.GetType() != typeof(PainfulStabsPower)
                    && p.GetType() != typeof(ShriekPower)
                    && p.GetType() != typeof(TankPower)
                    && p.GetType() != typeof(SoulboundPower)
                    && p.GetType() != typeof(ImitationLearningPower)
                    && p.GetType() != typeof(NightmarePower)
                ) 
                .ToList();
            var randInt = Owner.Player.RunState.Rng.Niche.NextInt(curatedPowers.Count);
            var randPowerModel = curatedPowers.ElementAtOrDefault(randInt);
            if (randPowerModel == null)
                return;
            await PowerCmd.Apply(choiceContext, randPowerModel, creature, Amount, Owner, null);
        }
        // enemies
        foreach (Creature creature in CombatState.HittableEnemies)
        {
            var curatedPowers = allPowers
                .Where(p => 
                    !p.Title.LocEntryKey.Contains("form", StringComparison.CurrentCultureIgnoreCase) 
                    // && p.Title.LocEntryKey.Contains("a", StringComparison.CurrentCultureIgnoreCase)
                    && p.GetType() != typeof(RingingPower) 
                    && p.GetType() != typeof(SandpitPower) 
                    && p.GetType() != typeof(StockPower)
                    && p.GetType() != typeof(MinionPower)
                    && p.GetType() != typeof(SteamEruptionPower)
                    && p.GetType() != typeof(BackAttackLeftPower)
                    && p.GetType() != typeof(BackAttackRightPower)
                    && p.GetType() != typeof(AdaptablePower)
                    && p.GetType() != typeof(SurprisePower)
                    && p.GetType() != typeof(BurrowedPower)
                    && p.GetType() != typeof(RampartPower)
                    && p.GetType() != typeof(ReattachPower)
                    && p.GetType() != typeof(AsleepPower)
                    && p.GetType() != typeof(SlumberPower)
                    && p.GetType() != typeof(IllusionPower)
                    && p.GetType() != typeof(LightningRodPower)
                    && p.GetType() != typeof(PhantomBladesPower)
                    && p.GetType() != typeof(AggressionPower)
                    && p.GetType() != typeof(InfiniteBladesPower)
                    && p.GetType() != typeof(ToolsOfTheTradePower)
                    && p.GetType() != typeof(FurnacePower)
                    && p.GetType() != typeof(SpectrumShiftPower)
                    && p.GetType() != typeof(TyrannyPower)
                    && p.GetType() != typeof(VoidFormPower)
                    && p.GetType() != typeof(CountdownPower)
                    && p.GetType() != typeof(DemesnePower)
                    && p.GetType() != typeof(SentryModePower)
                    && p.GetType() != typeof(ForbiddenGrimoirePower)
                    && p.GetType() != typeof(ConsumingShadowPower)
                    && p.GetType() != typeof(CreativeAiPower)
                    && p.GetType() != typeof(MachineLearningPower)
                    && p.GetType() != typeof(SpinnerPower)
                    && p.GetType() != typeof(HelloWorldPower)
                    && p.GetType() != typeof(StratagemPower)
                    && p.GetType() != typeof(EntropyPower)
                    && p.GetType() != typeof(MayhemPower)
                    && p.GetType() != typeof(TankPower)
                    && p.GetType() != typeof(SoulboundPower)
                    && p.GetType() != typeof(ImitationLearningPower)
                    && p.GetType() != typeof(NightmarePower)
                    && p.GetType() != typeof(HexPower)
                )
                .ToList();
            var randInt = Owner.Player.RunState.Rng.Niche.NextInt(curatedPowers.Count);
            var randPowerModel = curatedPowers.ElementAtOrDefault(randInt);
            if (randPowerModel == null)
                return;
            await PowerCmd.Apply(choiceContext, randPowerModel, creature, Amount, Owner, null);
        }
    }
}