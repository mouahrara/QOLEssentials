using HarmonyLib;
using StardewValley.Menus;
using QOLEssentials.UserInterface.Zoom.Utilities;

namespace QOLEssentials.UserInterface.Zoom.Patches
{
	internal class CarpenterMenuPatch
	{
		internal static void Apply(Harmony harmony)
		{
			harmony.Patch(
				original: AccessTools.Method(typeof(CarpenterMenu), nameof(CarpenterMenu.setUpForBuildingPlacement)),
				postfix: new HarmonyMethod(typeof(MenusPatchUtility), nameof(MenusPatchUtility.EnterFarmViewPostfix))
			);
			harmony.Patch(
				original: AccessTools.Method(typeof(CarpenterMenu), nameof(CarpenterMenu.returnToCarpentryMenu)),
				postfix: new HarmonyMethod(typeof(MenusPatchUtility), nameof(MenusPatchUtility.LeaveFarmViewPostfix))
			);
			harmony.Patch(
				original: AccessTools.Method(typeof(CarpenterMenu), nameof(CarpenterMenu.returnToCarpentryMenuAfterSuccessfulBuild)),
				postfix: new HarmonyMethod(typeof(MenusPatchUtility), nameof(MenusPatchUtility.LeaveFarmViewPostfix))
			);
		}
	}
}
