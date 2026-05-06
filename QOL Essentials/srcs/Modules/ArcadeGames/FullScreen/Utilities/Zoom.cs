using StardewModdingAPI.Utilities;
using StardewValley;

namespace QOLEssentials.ArcadeGames.FullScreen.Utilities
{
	internal class ZoomUtility
	{
		private static readonly PerScreen<float>	oldBaseZoomLevel = new(() => 1f);

		private static float OldBaseZoomLevel
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
			Game1.forceSnapOnNextViewportUpdate = true;
			Game1.game1.refreshWindowSettings();
		}
	}
}
