using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MyFirstCharacter.MyFirstCharacterCode.Character;
using MyFirstCharacter.MyFirstCharacterCode.Extensions;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace MyFirstCharacter.MyFirstCharacterCode.Cards;


[Pool(typeof(MyFirstCharacterCardPool))]
public abstract class Whatever(int cost, CardType type, CardRarity rarity, TargetType target) : CustomCardModel(cost, type, rarity, target)
{

    public override string CustomPortraitPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigCardImagePath();
    public override string PortraitPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("CardsVar", 1)];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // Whatever thisCard = this;
        int cardCount = this.DynamicVars.Cards.IntValue;
        IEnumerable<CardModel> cardDraw =
            await CardPileCmd.Draw(choiceContext, this.DynamicVars.Cards.BaseValue, this.Owner);
    }
}