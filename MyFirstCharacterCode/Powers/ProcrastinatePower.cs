using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MyFirstCharacter.MyFirstCharacterCode.Powers;

namespace MyFirstCharacter.MyFirstCharacterCode.Powers;

public class ProcrastinatePower() : MyFirstCharacterPower
{
    private const string _cardKey = "Card";
    
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;
    
    public override PowerStackType StackType =>
        PowerStackType.Counter;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new StringVar(_cardKey)];

    protected override object InitInternalData() => new Data();
    private class Data
    {
        public CardModel? SelectedCard;
    }

    public override async Task AfterAutoPrePlayPhaseEntered(PlayerChoiceContext choiceContext, Player player)
    {
        CardModel card;
        if (player != Owner.Player)
            return;
        card = GetInternalData<Data>().SelectedCard!;
        for (int i = 0; i < 2; i++)
            await CardCmd.AutoPlay(choiceContext, card.CreateDupe(player), null);
        await PowerCmd.Remove(this);
    }

    public void SetSelectedCard(CardModel card)
    {
        Log.Info(String.Format("THE SELECTED CARD WAS: " + card.Title));
        CardModel clone = card.CreateClone();
        CardCmd.ClearAffliction(clone);
        GetInternalData<Data>().SelectedCard = clone;
        ((StringVar) DynamicVars["Card"]).StringValue = clone.Title;
    }
}