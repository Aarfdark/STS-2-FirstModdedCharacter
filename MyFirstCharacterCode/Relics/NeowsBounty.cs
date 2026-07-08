using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.ValueProps;
using MyFirstCharacter.MyFirstCharacterCode.Relics;

namespace MyFirstCharacter.MyFirstCharacterCode.Relics;

public class NeowsBounty() : MyFirstCharacterRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Ancient;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("Relics", 28)];
    
    private static IEnumerable<RelicModel> GetValidRelics(Player owner)
    {
        return ModelDb.Event<Neow>().AllPossibleOptions.Where( (o => o.Relic != null && o.Relic.IsAllowedAtNeow(owner))).Select((Func<EventOption, RelicModel>) (o => o.Relic!)).OfType<RelicModel>();
    }
    

    public async override Task AfterObtained()
    {
        // grab every Neow relic
        List<RelicModel> validRelics = GetValidRelics(Owner).ToList();
        Owner.PlayerRng.Rewards.Shuffle(validRelics);
        // make them all into Rewards objects
        List<Reward> rewards = validRelics.Take(DynamicVars["Relics"].IntValue)
            .Select((Func<RelicModel, Reward>)((r) => new RelicReward(r, Owner)))
            .ToList<Reward>();
        // give them to player
        await new RewardsSet(Owner).WithCustomRewards(rewards).WithSkippingDisallowed().Offer();

        // get every card in Deck
        foreach (CardModel card in PileType.Deck.GetPile(Owner).Cards)
        {
            if (card.Rarity == CardRarity.Basic)
                _ = CardPileCmd.RemoveFromDeck(card);
        }
    }
}