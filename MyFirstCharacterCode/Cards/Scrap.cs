using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MyFirstCharacter.MyFirstCharacterCode.Cards;
using MyFirstCharacter.MyFirstCharacterCode.Tags;

namespace MyFirstCharacter.MyFirstCharacterCode.Cards;

public class Scrap() : MyFirstCharacterCard(0, CardType.Skill, CardRarity.Token, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(0)];
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    
    protected override HashSet<CardTag> CanonicalTags => [OctaviaDangerTags.Scrap];
    
    public static async Task<IEnumerable<Scrap>> CreateInHand(
        Player owner,
        int amount,
        ICombatState combatState)
    {
        IEnumerable<Scrap> scraps = Create(owner, amount, combatState);
        IEnumerable<Scrap> cards = scraps.ToList();
        await CardPileCmd.AddGeneratedCardsToCombat(cards, PileType.Hand, owner);
        IEnumerable<Scrap> inHand = cards;
        return inHand;
    }
    
    public static IEnumerable<Scrap> Create(
        Player owner, int amount, ICombatState combatState)
    {
        List<Scrap> scrapList = new List<Scrap>();
        for (int index = 0; index < amount; ++index)
            scrapList.Add(combatState.CreateCard<Scrap>(owner));
        return scrapList;
    }
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (!PileType.Hand.GetPile(Owner).IsEmpty)
        {
            var firstCardInHand = PileType.Draw.GetPile(Owner).Cards.FirstOrDefault();
            await CardCmd.Exhaust(choiceContext, firstCardInHand!);
        }
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1);
    }
}