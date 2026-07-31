using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MyFirstCharacter.MyFirstCharacterCode.Cards;

namespace MyFirstCharacter.MyFirstCharacterCode.Events;

public class TheCloset() : CustomEventModel()
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new GoldVar(200), 
        new StringVar("ComeOutCurse", ModelDb.Card<Regret>().Title),
        new StringVar("StayInCurse", ModelDb.Card<Sorrow>().Title)
    ];
    public override string CustomInitialPortraitPath => ImageHelper.GetImagePath($"events/{ModelDb.Event<ThisOrThat>().Id.Entry.ToLowerInvariant()}.png");
    public override string CustomBackgroundScenePath => SceneHelper.GetScenePath("events/background_scenes/" + ModelDb.Event<ThisOrThat>().Id.Entry.ToLowerInvariant());
    protected override IReadOnlyList<EventOption> GenerateInitialOptions() =>
    [
        Option(StayIn, HoverTipFactory.FromCardWithCardHoverTips<Regret>()),
        Option(ComeOut, HoverTipFactory.FromCardWithCardHoverTips<Sorrow>())
    ];

    private async Task ComeOut()
    {
        CardModel? original = (await CardSelectCmd.FromDeckForTransformation(Owner!, new CardSelectorPrefs(CardSelectorPrefs.TransformSelectionPrompt, 1))).FirstOrDefault();
        if (original != null)
        {
            CardModel transformed = (await CardCmd.TransformToRandom(original, Rng, CardPreviewStyle.EventLayout)).cardAdded;
            CardCmd.Upgrade(transformed);
        }
        await CardPileCmd.AddCurseToDeck<Regret>(Owner!);
        SetEventFinished(L10NLookup("THE_CLOSET.pages.COME_OUT.description"));
    }

    private async Task StayIn()
    {
        await PlayerCmd.GainGold(DynamicVars.Gold.IntValue, Owner!);
        await CardPileCmd.AddCurseToDeck<Sorrow>(Owner!);
        SetEventFinished(L10NLookup("THE_CLOSET.pages.STAY_IN.description"));
    }
}