using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using MyFirstCharacter.MyFirstCharacterCode.Cards;
using MyFirstCharacter.MyFirstCharacterCode.Keywords;
using MyFirstCharacter.MyFirstCharacterCode.Powers;

namespace MyFirstCharacter.MyFirstCharacterCode.Cards;

public class Tlc() : MyFirstCharacterCard(1,
    CardType.Skill, CardRarity.Rare,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<CharmPower>(6), new HealVar(15)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [OctaviaDangerKeywords.Ashbound];
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (play.Target == null)
            return;
        await PowerCmd.Apply<CharmPower>(choiceContext, play.Target,
            DynamicVars["CharmPower"].BaseValue, Owner.Creature, this);
        await CreatureCmd.Heal(play.Target, DynamicVars.Heal.BaseValue);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Heal.UpgradeValueBy(-5);
    }
}