// using System.Reflection;
// using HarmonyLib;
// using MegaCrit.Sts2.Core.Commands;
// using MegaCrit.Sts2.Core.Events;
// using MegaCrit.Sts2.Core.Helpers;
// using MegaCrit.Sts2.Core.Models;
// using MegaCrit.Sts2.Core.Models.Events;
// using MyFirstCharacter.MyFirstCharacterCode.Relics;
//
// namespace MyFirstCharacter.MyFirstCharacterCode.Patches;
//
// [HarmonyPatch(typeof(Neow), "CurseOptions", MethodType.Getter)]
// public class AddCursedNeowOptionsPatch
// {
//     public static void Postfix(Neow instance, ref IEnumerable<EventOption> result)
//     {
//         List<EventOption> list = result.ToList();
//         list.Add(RelicOption<NeowsBounty>("INITIAL", "NEOW.pages.DONE.POSITIVE.description", instance));
//         result = list;
//     }
//
//     protected static EventOption RelicOption<T>(string pageName = "INITIAL", string? customDonePage = null, Neow neow = null) where T : RelicModel
//     {
//         return RelicOption(ModelDb.Relic<T>().ToMutable(), pageName, null, neow);
//     }
//
//     protected static EventOption RelicOption(RelicModel relic, string pageName = "INITIAL", string? customDonePage = null, Neow neow = null!)
//     {
//         relic.AssertMutable();
//         relic.Owner = neow.Owner!;
//         string textKey = $"{StringHelper.Slugify(neow.GetType().Name)}.pages.{pageName}.options.{relic.Id.Entry}";
//         return EventOption.FromRelic(relic, neow, OnChosen, textKey);
//         async Task OnChosen()
//         {
//             await RelicCmd.Obtain(relic, neow.Owner!);
//             PropertyInfo customDonePageProp = typeof(AncientEventModel).GetProperty("CustomDonePage", BindingFlags.Instance | BindingFlags.NonPublic)!;
//             customDonePageProp.SetValue(neow, "NEOW.pages.DONE.POSITIVE.description");
//             MethodInfo doneMethod = typeof(AncientEventModel).GetMethod("Done", BindingFlags.Instance | BindingFlags.NonPublic)!;
//             doneMethod!.Invoke(neow, null);
//         }
//     }
// }