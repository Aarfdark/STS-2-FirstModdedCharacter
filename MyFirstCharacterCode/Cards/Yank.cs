using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MyFirstCharacter.MyFirstCharacterCode.Cards;

namespace MyFirstCharacter.MyFirstCharacterCode.Cards;

public class Yank() : MyFirstCharacterCard(1,
    CardType.Skill, CardRarity.Common,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2), new DynamicVar("Scraps", 1)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (CombatState == null)
            return;
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
        var scrap = await Scrap.CreateInHand(Owner, DynamicVars["Scraps"].IntValue, CombatState);
        if (IsUpgraded)
            foreach (var card in scrap)
                CardCmd.Upgrade(card);
    }
}