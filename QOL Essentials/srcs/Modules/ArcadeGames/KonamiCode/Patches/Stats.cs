using System;
using HarmonyLib;
using StardewValley;
using QOLEssentials.ArcadeGames.KonamiCode.Utilities;

namespace QOLEssentials.ArcadeGames.KonamiCode.Patches
{
	internal class StatsPatch
	{
		internal static void Apply(Harmony harmony)
		{
			harmony.Patch(
				original: AccessTools.Method(typeof(Stats), nameof(Stats.Increment), new Type[] { typeof(string), typeof(int) }),
				prefix: new HarmonyMethod(typeof(StatsPatch), nameof(IncrementPrefix))
			);
			harmony.Patch(
				original: AccessTools.Method(typeof(Stats), nameof(Stats.Increment), new Type[] { typeof(string), typeof(uint) }),
				prefix: new HarmonyMethod(typeof(StatsPatch), nameof(IncrementPrefix))
			);
		}

		private static bool IncrementPrefix(string key)
		{
			if (!ModEntry.Config.ArcadeGamesPayToPlayKonamiCode || !KonamiCodeUtility.InfiniteLivesMode)
				return true;

			return key != "completedPrairieKing" && key != "completedPrairieKingWithoutDying" && key != "completedJunimoKart";
		}
	}
}
