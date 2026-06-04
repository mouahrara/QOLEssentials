using StardewModdingAPI;
using QOLEssentials.Other.BetterPorchRepair.Utilities;

namespace QOLEssentials.Utilities
{
	public sealed class ModConfig
	{
		public bool		ArcadeGamesFullScreen = true;
		public float	ArcadeGamesFullScreenFillRatio = 1.0f;
		public bool		ArcadeGamesPayToPlay = true;
		public int		ArcadeGamesPayToPlayCoinPerJotPKGame = 1;
		public int		ArcadeGamesPayToPlayCoinPerJKGame = 1;
		public bool		ArcadeGamesKonamiCode = true;
		public bool		ArcadeGamesNonRealisticLeaderboard = true;
		public bool		MachinesFastReplacement = true;
		public bool		ShopsBetterAnimalPurchase = true;
		public SButton	ShopsBetterAnimalPurchaseSecondaryPreviousKey = SButton.LeftShoulder;
		public SButton	ShopsBetterAnimalPurchaseSecondaryNextKey = SButton.RightShoulder;
		public bool		ShopsGeodesAutoProcess = true;
		public int		ShopsGeodesAutoProcessProcessSpeedMultiplier = 2;
		public bool		UserInterfaceFastScrolling = true;
		public float	UserInterfaceFastScrollingScrollSpeedMultiplier = 3.0f;
		public bool		UserInterfaceFastScrollingConsistentScrolling = true;
		public bool		UserInterfaceZoom = true;
		public SButton	UserInterfaceZoomSecondaryZoomInKey = SButton.RightTrigger;
		public SButton	UserInterfaceZoomSecondaryZoomOutKey = SButton.LeftTrigger;
		public float	UserInterfaceZoomZoomSpeedMultiplier = 1.0f;
		public float	UserInterfaceZoomMinimumZoomLevel = 0.25f;
		public bool		OtherBetterPorchRepair = true;
		public bool		OtherFestivalEndTime = true;
		public int		OtherFestivalEndTimeAdditionalTime = 200;
	}

	internal class GMCMUtility
	{
		internal static void Initialize()
		{
			ReadConfig();
			Register();
		}

		private static void ReadConfig()
		{
			ModEntry.Config = ModEntry.Helper.ReadConfig<ModConfig>();
		}

