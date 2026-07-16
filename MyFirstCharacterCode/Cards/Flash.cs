using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using MyFirstCharacter.MyFirstCharacterCode.Cards;
using MyFirstCharacter.MyFirstCharacterCode.Powers;

namespace MyFirstCharacter.MyFirstCharacterCode.Cards;

public class Flash() : MyFirstCharacterCard(3,
    CardType.Attack, CardRarity.Ancient,
    TargetType.AllEnemies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(12, ValueProp.Move), new DamageVar(12, ValueProp.Move), new PowerVar<CharmPower>(4)];
    public override bool GainsBlock => true;

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (CombatState == null)
            return;
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, play);
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this, play).TargetingAllOpponents(CombatState).WithHitFx("vfx/vfx_attack_blunt", tmpSfx: "heavy_attack.mp3").Execute(choiceContext);
        foreach (Creature hittableEnemy in CombatState.HittableEnemies)
        {
            await PowerCmd.Apply<CharmPower>(choiceContext, hittableEnemy, DynamicVars["CharmPower"].BaseValue, Owner.Creature, this);
        }
    }
    
    public override async Task AfterAutoPostPlayPhaseEntered(PlayerChoiceContext choiceContext, Player player)
    {
        if (player != Owner || PileType.Draw.GetPile(Owner).Cards.FirstOrDefault() != this)
            return;
        await CardPileCmd.AutoPlayFromDrawPile(choiceContext, Owner, 1, CardPilePosition.Top, false);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(4);
        DynamicVars.Damage.UpgradeValueBy(4);
        DynamicVars["CharmPower"].UpgradeValueBy(2);
    }
}