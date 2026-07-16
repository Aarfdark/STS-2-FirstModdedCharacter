using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MyFirstCharacter.MyFirstCharacterCode.Cards;

namespace MyFirstCharacter.MyFirstCharacterCode.Cards;


public class StructuredChaos() : MyFirstCharacterCard(-1,
    CardType.Skill, CardRarity.Rare,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];
    protected override bool HasEnergyCostX => true;

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        int count = IsUpgraded ? ResolveEnergyXValue()+1 : ResolveEnergyXValue();
        if (PileType.Draw.GetPile(Owner).IsEmpty || count <= 0)
            return;
        var topCardOfDrawPile = PileType.Draw.GetPile(Owner).Cards[0];
        for (int i = 0; i < count; i++)
            await CardCmd.AutoPlay(choiceContext, topCardOfDrawPile, null);
    }
}