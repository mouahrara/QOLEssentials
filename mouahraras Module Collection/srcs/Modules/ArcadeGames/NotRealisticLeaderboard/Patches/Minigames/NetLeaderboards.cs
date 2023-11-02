using System.Collections.Generic;
using HarmonyLib;
using StardewValley.Minigames;

namespace mouahrarasModuleCollection.ArcadeGames.NotRealisticLeaderboard.Patches
{
	internal class NetLeaderboardsPatch
	{
		internal static void Apply(Harmony harmony)
		{
			harmony.Patch(
				original: AccessTools.Method(typeof(NetLeaderboards), nameof(NetLeaderboards.GetScores)),
				postfix: new HarmonyMethod(typeof(NetLeaderboardsPatch), nameof(GetScoresPostfix))
			);
		}

		private static void GetScoresPostfix(NetLeaderboards __instance, ref List<KeyValuePair<string, int>> __result)
		{
			if (!ModEntry.Config.ArcadeGamesPayToPlayNotRealisticLeaderboard)
				return;

			List<KeyValuePair<string, int>> uniqueList = new();

			for (int i = 0; i < __result.Count; i++)
			{
				bool isDuplicate = false;

				for (int j = 0; j < i; j++)
				{
					if (__result[i].Key == __result[j].Key)
					{
						isDuplicate = true;
						break;
					}
				}

				if (!isDuplicate)
				{
					uniqueList.Add(__result[i]);
				}
			}
			__result = uniqueList;
		}
	}
}
