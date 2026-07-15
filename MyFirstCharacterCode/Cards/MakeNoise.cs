using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MyFirstCharacter.MyFirstCharacterCode.Cards;
using MyFirstCharacter.MyFirstCharacterCode.Powers;

namespace MyFirstCharacter.MyFirstCharacterCode.Cards;

public class MakeNoise() : MyFirstCharacterCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("StrengthLoss", 1), new CardsVar(1)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        foreach (Creature hittableEnemy in CombatState!.HittableEnemies)
        {
            await PowerCmd.Apply<MakeNoisePower>(choiceContext, hittableEnemy, this.DynamicVars["StrengthLoss"].BaseValue, this.Owner.Creature, (CardModel) this);
        }
        CardSelectorPrefs prefs = new CardSelectorPrefs(this.SelectionScreenPrompt, 1);
        CardModel? card = (await CardSelectCmd.FromCombatPile(choiceContext, PileType.Discard.GetPile(this.Owner), this.Owner, prefs)).FirstOrDefault<CardModel>();
        if (card == null)
            return;
        await CardPileCmd.Add(card, PileType.Draw, CardPilePosition.Top);
    }

    protected override void OnUpgrade()
    {
        this.DynamicVars["StrengthLoss"].UpgradeValueBy(1);
    }
}