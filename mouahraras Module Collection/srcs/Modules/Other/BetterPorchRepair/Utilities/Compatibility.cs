namespace mouahrarasModuleCollection.Other.BetterPorchRepair.Utilities
{
	internal class CompatibilityUtility
	{
		internal const string flippedKey = "mouahrara.FlipBuildings_Flipped";
		internal static readonly bool IsFlipBuildingsLoaded = ModEntry.Helper.ModRegistry.IsLoaded("mouahrara.FlipBuildings");
	}
}
