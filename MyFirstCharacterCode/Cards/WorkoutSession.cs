using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MyFirstCharacter.MyFirstCharacterCode.Cards;

namespace MyFirstCharacter.MyFirstCharacterCode.Cards;


public class WorkoutSession() : MyFirstCharacterCard(3,
    CardType.Skill, CardRarity.Token,
    TargetType.AllAllies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        if (CombatState == null)
            return;

        List<Task<IEnumerable<CardModel>>> tasks = [];
        foreach (Creature teammate in CombatState.GetTeammatesOf(Owner.Creature).Where(c => c.IsAlive && c.IsPlayer))
        {
            CardSelectorPrefs prefs = new CardSelectorPrefs(CardSelectorPrefs.ExhaustSelectionPrompt, 1);
            tasks.Add(
                CardSelectCmd.FromHand(choiceContext, teammate.Player!, prefs, null, this)
            );
            // CardModel? card = (await CardSelectCmd.FromHand(choiceContext, teammate.Player!, prefs, null, this)).FirstOrDefault();
            //     if (card == null)
            //         return;
            //     card.ExhaustOnNextPlay = true;
            //     await CardCmd.AutoPlay(choiceContext, card, null);
        }
        
        var results = await Task.WhenAll(tasks.ToArray());
        foreach (var cardList in results)
        {
            CardModel? card = cardList.FirstOrDefault();
            if (card == null)
                return;
            card.ExhaustOnNextPlay = true;
            await CardCmd.AutoPlay(choiceContext, card, null);
        }
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}