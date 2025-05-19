using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using QOLEssentials.UserInterface.Zoom.Utilities;

namespace QOLEssentials.UserInterface.Zoom.Handlers
{
	internal static class UpdateTickingHandler
	{
		/// <inheritdoc cref="IGameLoopEvents.UpdateTicking"/>
		/// <param name="sender">The event sender.</param>
		/// <param name="e">The event data.</param>
		internal static void Apply(object sender, UpdateTickingEventArgs e)
		{
			if (MenusPatchUtility.ShouldProcess(Game1.activeClickableMenu))
			{
				bool isZoomInKeyDown = ModEntry.Helper.Input.IsDown(ModEntry.Config.UserInterfaceZoomInKey);
				bool isZoomOutKeyDown = ModEntry.Helper.Input.IsDown(ModEntry.Config.UserInterfaceZoomOutKey);

				if (!isZoomInKeyDown || !isZoomOutKeyDown)
				{
					if (isZoomInKeyDown)
					{
						ZoomUtility.AddZoomLevel(120);
					}
					else if (isZoomOutKeyDown)
					{
						ZoomUtility.AddZoomLevel(-120);
					}
				}
			}
		}
	}
}
