using System;
using HarmonyLib;
using StardewValley;
using StardewValley.Menus;
using mouahrarasModuleCollection.ClintsShop.GeodesAutoProcess.Utilities;

namespace mouahrarasModuleCollection.ClintsShop.GeodesAutoProcess.Patches
{
	internal class FarmerPatch
	{
		internal static void Apply(Harmony harmony)
		{
			harmony.Patch(
				original: AccessTools.Method(typeof(Farmer), nameof(Farmer.holdUpItemThenMessage), new Type[] { typeof(Item), typeof(bool) }),
				prefix: new HarmonyMethod(typeof(FarmerPatch), nameof(HoldUpItemThenMessagePrefix))
			);
		}

		private static bool HoldUpItemThenMessagePrefix(Farmer __instance, Item item, bool showMessage)
		{
			if (!ModEntry.Config.ClintsShopGeodesAutoProcess)
				return true;
			if (Game1.activeClickableMenu == null || Game1.activeClickableMenu.GetType() != typeof(GeodeMenu))
				return true;
			GeodesAutoProcessUtility.SetFoundArtifact(item);
			return false;
		}
	}
}
