using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using MyFirstCharacter.MyFirstCharacterCode.Cards;

namespace MyFirstCharacter.MyFirstCharacterCode.Cards;

[Pool(typeof(ColorlessCardPool))]
public class Bramble() : MyFirstCharacterCard(1,
    CardType.Attack, CardRarity.Uncommon,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<ThornsPower>(2), new DynamicVar("Increase", 1)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<ThornsPower>(choiceContext, Owner.Creature, DynamicVars["ThornsPower"].BaseValue,
            Owner.Creature, this);
        var incThorns = DynamicVars["Increase"].BaseValue;
        var thornsVar = DynamicVars["ThornsPower"];
        thornsVar.BaseValue += incThorns;
    }

    protected override void OnUpgrade()
    {
        DynamicVars["ThornsPower"].BaseValue += 1;
        DynamicVars["Increase"].BaseValue += 1;
    }
}