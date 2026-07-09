using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Events;
using MyFirstCharacter.MyFirstCharacterCode.Relics;

namespace MyFirstCharacter.MyFirstCharacterCode.Ancients;

public class BigNeow : CustomAncientModel
{
    protected override OptionPools MakeOptionPools => new(
        MakePool(AncientOption<NeowsBounty>()),
        MakePool(AncientOption<NeowsBounty>()),
        MakePool(AncientOption<NeowsBounty>())
    );

    public override bool ShouldForceSpawn(ActModel act, AncientEventModel? rngChosenAncient)
    {
        // if (rngChosenAncient is Neow)
        //     return true;
        return false;
    }

    public override bool IsValidForAct(ActModel act)
    {
        return false;
    }
    
    public override string CustomScenePath => "res://scenes/events/background_scenes/neow.tscn";
    public override string CustomMapIconPath => "res://packed/map/ancients/ancient_node_neow.png";
    public override string CustomMapIconOutlinePath => "res://packed/map/ancients/ancient_node_neow_outline.png";
}