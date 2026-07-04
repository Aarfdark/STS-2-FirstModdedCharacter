using BaseLib.Abstracts;
using MyFirstCharacter.MyFirstCharacterCode.Extensions;
using Godot;

namespace MyFirstCharacter.MyFirstCharacterCode.Character;

public class MyFirstCharacterPotionPool : CustomPotionPoolModel
{
    public override Color LabOutlineColor => MyFirstCharacter.Color;


    public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();
}