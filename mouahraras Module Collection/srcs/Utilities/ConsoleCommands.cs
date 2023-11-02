using System.IO;
using System.Collections.Generic;
using StardewModdingAPI;
using StardewValley;

namespace mouahrarasModuleCollection.Utilities
{
	internal class ConsoleCommandsUtility
	{
		internal static void Load()
		{
			ModEntry.Helper.ConsoleCommands.Add("mmc_uninstall", $"Usage: mmc_uninstall\nUninstall the mouahrara's Module Collection mod", (_, _) => mmc_uninstall());
		}

		private static void mmc_uninstall()
		{
			if (!Context.IsWorldReady)
			{
				ModEntry.Monitor.Log(ModEntry.Helper.Translation.Get("ConsoleCommands.NoSaveLoaded"), LogLevel.Warn);
				return;
			}
			if (!Game1.IsMasterGame)
			{
				ModEntry.Monitor.Log(ModEntry.Helper.Translation.Get("ConsoleCommands.NotMasterplayer"), LogLevel.Warn);
				return;
			}

			DisableModules();
			RebuildJunimoKartLeaderboard();
			ModEntry.Monitor.Log(ModEntry.Helper.Translation.Get("ConsoleCommands.CompleteUninstallation", new { ModName = ModEntry.ModManifest.Name, ModsFolder = Path.Combine(Path.GetDirectoryName(ModEntry.Helper.GetType().Assembly.Location), "Mods")}), LogLevel.Info);
		}

		private static void DisableModules()
		{
			ModEntry.Config.ArcadeGamesPayToPlay = false;
			ModEntry.Config.ArcadeGamesPayToPlayKonamiCode = false;
			ModEntry.Config.ArcadeGamesPayToPlayNotRealisticLeaderboard = false;
			ModEntry.Config.ClintsShopSimultaneousServices = false;
			ModEntry.Config.ClintsShopGeodesAutoProcess = false;
			ModEntry.Config.FestivalsEndTime = false;
			ModEntry.Config.FarmViewZoom = false;
			ModEntry.Config.FarmViewFastScrolling = false;
			ModEntry.Config.MarniesShopAnimalPurchase = false;
			ModEntry.Monitor.Log(ModEntry.Helper.Translation.Get("ConsoleCommands.DisableModulesSuccess"), LogLevel.Info);
		}

		private static void	RebuildJunimoKartLeaderboard()
		{
			List<KeyValuePair<string, int>> leaderbord = Game1.player.team.junimoKartScores.GetScores();

			AddIfNotExists(leaderbord, "Lewis", 50000);
			AddIfNotExists(leaderbord, "Shane", 25000);
			AddIfNotExists(leaderbord, "Sam", 10000);
			AddIfNotExists(leaderbord, "Abigail", 5000);
			AddIfNotExists(leaderbord, "Vincent", 250);
			ModEntry.Monitor.Log(ModEntry.Helper.Translation.Get("ConsoleCommands.RebuildJunimoKartLeaderboardSuccess"), LogLevel.Info);
		}

		private static void AddIfNotExists(List<KeyValuePair<string, int>> leaderbord, string playerName, int score)
		{
			foreach (KeyValuePair<string, int> entry in leaderbord)
			{
				if (entry.Key == playerName && entry.Value == score)
					return;
			}
			Game1.player.team.junimoKartScores.AddScore(playerName, score);
		}
	}
}
