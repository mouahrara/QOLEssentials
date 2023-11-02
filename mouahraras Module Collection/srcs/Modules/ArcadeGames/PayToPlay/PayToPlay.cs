using System;
using HarmonyLib;
using StardewModdingAPI;
using mouahrarasModuleCollection.ArcadeGames.PayToPlay.Hooks;
using mouahrarasModuleCollection.ArcadeGames.PayToPlay.Patches;

namespace mouahrarasModuleCollection.ArcadeGames.SubModules
{
	internal class PayToPlaySubModule
	{
		internal static void Apply(Harmony harmony)
		{
			// Load Harmony patches
			try
			{
				// Apply locations patches
				AbigailGamePatch.Apply(harmony);
				MineCartPatch.Apply(harmony);
			}
			catch (Exception e)
			{
				ModEntry.Monitor.Log($"Issue with Harmony patching of the {typeof(PayToPlaySubModule)} module: {e}", LogLevel.Error);
				return;
			}

			// Hook into the required events
			ModEntry.Helper.Events.GameLoop.SaveLoaded += SaveLoadedHook.Apply;
		}
	}
}
