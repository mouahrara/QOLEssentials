using System;
using HarmonyLib;
using StardewValley;
using StardewValley.Menus;
using mouahrarasModuleCollection.UserInterface.Zoom.Utilities;

namespace mouahrarasModuleCollection.UserInterface.Zoom.Patches
{
	internal class IClickableMenuPatch
	{
		internal static void Apply(Harmony harmony)
		{
			harmony.Patch(
				original: AccessTools.Method(typeof(IClickableMenu), nameof(IClickableMenu.receiveScrollWheelAction), new Type[] { typeof(int) }),
				postfix: new HarmonyMethod(typeof(IClickableMenuPatch), nameof(ReceiveScrollWheelActionPostfix))
			);
		}

		private static void ReceiveScrollWheelActionPostfix(IClickableMenu __instance, int direction)
		{
			if (!MenusPatchUtility.ShouldProcess(__instance))
				return;

			ZoomUtility.AddZoomLevel(direction * 2);
		}
	}
}
