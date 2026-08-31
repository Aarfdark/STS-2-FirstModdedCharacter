using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.ValueProps;
using MyFirstCharacter.MyFirstCharacterCode.Cards;
using MyFirstCharacter.MyFirstCharacterCode.Keywords;

namespace MyFirstCharacter.MyFirstCharacterCode.Cards;

public class Reference() : MyFirstCharacterCard(2,
    CardType.Attack, CardRarity.Uncommon,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(17, ValueProp.Move), new DynamicVar("Keywords", 1)];

    private IEnumerable<CardKeyword> potentialKeys = [
        CardKeyword.Exhaust,
        CardKeyword.Ethereal,
        CardKeyword.Innate,
        CardKeyword.Retain,
        CardKeyword.Sly,
        OctaviaDangerKeywords.Rigged,
        OctaviaDangerKeywords.Ashbound,
        OctaviaDangerKeywords.Sempiternal
    ];
    
    public override async Task AfterRoomEntered(AbstractRoom room)
    {
        if (!(room is CombatRoom))
            return;
        Log.Info("HELLO WE ARE IN A COMBAT ROOM RIGHT NOW");
        var tempDuplicate = potentialKeys;
        CardKeyword[] randKeywords = new CardKeyword[DynamicVars["Keywords"].IntValue];
        for (int i = 0; i < DynamicVars["Keywords"].IntValue; i++)
        {
            // adds a random keyword
            int randIndex = Owner.RunState.Rng.Niche.NextInt(tempDuplicate.Count());
            CardKeyword randKeyword = tempDuplicate.ElementAtOrDefault(randIndex);
            tempDuplicate = tempDuplicate.Where(k => k != randKeyword).ToList();
            randKeywords[i] = randKeyword;
            Log.Info(randKeyword.ToString());
        }

        CardCmd.ApplyKeyword(this, randKeywords);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (play.Target == null)
            return;
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this, play)
            .Targeting(play.Target!).WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Keywords"].UpgradeValueBy(1);
    }
}