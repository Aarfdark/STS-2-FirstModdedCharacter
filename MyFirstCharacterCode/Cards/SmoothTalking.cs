using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Saves.Runs;
using MyFirstCharacter.MyFirstCharacterCode.Cards;
using MyFirstCharacter.MyFirstCharacterCode.Powers;

namespace MyFirstCharacter.MyFirstCharacterCode.Cards;

public class SmoothTalking() : MyFirstCharacterCard(2,
    CardType.Skill, CardRarity.Rare,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<CharmPower>(1), (new DynamicVar("CharmScale", 1))];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    private int _currentCharm = 1;

    [SavedProperty]
    public int CurrentCharm
    {
        get => _currentCharm;
        set
        {
            _currentCharm = value;
            DynamicVars["CharmPower"].BaseValue = _currentCharm;
        }
    }
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (play.Target == null)
            return;
        await PowerCmd.Apply<CharmPower>(choiceContext, play.Target, DynamicVars["CharmPower"].BaseValue, Owner.Creature, this);
        
    }

    protected override void OnUpgrade()
    {
        DynamicVars["CharmScale"].UpgradeValueBy(1);
    }
}