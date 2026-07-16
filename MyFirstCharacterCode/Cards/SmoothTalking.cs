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

    private int _increasedCharm;
    [SavedProperty]
    public int IncreasedCharm
    {
        get => _increasedCharm;
        set
        {
            AssertMutable();
            _increasedCharm = value;
        }
    }
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (play.Target == null)
            return;
        await PowerCmd.Apply<CharmPower>(choiceContext, play.Target, DynamicVars["CharmPower"].BaseValue, Owner.Creature, this);
        int scaleAmt = DynamicVars["CharmScale"].IntValue;
        BuffFromPlay(scaleAmt);
        if (!(DeckVersion is SmoothTalking deckVersion))
            return;
        deckVersion.BuffFromPlay(scaleAmt);
    }

    private void BuffFromPlay(int extraCharm)
    {
        IncreasedCharm += extraCharm;
        UpdateCharm();
    }

    private void UpdateCharm()
    {
        CurrentCharm = 1 + IncreasedCharm;
    }
    
    protected override void OnUpgrade()
    {
        DynamicVars["CharmScale"].UpgradeValueBy(1);
    }

    protected override void AfterDowngraded()
    {
        UpdateCharm();
    }
}