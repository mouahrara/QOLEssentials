using System;
using HarmonyLib;
using StardewModdingAPI;
using mouahrarasModuleCollection.TweaksAndFeatures.ArcadeGames.NonRealisticLeaderboard.Patches;

namespace mouahrarasModuleCollection.Modules
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
