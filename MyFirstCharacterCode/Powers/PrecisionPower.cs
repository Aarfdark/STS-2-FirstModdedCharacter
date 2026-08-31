// using MegaCrit.Sts2.Core.Combat;
// using MegaCrit.Sts2.Core.Commands;
// using MegaCrit.Sts2.Core.Entities.Cards;
// using MegaCrit.Sts2.Core.Entities.Creatures;
// using MegaCrit.Sts2.Core.Entities.Powers;
// using MegaCrit.Sts2.Core.GameActions.Multiplayer;
// using MegaCrit.Sts2.Core.ValueProps;
// using MyFirstCharacter.MyFirstCharacterCode.Powers;
//
// namespace MyFirstCharacter.MyFirstCharacterCode.Powers;
//
// public class PrecisionPower() : MyFirstCharacterPower
// {
//     public override PowerType Type =>
//         PowerType.Buff;
//
//     public override PowerStackType StackType =>
//         PowerStackType.Counter;
//
//     public override async Task BeforeCardPlayed(CardPlay cardPlay)
//     {
//         if (cardPlay.Card.Owner.Creature != Owner || cardPlay.Card.Type != CardType.Attack)
//             return;
//         var cardDamage = cardPlay.Card.DynamicVars.Damage.BaseValue;
//         cardPlay.Card.DynamicVars.Damage.BaseValue = 0;
//         if (cardPlay.Target == null)
//             return;
//         // await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(cardPlay.Card, cardPlay)
//         //     .Targeting(cardPlay.Target!).WithHitFx("vfx/vfx_attack_slash")
//         //     .Execute(cardPlay);
//     }
//
//     public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
//     {
//         if (!participants.Contains(Owner))
//             return;
//         await PowerCmd.Remove(this);
//     }
// }