using System;
using HarmonyLib;
using StardewModdingAPI;
using mouahrarasModuleCollection.ArcadeGames.KonamiCode.Patches;

namespace mouahrarasModuleCollection.Modules
{
	internal class KonamiCodeModule
	{
		internal static void Apply(Harmony harmony)
		{
			// Load Harmony patches
			try
			{
				// Apply minigames patches
				AbigailGamePatch.Apply(harmony);
				MineCartPatch.Apply(harmony);

				// Apply Stats patches
				StatsPatch.Apply(harmony);

				// Apply network patches
				MultiplayerPatch.Apply(harmony);
			}
			catch (Exception e)
			{
				ModEntry.Monitor.Log($"Issue with Harmony patching of the {typeof(KonamiCodeModule)} module: {e}", LogLevel.Error);
				return;
			}
		}
	}
}
