using System;
using HarmonyLib;
using StardewModdingAPI;
using QOLEssentials.ArcadeGames.PayToPlay.Handlers;
using QOLEssentials.ArcadeGames.PayToPlay.Patches;

namespace QOLEssentials.Modules
{
	internal class PayToPlayModule
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
				ModEntry.Monitor.Log($"Issue with Harmony patching of the {typeof(PayToPlayModule)} module: {e}", LogLevel.Error);
				return;
			}

			// Subscribe to events
			ModEntry.Helper.Events.GameLoop.SaveLoaded += SaveLoadedHandler.Apply;
		}
	}
}
