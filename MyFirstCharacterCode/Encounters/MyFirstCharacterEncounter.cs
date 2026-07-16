using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Rooms;

namespace MyFirstCharacter.MyFirstCharacterCode.Encounters;

public abstract class MyFirstCharacterEncounter : CustomEncounterModel
{
    public MyFirstCharacterEncounter(RoomType roomType, bool autoAdd = true) : base(roomType, autoAdd)
    {
        
    }
}