// using BaseLib.Extensions;
// using MegaCrit.Sts2.Core.Models;
// using MegaCrit.Sts2.Core.Rooms;
// using MyFirstCharacter.MyFirstCharacterCode.Monsters;
//
// namespace MyFirstCharacter.MyFirstCharacterCode.Encounters;
//

// TODO: everything

// public class TheMillers : MyFirstCharacterEncounter
// {
//     public TheMillers(RoomType roomType, bool autoAdd = true) : base(roomType, autoAdd)
//     {
//         
//     }
//
//     public override bool IsValidForAct(ActModel act)
//     {
//         if (act.ActNumber() == 1)
//             return true;
//         return false;
//     }
//
//     public override RoomType RoomType => RoomType.Monster;
//
//     public override IEnumerable<MonsterModel> AllPossibleMonsters =>
//         [ModelDb.Monster<Miller>()];
//
//     protected override IReadOnlyList<(MonsterModel, string?)> GenerateMonsters()
//     {
//         return [(ModelDb.Monster<Miller>().ToMutable(), null)];
//     }
// }