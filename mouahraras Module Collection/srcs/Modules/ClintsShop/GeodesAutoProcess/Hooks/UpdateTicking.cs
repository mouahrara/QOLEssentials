using StardewModdingAPI.Events;
using mouahrarasModuleCollection.ClintsShop.GeodesAutoProcess.Utilities;

namespace mouahrarasModuleCollection.ClintsShop.GeodesAutoProcess.Hooks
{
	internal static class UpdateTickingHook
	{
		/// <inheritdoc cref="IGameLoopEvents.UpdateTicking"/>
		/// <param name="sender">The event sender.</param>
		/// <param name="e">The event data.</param>
		internal static void Apply(object sender, UpdateTickingEventArgs e)
		{
			if (!ModEntry.Config.ClintsShopGeodesAutoProcess)
				return;

			if (GeodesAutoProcessUtility.IsProcessing() && GeodesAutoProcessUtility.GetGeodeMenu().geodeAnimationTimer <= 0)
				GeodesAutoProcessUtility.CrackGeodeSecure();
		}
	}
}
