using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using MyFirstCharacter.MyFirstCharacterCode.Hooks;
using MyFirstCharacter.MyFirstCharacterCode.Keywords;
using MyFirstCharacter.MyFirstCharacterCode.Relics;

namespace MyFirstCharacter.MyFirstCharacterCode.Relics;

public class EffigyOfPhoebe() : MyFirstCharacterRelic, IOnCardPlayedViaAshbound
{
    public override RelicRarity Rarity =>
        RelicRarity.Common;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1)];
    
    public async Task OnCardPlayedViaAshbound(ICombatState combatState, PlayerChoiceContext choiceContext, CardModel card)
    {
        if (card.Keywords.Contains(OctaviaDangerKeywords.Ashbound))
            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, null);
    }
}