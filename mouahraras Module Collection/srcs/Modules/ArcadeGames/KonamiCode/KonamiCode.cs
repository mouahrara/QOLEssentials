using System;
using HarmonyLib;
using StardewModdingAPI;
using mouahrarasModuleCollection.ArcadeGames.KonamiCode.Patches;

namespace mouahrarasModuleCollection.ArcadeGames.SubModules
{
	internal class KonamiCodeSubModule
	{
		internal static void Apply(Harmony harmony)
		{
			// Load Harmony patches
			try
			{
				// Apply minigames patches
				AbigailGamePatch.Apply(harmony);
				MineCartPatch.Apply(harmony);

				// Apply Game1 patches
				Game1Patch.Apply(harmony);

				// Apply network patches
				MultiplayerPatch.Apply(harmony);
			}
			catch (Exception e)
			{
				ModEntry.Monitor.Log($"Issue with Harmony patching of the {typeof(KonamiCodeSubModule)} module: {e}", LogLevel.Error);
				return;
			}
		}
	}
}
