using StardewModdingAPI.Events;
using mouahrarasModuleCollection.TweaksAndFeatures.UserInterface.Zoom.Utilities;

namespace mouahrarasModuleCollection.TweaksAndFeatures.UserInterface.Zoom.Handlers
{
	internal static class UpdateTickingHandler
	{
		/// <inheritdoc cref="IGameLoopEvents.UpdateTicking"/>
		/// <param name="sender">The event sender.</param>
		/// <param name="e">The event data.</param>
		internal static void Apply(object sender, UpdateTickingEventArgs e)
		{
			if (!ModEntry.Config.UserInterfaceZoom)
				return;
			if (ModEntry.Helper.Input.IsDown(ModEntry.Config.UserInterfaceZoomInKey))
			{
				ZoomUtility.AddZoomLevel(120);
				return;
			}
			if (ModEntry.Helper.Input.IsDown(ModEntry.Config.UserInterfaceZoomOutKey))
			{
				ZoomUtility.AddZoomLevel(-120);
				return;
			}
		}
	}
}
