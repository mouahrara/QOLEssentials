using System;
using HarmonyLib;
using StardewValley;
using StardewValley.Menus;
using mouahrarasModuleCollection.TweaksAndFeatures.Shops.GeodesAutoProcess.Utilities;

namespace mouahrarasModuleCollection.TweaksAndFeatures.Shops.GeodesAutoProcess.Patches
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

		private static bool HoldUpItemThenMessagePrefix(Item item)
		{
			if (!ModEntry.Config.ShopsGeodesAutoProcess)
				return true;
			if (Game1.activeClickableMenu == null || Game1.activeClickableMenu.GetType() != typeof(GeodeMenu))
				return true;
			GeodesAutoProcessUtility.FoundArtifact = item;
			return false;
		}
	}
}
