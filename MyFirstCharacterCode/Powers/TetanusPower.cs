using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using MyFirstCharacter.MyFirstCharacterCode.Cards;
using MyFirstCharacter.MyFirstCharacterCode.Powers;

namespace MyFirstCharacter.MyFirstCharacterCode.Powers;

public class TetanusPower() : MyFirstCharacterPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    public async override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (!(cardPlay.Card is Scrap) || cardPlay.Card.Owner.Creature != Owner || Owner.Player == null)
            return;
        
        Creature? target = Owner.Player!.RunState.Rng.CombatTargets.NextItem(CombatState.HittableEnemies);
        if (target == null)
            return;
        await PowerCmd.Apply<DemisePower>(choiceContext, target, Amount, cardPlay.Card.Owner.Creature, cardPlay.Card);
    }
}