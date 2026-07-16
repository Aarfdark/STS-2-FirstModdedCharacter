using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using MyFirstCharacter.MyFirstCharacterCode.Cards;

namespace MyFirstCharacter.MyFirstCharacterCode.Cards;

public class RepositioningStrike() : MyFirstCharacterCard(1,
    CardType.Attack, CardRarity.Uncommon,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(11, ValueProp.Move), new EnergyVar(1)];
    protected override HashSet<CardTag> CanonicalTags => [CardTag.Strike];
    
    public override async Task AfterCardChangedPiles(CardModel card, PileType oldPileType, AbstractModel? clonedBy)
    {
        if (Pile == null)
            return;
        
        if (Pile.Type != PileType.Hand)
            return;
        
        // reset energy to default before adding based on card position
        var curEnergyCost = EnergyCost.GetResolved();
        EnergyCost.AddThisCombat(-curEnergyCost+DynamicVars.Energy.IntValue);
        
        var count = 0;
        foreach (CardModel cardInHand in CardPile.GetCards(Owner, PileType.Hand))
        {
            if (cardInHand == this)
                EnergyCost.AddThisCombat(count);
            count++;
        }
    }
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (play.Target == null)
            return;
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this, play).Targeting(play.Target).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Energy.UpgradeValueBy(-1);
    }
}