using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MyFirstCharacter.MyFirstCharacterCode.Powers;

namespace MyFirstCharacter.MyFirstCharacterCode.Powers;

public class InstabilityPower() : MyFirstCharacterPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        foreach (CardModel card in await CardSelectCmd.FromHand(choiceContext, player,
                     new CardSelectorPrefs(this.SelectionScreenPrompt, this.Amount),
                     null, (AbstractModel)this))
        {
            // EnchantmentModel[] randomAttackEnchants = {};
            // EnchantmentModel[] randomDefenseEnchants = {};
            // EnchantmentModel[] randomPowerEnchants = {};
            // find out what type of card it is then pick a random enchantment based on that
            if (card.Type == CardType.Attack)
                CardCmd.Enchant<Corrupted>(card, 1);
            else if (card.Type == CardType.Skill)
                CardCmd.Enchant<Adroit>(card, 1);
            else if (card.Type == CardType.Power)
                CardCmd.Enchant<Glam>(card, 1);
            else
                CardCmd.Enchant<Steady>(card, 1);
        }
    }
}