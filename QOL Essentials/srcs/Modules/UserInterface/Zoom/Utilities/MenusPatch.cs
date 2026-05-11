using System;
using StardewValley;
using StardewValley.Menus;
using QOLEssentials.UserInterface.Zoom.Handlers;

namespace QOLEssentials.UserInterface.Zoom.Utilities
{
	internal class MenusPatchUtility
	{
		internal static void EnterFarmViewPostfix()
		{
			if (!ModEntry.Config.UserInterfaceZoom)
				return;

			ZoomUtility.SetZoomLevel(Game1.options.baseZoomLevel, true);
			ModEntry.Helper.Events.GameLoop.UpdateTicking -= UpdateTickingHandler.Apply;
			ModEntry.Helper.Events.GameLoop.UpdateTicking += UpdateTickingHandler.Apply;
		}

		internal static void LeaveFarmViewPostfix()
		{
			if (!ModEntry.Config.UserInterfaceZoom)
				return;

			ModEntry.Helper.Events.GameLoop.UpdateTicking -= UpdateTickingHandler.Apply;
			if (Game1.isWarping && Game1.locationRequest is not null)
			{
				Game1.locationRequest.OnLoad += ZoomUtility.Reset;
			}
			else
			{
				ZoomUtility.Reset();
			}
		}

		internal static void GameWindowSizeChangedPostfix(IClickableMenu __instance)
		{
			if (!ModEntry.Config.UserInterfaceZoom || (__instance is not CarpenterMenu && __instance is not PurchaseAnimalsMenu && __instance is not AnimalQueryMenu) || !__instance.shouldClampGamePadCursor())
				return;

			float mapBasedMin = Game1.currentLocation is not null ? Math.Max((float)Game1.game1.localMultiplayerWindow.Width / Game1.currentLocation.Map.DisplayWidth, (float) Game1.game1.localMultiplayerWindow.Height / Game1.currentLocation.Map.DisplayHeight) / Game1.game1.zoomModifier : ModEntry.Config.UserInterfaceZoomMinimumZoomLevel;
			float min = Math.Max(ModEntry.Config.UserInterfaceZoomMinimumZoomLevel, mapBasedMin);

			if (Game1.options.baseZoomLevel < min)
			{
				ZoomUtility.SetZoomLevel(min, false);
				ZoomUtility.RefreshViewportSize();
			}
			Game1.clampViewportToGameMap();
		}

		internal static bool ShouldProcess(IClickableMenu __instance)
		{
			if (!ModEntry.Config.UserInterfaceZoom)
				return false;

			return (__instance is CarpenterMenu or PurchaseAnimalsMenu or AnimalQueryMenu) && ((__instance as CarpenterMenu)?.freeze != true) && ((__instance as PurchaseAnimalsMenu)?.freeze != true) && !Game1.IsFading() && __instance.shouldClampGamePadCursor() && __instance.overrideSnappyMenuCursorMovementBan();
		}
	}
}
