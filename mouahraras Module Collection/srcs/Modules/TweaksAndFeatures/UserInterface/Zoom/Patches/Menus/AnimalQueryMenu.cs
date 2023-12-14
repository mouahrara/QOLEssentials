using HarmonyLib;
using StardewValley.Menus;
using mouahrarasModuleCollection.TweaksAndFeatures.UserInterface.Zoom.Utilities;

namespace mouahrarasModuleCollection.TweaksAndFeatures.UserInterface.Zoom.Patches
{
	internal class AnimalQueryMenuPatch
	{
		internal static void Apply(Harmony harmony)
		{
			harmony.Patch(
				original: AccessTools.Method(typeof(AnimalQueryMenu), nameof(AnimalQueryMenu.prepareForAnimalPlacement)),
				postfix: new HarmonyMethod(typeof(MenusPatchUtility), nameof(MenusPatchUtility.EnterFarmViewPostfix))
			);
			harmony.Patch(
				original: AccessTools.Method(typeof(AnimalQueryMenu), nameof(AnimalQueryMenu.prepareForReturnFromPlacement)),
				postfix: new HarmonyMethod(typeof(MenusPatchUtility), nameof(MenusPatchUtility.LeaveFarmViewPostfix))
			);
		}
	}
}
