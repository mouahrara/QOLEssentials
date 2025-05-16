using System;
using HarmonyLib;
using StardewModdingAPI;
using StardewValley.Menus;
using mouahrarasModuleCollection.Shops.GeodesAutoProcess.Utilities;

namespace mouahrarasModuleCollection.Shops.GeodesAutoProcess.Patches
{
	internal class MenuWithInventoryPatch
	{
		internal static void Apply(Harmony harmony)
		{
			harmony.Patch(
				original: AccessTools.Method(typeof(MenuWithInventory), nameof(MenuWithInventory.receiveLeftClick), new Type[] { typeof(int), typeof(int), typeof(bool) }),
				prefix: new HarmonyMethod(typeof(MenuWithInventoryPatch), nameof(ReceiveLeftClickPrefix))
			);
		}

		private static bool ReceiveLeftClickPrefix(MenuWithInventory __instance, int x, int y)
		{
			if (!ModEntry.Config.ShopsGeodesAutoProcess || __instance is not GeodeMenu geodeMenu)
				return true;

			if (Constants.TargetPlatform == GamePlatform.Android)
			{
				if (__instance.upperRightCloseButton is not null && __instance.upperRightCloseButton.containsPoint(x, y) && !geodeMenu.readyToClose())
				{
					GeodesAutoProcessUtility.EndGeodeProcessing();
					return false;
				}
			}
			else
			{
				if (__instance.okButton is not null && __instance.okButton.containsPoint(x, y) && !geodeMenu.readyToClose())
				{
					GeodesAutoProcessUtility.EndGeodeProcessing();
					return false;
				}
			}
			return true;
		}
	}
}
