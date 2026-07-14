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

    public required CardModel TopCardOfDrawPile;
    
    public override int ModifyCardPlayCount(CardModel card, Creature? target, int playCount)
    {
        int count = IsUpgraded ? ResolveEnergyXValue()+1 : ResolveEnergyXValue();
        return (card.Owner.Creature != Owner.Creature || card != TopCardOfDrawPile) ? playCount : count;
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (PileType.Draw.GetPile(Owner).IsEmpty)
            return;
        TopCardOfDrawPile = PileType.Draw.GetPile(Owner).Cards[0];
        await CardPileCmd.AutoPlayFromDrawPile(choiceContext, Owner, 1, CardPilePosition.Top, false);
    }
}