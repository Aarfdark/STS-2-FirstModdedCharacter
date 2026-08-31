// using MegaCrit.Sts2.Core.Commands;
// using MegaCrit.Sts2.Core.Entities.Cards;
// using MegaCrit.Sts2.Core.GameActions.Multiplayer;
// using MegaCrit.Sts2.Core.Localization.DynamicVars;
// using MyFirstCharacter.MyFirstCharacterCode.Cards;
// using MyFirstCharacter.MyFirstCharacterCode.Powers;
//
// namespace MyFirstCharacter.MyFirstCharacterCode.Cards;
//
// public class Precision() : MyFirstCharacterCard(2,
//     CardType.Skill, CardRarity.Uncommon,
//     TargetType.Self)
// {
//     protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<PrecisionPower>(1)];
//
//     protected override async Task OnPlay(
//         PlayerChoiceContext choiceContext,
//         CardPlay play)
//     {
//         await PowerCmd.Apply<PrecisionPower>(choiceContext, Owner.Creature, DynamicVars["PrecisionPower"].BaseValue,
//             Owner.Creature, this);
//     }
//
//     protected override void OnUpgrade()
//     {
//         EnergyCost.UpgradeBy(-1);
//     }
// }