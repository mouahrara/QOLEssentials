using System;
using StardewModdingAPI.Utilities;
using StardewValley;

namespace QOLEssentials.UserInterface.Zoom.Utilities
{
	internal class ZoomUtility
	{
		private static readonly PerScreen<float>	oldBaseZoomLevel = new(() => 1f);

		internal static float OldBaseZoomLevel
		{
			get => oldBaseZoomLevel.Value;
			set => oldBaseZoomLevel.Value = value;
		}

		internal static void Reset()
		{
			Game1.options.desiredBaseZoomLevel = OldBaseZoomLevel;
			Game1.options.baseZoomLevel = Game1.options.desiredBaseZoomLevel;
			Game1.forceSnapOnNextViewportUpdate = true;
			Game1.game1.refreshWindowSettings();
			RefreshViewportSize();
			OldBaseZoomLevel = 1f;
		}

		internal static void SetZoomLevel(float zoomLevel, bool saveCurrentZoomLevel)
		{
			if (saveCurrentZoomLevel)
			{
				OldBaseZoomLevel = Game1.options.baseZoomLevel;
			}
			Game1.options.desiredBaseZoomLevel = zoomLevel;
			Game1.options.baseZoomLevel = Game1.options.desiredBaseZoomLevel;
			Game1.game1.refreshWindowSettings();
		}

		internal static void AddZoomLevel(int direction)
		{
			float mapBasedMin = Game1.currentLocation is not null ? Math.Max((float)Game1.game1.localMultiplayerWindow.Width / Game1.currentLocation.Map.DisplayWidth, (float) Game1.game1.localMultiplayerWindow.Height / Game1.currentLocation.Map.DisplayHeight) / Game1.game1.zoomModifier : ModEntry.Config.UserInterfaceZoomMinimumZoomLevel;
			float min = Math.Max(ModEntry.Config.UserInterfaceZoomMinimumZoomLevel, mapBasedMin);
			float max = OldBaseZoomLevel;
			float newZoomLevel = Game1.options.baseZoomLevel - (-direction * ModEntry.Config.UserInterfaceZoomZoomSpeedMultiplier / 8001f);

			if (direction < 0 && newZoomLevel < min)
			{
				newZoomLevel = min;
			}
			if (direction > 0 && newZoomLevel > max)
			{
				newZoomLevel = max;
			}
			SetZoomLevel(newZoomLevel, false);
			RefreshViewportSize();
			Game1.clampViewportToGameMap();
		}

		internal static void RefreshViewportSize()
		{
			Game1.viewport.Width = (int)Math.Ceiling(Game1.game1.localMultiplayerWindow.Width / (Game1.options.baseZoomLevel * Game1.game1.zoomModifier));
			Game1.viewport.Height = (int)Math.Ceiling(Game1.game1.localMultiplayerWindow.Height / (Game1.options.baseZoomLevel * Game1.game1.zoomModifier));
		}
	}
}