		private static void Register()
		{
			// Get Generic Mod Config Menu's API
			GenericModConfigMenu.IGenericModConfigMenuApi gmcm = ModEntry.Helper.ModRegistry.GetApi<GenericModConfigMenu.IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
			if (gmcm is null)
				return;

			// Register mod
			gmcm.Register(
				mod: ModEntry.ModManifest,
				reset: () => ModEntry.Config = new ModConfig(),
				save: () => ModEntry.Helper.WriteConfig(ModEntry.Config)
			);

			// Main
			gmcm.AddPageLink(
				mod: ModEntry.ModManifest,
				pageId: "Arcade games",
				text: () => "> " + ModEntry.Helper.Translation.Get("GMCM.ArcadeGames.Title")
			);
			gmcm.AddPageLink(
				mod: ModEntry.ModManifest,
				pageId: "Machines",
				text: () => "> " + ModEntry.Helper.Translation.Get("GMCM.Machines.Title")
			);
			gmcm.AddPageLink(
				mod: ModEntry.ModManifest,
				pageId: "Shops",
				text: () => "> " + ModEntry.Helper.Translation.Get("GMCM.Shops.Title")
			);
			if (Constants.TargetPlatform != GamePlatform.Android)
			{
				gmcm.AddPageLink(
					mod: ModEntry.ModManifest,
					pageId: "User Interface",
					text: () => "> " + ModEntry.Helper.Translation.Get("GMCM.UserInterface.Title")
				);
			}
			gmcm.AddPageLink(
				mod: ModEntry.ModManifest,
				pageId: "Other",
				text: () => "> " + ModEntry.Helper.Translation.Get("GMCM.Other.Title")
			);

			// Arcade games
			gmcm.AddPage(
				mod: ModEntry.ModManifest,
				pageId: "Arcade games",
				pageTitle: () => ModEntry.Helper.Translation.Get("GMCM.ArcadeGames.Title")
			);
			// gmcm.AddPageLink(
			// 	mod: ModEntry.ModManifest,
			// 	pageId: ModEntry.ModManifest.UniqueID,
			// 	text: () => "@ " + "Back to QOL Essentials"
			// );
			gmcm.AddParagraph(
				mod: ModEntry.ModManifest,
				text: () => ModEntry.Helper.Translation.Get("GMCM.ArcadeGames.Description")
			);
			if (Constants.TargetPlatform != GamePlatform.Android)
			{
				gmcm.AddPageLink(
					mod: ModEntry.ModManifest,
					pageId: "Arcade games - Full screen",
					text: () => "> " + ModEntry.Helper.Translation.Get("GMCM.ArcadeGames.FullScreen.Title"),
					tooltip: () => ModEntry.Helper.Translation.Get("GMCM.ArcadeGames.FullScreen.Tooltip")
				);
				gmcm.AddPageLink(
					mod: ModEntry.ModManifest,
					pageId: "Arcade games - Konami code",
					text: () => "> " + ModEntry.Helper.Translation.Get("GMCM.ArcadeGames.KonamiCode.Title"),
					tooltip: () => ModEntry.Helper.Translation.Get("GMCM.ArcadeGames.KonamiCode.Tooltip")
				);
			}
			gmcm.AddPageLink(
				mod: ModEntry.ModManifest,
				pageId: "Arcade games - Non-realistic leaderboard",
				text: () => "> " + ModEntry.Helper.Translation.Get("GMCM.ArcadeGames.NonRealisticLeaderboard.Title"),
				tooltip: () => ModEntry.Helper.Translation.Get("GMCM.ArcadeGames.NonRealisticLeaderboard.Tooltip")
			);
			gmcm.AddPageLink(
				mod: ModEntry.ModManifest,
				pageId: "Arcade games - Pay-to-play",
				text: () => "> " + ModEntry.Helper.Translation.Get("GMCM.ArcadeGames.PayToPlay.Title"),
				tooltip: () => ModEntry.Helper.Translation.Get("GMCM.ArcadeGames.PayToPlay.Tooltip")
			);
			if (Constants.TargetPlatform != GamePlatform.Android)
			{
				// Arcade games - Full screen
				gmcm.AddPage(
					mod: ModEntry.ModManifest,
					pageId: "Arcade games - Full screen",
					pageTitle: () => ModEntry.Helper.Translation.Get("GMCM.ArcadeGames.FullScreen.Title")
				);
				gmcm.AddPageLink(
					mod: ModEntry.ModManifest,
					pageId: "Arcade games",
					text: () => "@ " + ModEntry.Helper.Translation.Get("GMCM.ArcadeGames.BackTo")
				);
				gmcm.AddParagraph(
					mod: ModEntry.ModManifest,
					text: () => ModEntry.Helper.Translation.Get("GMCM.ArcadeGames.FullScreen.Description")
				);
				gmcm.AddBoolOption(
					mod: ModEntry.ModManifest,
					name: () => ModEntry.Helper.Translation.Get("GMCM.Modules.Enabled.Title"),
					tooltip: () => ModEntry.Helper.Translation.Get("GMCM.Modules.Enabled.Tooltip"),
					getValue: () => ModEntry.Config.ArcadeGamesFullScreen,
					setValue: value => ModEntry.Config.ArcadeGamesFullScreen = value
				);
				gmcm.AddNumberOption(
					mod: ModEntry.ModManifest,
					name: () => ModEntry.Helper.Translation.Get("GMCM.ArcadeGames.FullScreen.FillRatio.Title"),
					tooltip: () => ModEntry.Helper.Translation.Get("GMCM.ArcadeGames.FullScreen.FillRatio.Tooltip"),
					getValue: () => ModEntry.Config.ArcadeGamesFullScreenFillRatio * 100f,
					setValue: value => ModEntry.Config.ArcadeGamesFullScreenFillRatio = value / 100f,
					min: 50f,
					max: 100f,
					interval: 5f
				);
				// Arcade games - Konami code
				gmcm.AddPage(
					mod: ModEntry.ModManifest,
					pageId: "Arcade games - Konami code",
					pageTitle: () => ModEntry.Helper.Translation.Get("GMCM.ArcadeGames.KonamiCode.Title")
				);
				gmcm.AddPageLink(
					mod: ModEntry.ModManifest,
					pageId: "Arcade games",
					text: () => "@ " + ModEntry.Helper.Translation.Get("GMCM.ArcadeGames.BackTo")
				);
				gmcm.AddParagraph(
					mod: ModEntry.ModManifest,
					text: () => ModEntry.Helper.Translation.Get("GMCM.ArcadeGames.KonamiCode.Description")
				);
				gmcm.AddParagraph(
					mod: ModEntry.ModManifest,
					text: () => ModEntry.Helper.Translation.Get("GMCM.ArcadeGames.KonamiCode.AdditionalInformation")
				);
				gmcm.AddBoolOption(
					mod: ModEntry.ModManifest,
					name: () => ModEntry.Helper.Translation.Get("GMCM.Modules.Enabled.Title"),
					tooltip: () => ModEntry.Helper.Translation.Get("GMCM.Modules.Enabled.Tooltip"),
					getValue: () => ModEntry.Config.ArcadeGamesKonamiCode,
					setValue: value => ModEntry.Config.ArcadeGamesKonamiCode = value
				);
			}
			// Arcade games - Non-realistic leaderboard
			gmcm.AddPage(
				mod: ModEntry.ModManifest,
				pageId: "Arcade games - Non-realistic leaderboard",
				pageTitle: () => ModEntry.Helper.Translation.Get("GMCM.ArcadeGames.NonRealisticLeaderboard.Title")
			);
			gmcm.AddPageLink(
				mod: ModEntry.ModManifest,
				pageId: "Arcade games",
				text: () => "@ " + ModEntry.Helper.Translation.Get("GMCM.ArcadeGames.BackTo")
			);
			gmcm.AddParagraph(
				mod: ModEntry.ModManifest,
				text: () => ModEntry.Helper.Translation.Get("GMCM.ArcadeGames.NonRealisticLeaderboard.Description")
			);
			gmcm.AddBoolOption(
				mod: ModEntry.ModManifest,
				name: () => ModEntry.Helper.Translation.Get("GMCM.Modules.Enabled.Title"),
				tooltip: () => ModEntry.Helper.Translation.Get("GMCM.Modules.Enabled.Tooltip"),
				getValue: () => ModEntry.Config.ArcadeGamesNonRealisticLeaderboard,
				setValue: value => ModEntry.Config.ArcadeGamesNonRealisticLeaderboard = value
			);
			// Arcade games - Pay-to-play
			gmcm.AddPage(
				mod: ModEntry.ModManifest,
				pageId: "Arcade games - Pay-to-play",
				pageTitle: () => ModEntry.Helper.Translation.Get("GMCM.ArcadeGames.PayToPlay.Title")
			);
			gmcm.AddPageLink(
				mod: ModEntry.ModManifest,
				pageId: "Arcade games",
				text: () => "@ " + ModEntry.Helper.Translation.Get("GMCM.ArcadeGames.BackTo")
			);
			gmcm.AddParagraph(
				mod: ModEntry.ModManifest,
				text: () => ModEntry.Helper.Translation.Get("GMCM.ArcadeGames.PayToPlay.Description")
			);
			gmcm.AddBoolOption(
				mod: ModEntry.ModManifest,
				name: () => ModEntry.Helper.Translation.Get("GMCM.Modules.Enabled.Title"),
				tooltip: () => ModEntry.Helper.Translation.Get("GMCM.Modules.Enabled.Tooltip"),
				getValue: () => ModEntry.Config.ArcadeGamesPayToPlay,
				setValue: value => ModEntry.Config.ArcadeGamesPayToPlay = value
			);
			gmcm.AddNumberOption(
				mod: ModEntry.ModManifest,
				name: () => ModEntry.Helper.Translation.Get("GMCM.ArcadeGames.PayToPlay.CoinPerJotPKGame.Title"),
				tooltip: () => ModEntry.Helper.Translation.Get("GMCM.ArcadeGames.PayToPlay.CoinPerJotPKGame.Tooltip"),
				getValue: () => ModEntry.Config.ArcadeGamesPayToPlayCoinPerJotPKGame,
				setValue: value => ModEntry.Config.ArcadeGamesPayToPlayCoinPerJotPKGame = value,
				min: 1,
				max: 5,
				interval: 1
			);
			gmcm.AddNumberOption(
				mod: ModEntry.ModManifest,
				name: () => ModEntry.Helper.Translation.Get("GMCM.ArcadeGames.PayToPlay.CoinPerJKGame.Title"),
				tooltip: () => ModEntry.Helper.Translation.Get("GMCM.ArcadeGames.PayToPlay.CoinPerJKGame.Tooltip"),
				getValue: () => ModEntry.Config.ArcadeGamesPayToPlayCoinPerJKGame,
				setValue: value => ModEntry.Config.ArcadeGamesPayToPlayCoinPerJKGame = value,
				min: 1,
				max: 5,
				interval: 1
			);

			// Machines
			gmcm.AddPage(
				mod: ModEntry.ModManifest,
				pageId: "Machines",
				pageTitle: () => ModEntry.Helper.Translation.Get("GMCM.Machines.Title")
			);
			// gmcm.AddPageLink(
			// 	mod: ModEntry.ModManifest,
			// 	pageId: ModEntry.ModManifest.UniqueID,
			// 	text: () => "@ " + "Back to QOL Essentials"
			// );
			gmcm.AddParagraph(
				mod: ModEntry.ModManifest,
				text: () => ModEntry.Helper.Translation.Get("GMCM.Machines.Description")
			);
			gmcm.AddPageLink(
				mod: ModEntry.ModManifest,
				pageId: "Machines - Fast replacement",
				text: () => "> " + ModEntry.Helper.Translation.Get("GMCM.Machines.FastReplacement.Title"),
				tooltip: () => ModEntry.Helper.Translation.Get("GMCM.Machines.FastReplacement.Tooltip")
			);
			// Machines - Fast replacement
			gmcm.AddPage(
				mod: ModEntry.ModManifest,
				pageId: "Machines - Fast replacement",
				pageTitle: () => ModEntry.Helper.Translation.Get("GMCM.Machines.FastReplacement.Title")
			);
			gmcm.AddPageLink(
				mod: ModEntry.ModManifest,
				pageId: "Machines",
				text: () => "@ " + ModEntry.Helper.Translation.Get("GMCM.Machines.BackTo")
			);
			gmcm.AddParagraph(
				mod: ModEntry.ModManifest,
				text: () => ModEntry.Helper.Translation.Get("GMCM.Machines.FastReplacement.Description")
			);
			gmcm.AddBoolOption(
				mod: ModEntry.ModManifest,
				name: () => ModEntry.Helper.Translation.Get("GMCM.Modules.Enabled.Title"),
				tooltip: () => ModEntry.Helper.Translation.Get("GMCM.Modules.Enabled.Tooltip"),
				getValue: () => ModEntry.Config.MachinesFastReplacement,
				setValue: value => ModEntry.Config.MachinesFastReplacement = value
			);

			// Shops
			gmcm.AddPage(
				mod: ModEntry.ModManifest,
				pageId: "Shops",
				pageTitle: () => ModEntry.Helper.Translation.Get("GMCM.Shops.Title")
			);
			// gmcm.AddPageLink(
			// 	mod: ModEntry.ModManifest,
			// 	pageId: ModEntry.ModManifest.UniqueID,
			// 	text: () => "@ " + "Back to QOL Essentials"
			// );
			gmcm.AddParagraph(
				mod: ModEntry.ModManifest,
				text: () => ModEntry.Helper.Translation.Get("GMCM.Shops.Description")
			);
			gmcm.AddPageLink(
				mod: ModEntry.ModManifest,
				pageId: "Shops - Better animal purchase",
				text: () => "> " + ModEntry.Helper.Translation.Get("GMCM.Shops.BetterAnimalPurchase.Title"),
				tooltip: () => ModEntry.Helper.Translation.Get("GMCM.Shops.BetterAnimalPurchase.Tooltip")
			);
			gmcm.AddPageLink(
				mod: ModEntry.ModManifest,
				pageId: "Shops - Geodes auto-process",
				text: () => "> " + ModEntry.Helper.Translation.Get("GMCM.Shops.GeodesAutoProcess.Title"),
				tooltip: () => ModEntry.Helper.Translation.Get("GMCM.Shops.GeodesAutoProcess.Tooltip")
			);
			// Shops - Better animal purchase
			gmcm.AddPage(
				mod: ModEntry.ModManifest,
				pageId: "Shops - Better animal purchase",
				pageTitle: () => ModEntry.Helper.Translation.Get("GMCM.Shops.BetterAnimalPurchase.Title")
			);
			gmcm.AddPageLink(
				mod: ModEntry.ModManifest,
				pageId: "Shops",
				text: () => "@ " + ModEntry.Helper.Translation.Get("GMCM.Shops.BackTo")
			);
			gmcm.AddParagraph(
				mod: ModEntry.ModManifest,
				text: () => ModEntry.Helper.Translation.Get("GMCM.Shops.BetterAnimalPurchase.Description")
			);
			gmcm.AddBoolOption(
				mod: ModEntry.ModManifest,
				name: () => ModEntry.Helper.Translation.Get("GMCM.Modules.Enabled.Title"),
				tooltip: () => ModEntry.Helper.Translation.Get("GMCM.Modules.Enabled.Tooltip"),
				getValue: () => ModEntry.Config.ShopsBetterAnimalPurchase,
				setValue: value => ModEntry.Config.ShopsBetterAnimalPurchase = value
			);
			gmcm.AddKeybind(
				mod: ModEntry.ModManifest,
				name: () => ModEntry.Helper.Translation.Get("GMCM.Shops.BetterAnimalPurchase.SecondaryPreviousKey.Title"),
				tooltip: () => ModEntry.Helper.Translation.Get("GMCM.Shops.BetterAnimalPurchase.SecondaryPreviousKey.Tooltip"),
				getValue: () => ModEntry.Config.ShopsBetterAnimalPurchaseSecondaryPreviousKey,
				setValue: value => ModEntry.Config.ShopsBetterAnimalPurchaseSecondaryPreviousKey = value
			);
			gmcm.AddKeybind(
				mod: ModEntry.ModManifest,
				name: () => ModEntry.Helper.Translation.Get("GMCM.Shops.BetterAnimalPurchase.SecondaryNextKey.Title"),
				tooltip: () => ModEntry.Helper.Translation.Get("GMCM.Shops.BetterAnimalPurchase.SecondaryNextKey.Tooltip"),
				getValue: () => ModEntry.Config.ShopsBetterAnimalPurchaseSecondaryNextKey,
				setValue: value => ModEntry.Config.ShopsBetterAnimalPurchaseSecondaryNextKey = value
			);
			// Shops - Geodes auto-process
			gmcm.AddPage(
				mod: ModEntry.ModManifest,
				pageId: "Shops - Geodes auto-process",
				pageTitle: () => ModEntry.Helper.Translation.Get("GMCM.Shops.GeodesAutoProcess.Title")
			);
			gmcm.AddPageLink(
				mod: ModEntry.ModManifest,
				pageId: "Shops",
				text: () => "@ " + ModEntry.Helper.Translation.Get("GMCM.Shops.BackTo")
			);
			gmcm.AddParagraph(
				mod: ModEntry.ModManifest,
				text: () => ModEntry.Helper.Translation.Get("GMCM.Shops.GeodesAutoProcess.Description")
			);
			gmcm.AddBoolOption(
				mod: ModEntry.ModManifest,
				name: () => ModEntry.Helper.Translation.Get("GMCM.Modules.Enabled.Title"),
				tooltip: () => ModEntry.Helper.Translation.Get("GMCM.Modules.Enabled.Tooltip"),
				getValue: () => ModEntry.Config.ShopsGeodesAutoProcess,
				setValue: value => ModEntry.Config.ShopsGeodesAutoProcess = value
			);
			gmcm.AddNumberOption(
				mod: ModEntry.ModManifest,
				name: () => ModEntry.Helper.Translation.Get("GMCM.Shops.GeodesAutoProcess.ProcessSpeedMultiplier.Title"),
				tooltip: () => ModEntry.Helper.Translation.Get("GMCM.Shops.GeodesAutoProcess.ProcessSpeedMultiplier.Tooltip"),
				getValue: () => ModEntry.Config.ShopsGeodesAutoProcessProcessSpeedMultiplier,
				setValue: value => ModEntry.Config.ShopsGeodesAutoProcessProcessSpeedMultiplier = value,
				min: 1,
				max: 20,
				interval: 1
			);

			// User Interface
			if (Constants.TargetPlatform != GamePlatform.Android)
			{
				gmcm.AddPage(
					mod: ModEntry.ModManifest,
					pageId: "User Interface",
					pageTitle: () => ModEntry.Helper.Translation.Get("GMCM.UserInterface.Title")
				);
				// gmcm.AddPageLink(
				// 	mod: ModEntry.ModManifest,
				// 	pageId: ModEntry.ModManifest.UniqueID,
				// 	text: () => "@ " + "Back to QOL Essentials"
				// );
				gmcm.AddParagraph(
					mod: ModEntry.ModManifest,
					text: () => ModEntry.Helper.Translation.Get("GMCM.UserInterface.Description")
				);
				gmcm.AddPageLink(
					mod: ModEntry.ModManifest,
					pageId: "User Interface - Fast scrolling",
					text: () => "> " + ModEntry.Helper.Translation.Get("GMCM.UserInterface.FastScrolling.Title"),
					tooltip: () => ModEntry.Helper.Translation.Get("GMCM.UserInterface.FastScrolling.Tooltip")
				);
				gmcm.AddPageLink(
					mod: ModEntry.ModManifest,
					pageId: "User Interface - Zoom",
					text: () => "> " + ModEntry.Helper.Translation.Get("GMCM.UserInterface.Zoom.Title"),
					tooltip: () => ModEntry.Helper.Translation.Get("GMCM.UserInterface.Zoom.Tooltip")
				);
				// User Interface - Fast scrolling
				gmcm.AddPage(
					mod: ModEntry.ModManifest,
					pageId: "User Interface - Fast scrolling",
					pageTitle: () => ModEntry.Helper.Translation.Get("GMCM.UserInterface.FastScrolling.Title")
				);
				gmcm.AddPageLink(
					mod: ModEntry.ModManifest,
					pageId: "User Interface",
					text: () => "@ " + ModEntry.Helper.Translation.Get("GMCM.UserInterface.BackTo")
				);
				gmcm.AddParagraph(
					mod: ModEntry.ModManifest,
					text: () => ModEntry.Helper.Translation.Get("GMCM.UserInterface.FastScrolling.Description")
				);
				gmcm.AddBoolOption(
					mod: ModEntry.ModManifest,
					name: () => ModEntry.Helper.Translation.Get("GMCM.Modules.Enabled.Title"),
					tooltip: () => ModEntry.Helper.Translation.Get("GMCM.Modules.Enabled.Tooltip"),
					getValue: () => ModEntry.Config.UserInterfaceFastScrolling,
					setValue: value => ModEntry.Config.UserInterfaceFastScrolling = value
				);
				gmcm.AddNumberOption(
					mod: ModEntry.ModManifest,
					name: () => ModEntry.Helper.Translation.Get("GMCM.UserInterface.FastScrolling.ScrollSpeedMultiplier.Title"),
					tooltip: () => ModEntry.Helper.Translation.Get("GMCM.UserInterface.FastScrolling.ScrollSpeedMultiplier.Tooltip"),
					getValue: () => ModEntry.Config.UserInterfaceFastScrollingScrollSpeedMultiplier,
					setValue: value => ModEntry.Config.UserInterfaceFastScrollingScrollSpeedMultiplier = value,
					min: 1.0f,
					max: 8.0f,
					interval: 0.25f
				);
				gmcm.AddBoolOption(
					mod: ModEntry.ModManifest,
					name: () => ModEntry.Helper.Translation.Get("GMCM.UserInterface.FastScrolling.ConsistentScrolling.Title"),
					tooltip: () => ModEntry.Helper.Translation.Get("GMCM.UserInterface.FastScrolling.ConsistentScrolling.Tooltip"),
					getValue: () => ModEntry.Config.UserInterfaceFastScrollingConsistentScrolling,
					setValue: value => ModEntry.Config.UserInterfaceFastScrollingConsistentScrolling = value
				);
				// User Interface - Zoom
				gmcm.AddPage(
					mod: ModEntry.ModManifest,
					pageId: "User Interface - Zoom",
					pageTitle: () => ModEntry.Helper.Translation.Get("GMCM.UserInterface.Zoom.Title")
				);
				gmcm.AddPageLink(
					mod: ModEntry.ModManifest,
					pageId: "User Interface",
					text: () => "@ " + ModEntry.Helper.Translation.Get("GMCM.UserInterface.BackTo")
				);
				gmcm.AddParagraph(
					mod: ModEntry.ModManifest,
					text: () => ModEntry.Helper.Translation.Get("GMCM.UserInterface.Zoom.Description")
				);
				gmcm.AddBoolOption(
					mod: ModEntry.ModManifest,
					name: () => ModEntry.Helper.Translation.Get("GMCM.Modules.Enabled.Title"),
					tooltip: () => ModEntry.Helper.Translation.Get("GMCM.Modules.Enabled.Tooltip"),
					getValue: () => ModEntry.Config.UserInterfaceZoom,
					setValue: value => ModEntry.Config.UserInterfaceZoom = value
				);
				gmcm.AddKeybind(
					mod: ModEntry.ModManifest,
					name: () => ModEntry.Helper.Translation.Get("GMCM.UserInterface.Zoom.SecondaryZoomInKey.Title"),
					tooltip: () => ModEntry.Helper.Translation.Get("GMCM.UserInterface.Zoom.SecondaryZoomInKey.Tooltip"),
					getValue: () => ModEntry.Config.UserInterfaceZoomSecondaryZoomInKey,
					setValue: value => ModEntry.Config.UserInterfaceZoomSecondaryZoomInKey = value
				);
				gmcm.AddKeybind(
					mod: ModEntry.ModManifest,
					name: () => ModEntry.Helper.Translation.Get("GMCM.UserInterface.Zoom.SecondaryZoomOutKey.Title"),
					tooltip: () => ModEntry.Helper.Translation.Get("GMCM.UserInterface.Zoom.SecondaryZoomOutKey.Tooltip"),
					getValue: () => ModEntry.Config.UserInterfaceZoomSecondaryZoomOutKey,
					setValue: value => ModEntry.Config.UserInterfaceZoomSecondaryZoomOutKey = value
				);
				gmcm.AddNumberOption(
					mod: ModEntry.ModManifest,
					name: () => ModEntry.Helper.Translation.Get("GMCM.UserInterface.Zoom.ZoomSpeedMultiplier.Title"),
					tooltip: () => ModEntry.Helper.Translation.Get("GMCM.UserInterface.Zoom.ZoomSpeedMultiplier.Tooltip"),
					getValue: () => ModEntry.Config.UserInterfaceZoomZoomSpeedMultiplier,
					setValue: value => ModEntry.Config.UserInterfaceZoomZoomSpeedMultiplier = value,
					min: 0.25f,
					max: 4.0f,
					interval: 0.25f
				);
				gmcm.AddNumberOption(
					mod: ModEntry.ModManifest,
					name: () => ModEntry.Helper.Translation.Get("GMCM.UserInterface.Zoom.MinimumZoomLevel.Title"),
					tooltip: () => ModEntry.Helper.Translation.Get("GMCM.UserInterface.Zoom.MinimumZoomLevel.Tooltip"),
					getValue: () => ModEntry.Config.UserInterfaceZoomMinimumZoomLevel,
					setValue: value => ModEntry.Config.UserInterfaceZoomMinimumZoomLevel = value,
					min: 0.0f,
					max: 0.75f,
					interval: 0.05f
				);
			}

			// Other
			gmcm.AddPage(
				mod: ModEntry.ModManifest,
				pageId: "Other",
				pageTitle: () => ModEntry.Helper.Translation.Get("GMCM.Other.Title")
			);
			// gmcm.AddPageLink(
			// 	mod: ModEntry.ModManifest,
			// 	pageId: ModEntry.ModManifest.UniqueID,
			// 	text: () => "@ " + "Back to QOL Essentials"
			// );
			gmcm.AddParagraph(
				mod: ModEntry.ModManifest,
				text: () => ModEntry.Helper.Translation.Get("GMCM.Other.Description")
			);
			gmcm.AddPageLink(
				mod: ModEntry.ModManifest,
				pageId: "Other - Festival end time",
				text: () => "> " + ModEntry.Helper.Translation.Get("GMCM.Other.FestivalEndTime.Title"),
				tooltip: () => ModEntry.Helper.Translation.Get("GMCM.Other.FestivalEndTime.Tooltip")
			);
			gmcm.AddPageLink(
				mod: ModEntry.ModManifest,
				pageId: "Other - Better porch repair",
				text: () => "> " + ModEntry.Helper.Translation.Get("GMCM.Other.BetterPorchRepair.Title"),
				tooltip: () => ModEntry.Helper.Translation.Get("GMCM.Other.BetterPorchRepair.Tooltip")
			);
			// Other - Better porch repair
			gmcm.AddPage(
				mod: ModEntry.ModManifest,
				pageId: "Other - Better porch repair",
				pageTitle: () => ModEntry.Helper.Translation.Get("GMCM.Other.BetterPorchRepair.Title")
			);
			gmcm.AddPageLink(
				mod: ModEntry.ModManifest,
				pageId: "Other",
				text: () => "@ " + ModEntry.Helper.Translation.Get("GMCM.Other.BackTo")
			);
			gmcm.AddParagraph(
				mod: ModEntry.ModManifest,
				text: () => ModEntry.Helper.Translation.Get("GMCM.Other.BetterPorchRepair.Description")
			);
			gmcm.AddBoolOption(
				mod: ModEntry.ModManifest,
				name: () => ModEntry.Helper.Translation.Get("GMCM.Modules.Enabled.Title"),
				tooltip: () => ModEntry.Helper.Translation.Get("GMCM.Modules.Enabled.Tooltip"),
				getValue: () => ModEntry.Config.OtherBetterPorchRepair,
				setValue: value => {
					ModEntry.Config.OtherBetterPorchRepair = value;
					BetterPorchRepairUtility.InvalidateCache();
				}
			);
			// Other - Festival end time
			gmcm.AddPage(
				mod: ModEntry.ModManifest,
				pageId: "Other - Festival end time",
				pageTitle: () => ModEntry.Helper.Translation.Get("GMCM.Other.FestivalEndTime.Title")
			);
			gmcm.AddPageLink(
				mod: ModEntry.ModManifest,
				pageId: "Other",
				text: () => "@ " + ModEntry.Helper.Translation.Get("GMCM.Other.BackTo")
			);
			gmcm.AddParagraph(
				mod: ModEntry.ModManifest,
				text: () => ModEntry.Helper.Translation.Get("GMCM.Other.FestivalEndTime.Description")
			);
			gmcm.AddParagraph(
				mod: ModEntry.ModManifest,
				text: () => ModEntry.Helper.Translation.Get("GMCM.Other.FestivalEndTime.AdditionalInformation")
			);
			gmcm.AddBoolOption(
				mod: ModEntry.ModManifest,
				name: () => ModEntry.Helper.Translation.Get("GMCM.Modules.Enabled.Title"),
				tooltip: () => ModEntry.Helper.Translation.Get("GMCM.Modules.Enabled.Tooltip"),
				getValue: () => ModEntry.Config.OtherFestivalEndTime,
				setValue: value => ModEntry.Config.OtherFestivalEndTime = value
			);
			gmcm.AddNumberOption(
				mod: ModEntry.ModManifest,
				name: () => ModEntry.Helper.Translation.Get("GMCM.Other.FestivalEndTime.AdditionalTime.Title"),
				tooltip: () => ModEntry.Helper.Translation.Get("GMCM.Other.FestivalEndTime.AdditionalTime.Tooltip"),
				getValue: () => ModEntry.Config.OtherFestivalEndTimeAdditionalTime / 100,
				setValue: value => ModEntry.Config.OtherFestivalEndTimeAdditionalTime = value * 100,
				min: 0,
				max: 8,
				interval: 1
			);
		}
	}
}
