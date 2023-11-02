using System;
using HarmonyLib;
using StardewValley.Menus;
using mouahrarasModuleCollection.ClintsShop.GeodesAutoProcess.Utilities;

namespace mouahrarasModuleCollection.ClintsShop.GeodesAutoProcess.Patches
{
	internal class MenuWithInventoryPatch
	{
		internal static void Apply(Harmony harmony)
		{
			harmony.Patch(
				original: AccessTools.Method(typeof(MenuWithInventory), nameof(MenuWithInventory.receiveLeftClick), new Type[] { typeof(int), typeof(int), typeof(bool) }),
				prefix: new HarmonyMethod(typeof(MenuWithInventoryPatch), nameof(ReceiveKeyPressPrefix))
			);
		}

		private static bool ReceiveKeyPressPrefix(MenuWithInventory __instance, int x, int y, bool playSound = true)
		{
			if (!ModEntry.Config.ClintsShopGeodesAutoProcess)
				return true;

			if (__instance.GetType() == typeof(GeodeMenu))
			{
				if (__instance.okButton != null && __instance.okButton.containsPoint(x, y) && !(__instance as GeodeMenu).readyToClose())
				{
					GeodesAutoProcessUtility.EndGeodeProcessing();
					return false;
				}
			}
			return true;
		}
	}
}
