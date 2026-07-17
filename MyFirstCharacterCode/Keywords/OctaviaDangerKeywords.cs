using BaseLib.Patches.Content;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace MyFirstCharacter.MyFirstCharacterCode.Keywords;

public class OctaviaDangerKeywords
{
    [CustomEnum, KeywordProperties(AutoKeywordPosition.Before)] 
    public static CardKeyword Ashbound;
    
    [CustomEnum, KeywordProperties(AutoKeywordPosition.After)]
    public static CardKeyword Rigged;

}