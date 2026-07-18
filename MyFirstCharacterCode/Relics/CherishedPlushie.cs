using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MyFirstCharacter.MyFirstCharacterCode.Powers;
using MyFirstCharacter.MyFirstCharacterCode.Relics;

namespace MyFirstCharacter.MyFirstCharacterCode.Relics;

public class CherishedPlushie() : MyFirstCharacterRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Shop;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<CharmPower>(2)];

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var card = cardPlay.Card;
        if (card.Type != CardType.Attack || !card.Keywords.Contains(CardKeyword.Retain))
            return;
        
        if (card.TargetType == TargetType.AnyEnemy)
        {
            if (cardPlay.Target == null)
                return;
            await PowerCmd.Apply<CharmPower>(choiceContext, cardPlay.Target, DynamicVars["CharmPower"].BaseValue,
                Owner.Creature, null);
        }
        else if (card.TargetType == TargetType.AllEnemies)
        {
            if (cardPlay.Player.Creature.CombatState == null)
                return;
            foreach (Creature hittableEnemy in cardPlay.Player.Creature.CombatState.HittableEnemies)
                await PowerCmd.Apply<CharmPower>(choiceContext, hittableEnemy, DynamicVars["CharmPower"].BaseValue, Owner.Creature, null);
        }
    }
}