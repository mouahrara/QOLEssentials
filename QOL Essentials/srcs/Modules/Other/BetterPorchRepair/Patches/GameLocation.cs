using System;
using HarmonyLib;
using StardewValley;
using Microsoft.Xna.Framework;
using StardewValley.Buildings;
using QOLEssentials.Other.BetterPorchRepair.Utilities;

namespace QOLEssentials.Other.BetterPorchRepair.Patches
{
	internal class GameLocationPatch
	{
		internal static void Apply(Harmony harmony)
		{
			harmony.Patch(
				original: AccessTools.Method(typeof(GameLocation), nameof(GameLocation.isCollidingPosition), new Type[] { typeof(Rectangle), typeof(xTile.Dimensions.Rectangle), typeof(bool), typeof(int), typeof(bool), typeof(Character), typeof(bool), typeof(bool), typeof(bool), typeof(bool) }),
				postfix: new HarmonyMethod(typeof(GameLocationPatch), nameof(IsCollidingPositionPostfix))
			);
		}

		private static void IsCollidingPositionPostfix(GameLocation __instance, Rectangle position, ref bool __result)
		{
			if (!ModEntry.Config.OtherBetterPorchRepair || __instance is not Farm farm || __result)
				return;

			Building farmhouse = farm.GetMainFarmHouse();

			if (farmhouse is not null && position.Intersects(new Rectangle((farmhouse.tileX.Value + (CompatibilityUtility.IsFlipBuildingsLoaded && farmhouse.modData.ContainsKey(CompatibilityUtility.flippedKey) ? farmhouse.tilesWide.Value : 0)) * Game1.tileSize, (farmhouse.tileY.Value + 3) * Game1.tileSize, 0, Game1.tileSize)))
			{
				__result = true;
			}
		}
	}
}
