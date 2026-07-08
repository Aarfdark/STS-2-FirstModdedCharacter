using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using MyFirstCharacter.MyFirstCharacterCode.Cards;
using MyFirstCharacter.MyFirstCharacterCode.Keywords;

namespace MyFirstCharacter.MyFirstCharacterCode.Cards;

public class Bite() : MyFirstCharacterCard(3,
    CardType.Attack, CardRarity.Common,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(3, ValueProp.Move), new RepeatVar(3)];
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => [OctaviaDangerKeywords.Ashbound];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).WithHitCount(DynamicVars.Repeat.IntValue).FromCard(this, play).Targeting(play.Target!).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Repeat.UpgradeValueBy(1);
    }
}