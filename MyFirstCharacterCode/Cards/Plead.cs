using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MyFirstCharacter.MyFirstCharacterCode.Cards;
using MyFirstCharacter.MyFirstCharacterCode.Keywords;
using MyFirstCharacter.MyFirstCharacterCode.Powers;

namespace MyFirstCharacter.MyFirstCharacterCode.Cards;

public class Plead() : MyFirstCharacterCard(2,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<CharmPower>(5)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [OctaviaDangerKeywords.Rigged];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (play.Target == null)
            return;
        await PowerCmd.Apply<PleadPower>(choiceContext, play.Target, DynamicVars["CharmPower"].BaseValue,
            Owner.Creature, this);
        
    }

    protected override void OnUpgrade()
    {
        DynamicVars["CharmPower"].UpgradeValueBy(3);
    }
}