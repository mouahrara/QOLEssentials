using System;
using HarmonyLib;
using StardewModdingAPI;
using mouahrarasModuleCollection.ArcadeGames.NonRealisticLeaderboard.Patches;

namespace mouahrarasModuleCollection.ArcadeGames.SubModules
{
	internal class NonRealisticLeaderboardSubModule
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
				ModEntry.Monitor.Log($"Issue with Harmony patching of the {typeof(NonRealisticLeaderboardSubModule)} module: {e}", LogLevel.Error);
				return;
			}
		}
	}
}
