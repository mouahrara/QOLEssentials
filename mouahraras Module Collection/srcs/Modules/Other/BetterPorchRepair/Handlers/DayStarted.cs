using StardewModdingAPI.Events;
using mouahrarasModuleCollection.Other.BetterPorchRepair.Utilities;

namespace mouahrarasModuleCollection.Other.BetterPorchRepair.Handlers
{
	internal static class DayStartedHandler
	{
		/// <inheritdoc cref="IGameLoopEvents.DayStarted"/>
		/// <param name="sender">The event sender.</param>
		/// <param name="e">The event data.</param>
		internal static void Apply(object sender, DayStartedEventArgs e)
		{
			if (!ModEntry.Config.OtherBetterPorchRepair)
				return;

			BetterPorchRepairUtility.InvalidateCache();
		}
	}
}
