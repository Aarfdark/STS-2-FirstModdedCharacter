using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;
using MyFirstCharacter.MyFirstCharacterCode.Cards;

namespace MyFirstCharacter.MyFirstCharacterCode.Cards;

public class UnstableSculpture() : MyFirstCharacterCard(2,
    CardType.Power, CardRarity.Rare,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        // RelicCmd.Obtain(RelicFactory.PullNextRelicFromFront(this.Owner).ToMutable(), this.Owner);
        RelicModel mutable = RelicFactory.PullNextRelicFromFront(this.Owner).ToMutable();
        mutable.IsWax = true;
        await RelicCmd.Obtain(mutable, Owner);
    }

    public override async Task AfterCombatEnd(CombatRoom room)
    {
        RelicModel? relic = Owner.Relics.FirstOrDefault(r => r.IsWax && !r.IsMelted);
        if (relic == null)
            return;
        await RelicCmd.Melt(relic);
        await Cmd.CustomScaledWait(0.5f, 0.75f);
    }

    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Innate);
    }
}