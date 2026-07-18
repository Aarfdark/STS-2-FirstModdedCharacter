using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace MyFirstCharacter.MyFirstCharacterCode.Hooks;

public interface IOnCardPlayedViaAshbound
{
    Task OnCardPlayedViaAshbound(
        ICombatState combatState,
        PlayerChoiceContext choiceContext,
        CardModel card);
}