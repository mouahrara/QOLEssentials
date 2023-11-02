using StardewModdingAPI.Events;
using mouahrarasModuleCollection.ArcadeGames.PayToPlay.Managers;

namespace mouahrarasModuleCollection.ArcadeGames.PayToPlay.Hooks
{
	internal static class SaveLoadedHook
	{
		/// <inheritdoc cref="IGameLoopEvents.SaveLoaded"/>
		/// <param name="sender">The event sender.</param>
		/// <param name="e">The event data.</param>
		internal static void Apply(object sender, SaveLoadedEventArgs e)
		{
			// Load assets
			AssetManager.Apply();
		}
	}
}
