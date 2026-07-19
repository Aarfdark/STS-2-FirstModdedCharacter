using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.ValueProps;
using MyFirstCharacter.MyFirstCharacterCode.Cards;
using MyFirstCharacter.MyFirstCharacterCode.Character;
using MyFirstCharacter.MyFirstCharacterCode.Relics;

namespace MyFirstCharacter.MyFirstCharacterCode.Relics;

[Pool(typeof(SharedRelicPool))]
public class NeowsBounty() : MyFirstCharacterRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Ancient;

    protected override IEnumerable<DynamicVar> CanonicalVars => [];
    
    private static IEnumerable<RelicModel> GetValidRelics(Player owner)
    {
        return ModelDb.Event<Neow>().AllPossibleOptions.Where( o => o.Relic != null && o.Relic.IsAllowedAtNeow(owner)).Select((Func<EventOption, RelicModel>) (o => o.Relic!));
    }
    

    public async override Task AfterObtained()
    {
        List<CardModel> starterCards = new List<CardModel>();
        foreach (CardModel card in PileType.Deck.GetPile(Owner).Cards)
        {
            if (card.Rarity == CardRarity.Basic)
                starterCards.Add(card);
        }
        foreach (CardModel card in starterCards)
            await CardPileCmd.RemoveFromDeck(card);
        
        // grab every Neow relic
        List<RelicModel> validRelics = GetValidRelics(Owner).ToList();
        Owner.PlayerRng.Rewards.Shuffle(validRelics);

        foreach (var relic in validRelics)
            await RelicCmd.Obtain(relic, Owner);
        
        await CardPileCmd.AddCurseToDeck<Sorrow>(Owner);
    }
}