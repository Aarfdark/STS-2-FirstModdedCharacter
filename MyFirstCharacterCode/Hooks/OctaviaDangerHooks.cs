using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace MyFirstCharacter.MyFirstCharacterCode.Hooks;

public static class OctaviaDangerHooks
{
    // gets registered Hooks and executes them
    private static async Task Dispatch<T>(
        ICombatState combatState,
        PlayerChoiceContext choiceContext,
        Func<T, Task> action)
        where T : class
    {
        foreach (T listener in combatState.IterateHookListeners().OfType<T>())
        {
            AbstractModel model = (AbstractModel)(object)listener;

            choiceContext.PushModel(model);
            await action(listener);
            choiceContext.PopModel(model);
        }
    }

    public static Task OnCardPlayedViaAshbound(
        ICombatState? combatState,
        PlayerChoiceContext choiceContext,
        CardModel card)
    {
        if (combatState != null)
            return Dispatch<IOnCardPlayedViaAshbound>(
                combatState,
                choiceContext,
                listener => listener.OnCardPlayedViaAshbound( 
                    combatState, choiceContext, card));
        return Task.CompletedTask;
    }
}