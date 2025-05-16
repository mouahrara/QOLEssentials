using System;
using HarmonyLib;
using StardewValley;
using StardewValley.Menus;
using mouahrarasModuleCollection.Shops.GeodesAutoProcess.Utilities;

namespace mouahrarasModuleCollection.Shops.GeodesAutoProcess.Patches
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
			if (!ModEntry.Config.ShopsGeodesAutoProcess || Game1.activeClickableMenu is not GeodeMenu)
				return true;

			GeodesAutoProcessUtility.FoundArtifact = item;
			return false;
		}
	}
}
