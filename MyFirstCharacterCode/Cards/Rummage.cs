using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using MyFirstCharacter.MyFirstCharacterCode.Cards;

namespace MyFirstCharacter.MyFirstCharacterCode.Cards;


public class Rummage() : MyFirstCharacterCard(2,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(8, ValueProp.Move)];
    public override bool GainsBlock => true;

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (CombatState == null)
            return;
        
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block.BaseValue, ValueProp.Move, play);
        List<Scrap>? scraps = Scrap.Create(Owner, 3, CombatState).ToList<Scrap>();
        CardPileAddResult drawResult = await CardPileCmd.AddGeneratedCardToCombat(scraps[0], PileType.Draw, Owner, CardPilePosition.Random);
        CardPileAddResult discardResult = await CardPileCmd.AddGeneratedCardToCombat(scraps[1], PileType.Discard, Owner);
        CardPileAddResult combat = await CardPileCmd.AddGeneratedCardToCombat( scraps[2], PileType.Hand, Owner);
        
        CardCmd.PreviewCardPileAdd([
            drawResult,
            discardResult
        ]);
        scraps = null;
        drawResult = new CardPileAddResult();
        discardResult = new CardPileAddResult();
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(4);
    }
}