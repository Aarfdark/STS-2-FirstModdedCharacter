using MegaCrit.Sts2.Core.Audio;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;

namespace MyFirstCharacter.MyFirstCharacterCode.Monsters;

public class Miller : MyFirstCharacterMonster
{
    // HP values
    public override int MinInitialHp => AscensionHelper.GetValueIfAscension(
        AscensionLevel.ToughEnemies, 18, 16);
    public override int MaxInitialHp => AscensionHelper.GetValueIfAscension(
        AscensionLevel.ToughEnemies, 24, 22);
    
    // Move data
    private int SlapDamage => AscensionHelper.GetValueIfAscension(
        AscensionLevel.DeadlyEnemies, 8, 6);

    private int StrengthAmount => 2;
    
    public override DamageSfxType TakeDamageSfxType => DamageSfxType.Stone;

    // Move Definitions
    private async Task SlapMove(IReadOnlyList<Creature> targets)
    {
        await DamageCmd
            .Attack(SlapDamage)
            .FromMonster(this)
            .WithAttackerAnim("AttackSingle", 0.15f)
            .WithAttackerFx(sfx: AttackSfx)
            .WithHitFx("vfx/vfx_attack_blunt", AttackSfx)
            .Execute(null);
    }

    private async Task StrengthMove(IReadOnlyList<Creature> targets)
    {
        await CreatureCmd.TriggerAnim(Creature, "Cast", 0.2f);
        await PowerCmd.Apply<StrengthPower>(
            new ThrowingPlayerChoiceContext(), Creature,
            StrengthAmount, Creature,null);
    }
    
    // State Machine
    protected override MonsterMoveStateMachine
        GenerateMoveStateMachine()
    {
        List<MonsterState> states = new List<MonsterState>();
        MoveState move1 = new MoveState(
            "SLAP_MOVE", SlapMove, new SingleAttackIntent(SlapDamage));
        MoveState move2 = new MoveState(
            "STRENGTH_MOVE", StrengthMove, new BuffIntent());
        
        ConditionalBranchState initialState = new ConditionalBranchState("INIT_MOVE");
        move1.FollowUpState = move2;
        move2.FollowUpState = move1;
        // initialState.AddState(move1, (Func<bool>) (() => !Creature.Monster.IsFront));
        // initialState.AddState(move2, (Func<bool>) (() => Creature.Monster.IsFront));
        states.Add(initialState);
        states.Add(move2);
        states.Add(move1);
        return new MonsterMoveStateMachine(states, initialState);
    }
}