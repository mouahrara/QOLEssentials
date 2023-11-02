using System;
using HarmonyLib;
using StardewValley;
using mouahrarasModuleCollection.ArcadeGames.KonamiCode.Utilities;

namespace mouahrarasModuleCollection.ArcadeGames.KonamiCode.Patches
{
	internal class Game1Patch
	{
		internal static void Apply(Harmony harmony)
		{
			harmony.Patch(
				original: AccessTools.Method(typeof(Game1), nameof(Game1.addMailForTomorrow), new Type[] { typeof(string), typeof(bool), typeof(bool) }),
				prefix: new HarmonyMethod(typeof(Game1Patch), nameof(AddMailForTomorrowPrefix))
			);
			harmony.Patch(
				original: AccessTools.Method(typeof(Game1), nameof(Game1.getSteamAchievement), new Type[] { typeof(string) }),
				prefix: new HarmonyMethod(typeof(Game1Patch), nameof(GetSteamAchievementPrefix))
			);
		}

		private static bool AddMailForTomorrowPrefix(Game1 __instance, string mailName)
		{
			if (!ModEntry.Config.ArcadeGamesPayToPlayKonamiCode || !KonamiCodeUtility.GetInfiniteLivesMode())
				return true;

			if (mailName.Equals("Beat_PK") || mailName.Equals("JunimoKart"))
				return false;
			return true;
		}

		private static bool GetSteamAchievementPrefix(Game1 __instance, string which)
		{
			if (!ModEntry.Config.ArcadeGamesPayToPlayKonamiCode || !KonamiCodeUtility.GetInfiniteLivesMode())
				return true;

			if (which.Equals("Achievement_PrairieKing") || which.Equals("Achievement_FectorsChallenge"))
				return false;
			return true;
		}
	}
}
