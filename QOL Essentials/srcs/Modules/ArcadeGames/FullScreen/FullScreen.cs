using System;
using HarmonyLib;
using StardewModdingAPI;
using QOLEssentials.ArcadeGames.FullScreen.Patches;

namespace QOLEssentials.Modules
{
	internal class FullScreenModule
	{
		internal static void Apply(Harmony harmony)
		{
			// Load Harmony patches
			try
			{
				// Apply minigames patches
				AbigailGamePatch.Apply(harmony);
				MineCartPatch.Apply(harmony);
			}
			catch (Exception e)
			{
				ModEntry.Monitor.Log($"Issue with Harmony patching of the {typeof(FullScreenModule)} module: {e}", LogLevel.Error);
				return;
			}
		}
	}
}
