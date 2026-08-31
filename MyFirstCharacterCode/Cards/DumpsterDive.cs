using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MyFirstCharacter.MyFirstCharacterCode.Cards;

namespace MyFirstCharacter.MyFirstCharacterCode.Cards;

public class DumpsterDive() : MyFirstCharacterCard(1,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.AllAllies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (CombatState == null)
            return;
        foreach (Creature teammate in CombatState.GetTeammatesOf(Owner.Creature).Where(c => c.IsAlive && c.IsPlayer))
        {
            for (int i = 0; i < DynamicVars.Cards.IntValue; ++i)
            {
                var scrap = await Scrap.CreateInHand(teammate.Player!, DynamicVars["Scraps"].IntValue, CombatState);
                if (IsUpgraded)
                    foreach (var card in scrap)
                        CardCmd.Upgrade(card);
            }
        }
    }

    protected override void OnUpgrade()
    {

    }
}