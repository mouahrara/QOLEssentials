using StardewModdingAPI.Events;
using mouahrarasModuleCollection.Other.BetterPorchRepair.Utilities;

namespace mouahrarasModuleCollection.Other.BetterPorchRepair.Handlers
{
	internal static class AssetRequestedHandler
	{
		/// <inheritdoc cref="IGameLoopEvents.AssetRequested"/>
		/// <param name="sender">The event sender.</param>
		/// <param name="e">The event data.</param>
		internal static void Apply(object sender, AssetRequestedEventArgs e)
		{
			// Load assets
			BetterPorchRepairUtility.RepairPorch(e);
		}
	}
}
