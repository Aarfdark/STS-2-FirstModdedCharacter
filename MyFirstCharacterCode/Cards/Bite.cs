using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using MyFirstCharacter.MyFirstCharacterCode.Cards;
using MyFirstCharacter.MyFirstCharacterCode.Hooks;
using MyFirstCharacter.MyFirstCharacterCode.Keywords;

namespace MyFirstCharacter.MyFirstCharacterCode.Cards;

public class Bite() : MyFirstCharacterCard(2,
    CardType.Attack, CardRarity.Common,
    TargetType.AnyEnemy), IOnCardPlayedViaAshbound
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(4, ValueProp.Move), new RepeatVar(3)];
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => [OctaviaDangerKeywords.Ashbound];
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (play.Target == null)
            return;
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).WithHitCount(DynamicVars.Repeat.IntValue).FromCard(this, play).Targeting(play.Target).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Repeat.UpgradeValueBy(1);
    }
    
    public async Task OnCardPlayedViaAshbound(ICombatState combatState, 
        PlayerChoiceContext choiceContext, CardModel card)
    {
        if (card != this)
            return;
        await CardCmd.AutoPlay(choiceContext, card.CreateDupe(Owner), null);
        // await DamageCmd.Attack(DynamicVars.Damage.BaseValue).WithHitCount(DynamicVars.Repeat.IntValue).FromCard(this, null).TargetingRandomOpponents(combatState).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
        Log.Info("BITE WAS PLAYED WITH ASHBOUND YIPPEEEEEEE");
    }
}