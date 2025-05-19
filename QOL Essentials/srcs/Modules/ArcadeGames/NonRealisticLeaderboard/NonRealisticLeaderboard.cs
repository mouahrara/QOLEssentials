using System;
using HarmonyLib;
using StardewModdingAPI;
using QOLEssentials.ArcadeGames.NonRealisticLeaderboard.Patches;

namespace QOLEssentials.Modules
{
	internal class NonRealisticLeaderboardModule
	{
		internal static void Apply(Harmony harmony)
		{
			// Load Harmony patches
			try
			{
				// Apply locations patches
				NetLeaderboardsPatch.Apply(harmony);
			}
			catch (Exception e)
			{
				ModEntry.Monitor.Log($"Issue with Harmony patching of the {typeof(NonRealisticLeaderboardModule)} module: {e}", LogLevel.Error);
				return;
			}
		}
	}
}
