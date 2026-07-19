using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MyFirstCharacter.MyFirstCharacterCode.Character;
using MyFirstCharacter.MyFirstCharacterCode.Keywords;
using MyFirstCharacter.MyFirstCharacterCode.Relics;

namespace MyFirstCharacter.MyFirstCharacterCode.Relics;

public class WeightedDice() : MyFirstCharacterRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Rare;

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        List<CardModel> hand = PileType.Hand.GetPile(Owner).Cards.ToList();
        if (hand.Count == 0)
            return;
        List<CardModel> list2 = hand.Where((Func<CardModel, bool>) (c =>
        {
            bool flag1 = !c.Keywords.Contains(CardKeyword.Unplayable);
            if (flag1)
            {
                bool flag2;
                switch (c.Type)
                {
                    case CardType.Curse:
                    case CardType.Quest:
                        flag2 = true;
                        break;
                    default:
                        flag2 = false;
                        break;
                }
                flag1 = !flag2;
            }
            return flag1 && !c.Keywords.Contains(OctaviaDangerKeywords.Rigged);
        })).ToList();
        
        List<CardModel> list3 = list2.Where((Func<CardModel, bool>) (c =>
        {
            bool flag;
            switch (c.Type)
            {
                case CardType.Attack:
                case CardType.Skill:
                case CardType.Power:
                    flag = true;
                    break;
                default:
                    flag = false;
                    break;
            }
            return flag;
        })).ToList();
        
        IEnumerable<CardModel> items = list3.Count == 0 ? list2 : (IEnumerable<CardModel>) list3;
        CardModel? card = Owner.RunState.Rng.CombatCardSelection.NextItem(items);
        if (card == null)
            return;

        Flash();
        card.AddKeyword(OctaviaDangerKeywords.Rigged);
        CardCmd.Preview(card);
    }
}