using HarmonyLib;
using mouahrarasModuleCollection.ArcadeGames.SubModules;

namespace mouahrarasModuleCollection.Modules
{
	internal class ArcadeGamesModule
	{
		internal static void Apply(Harmony harmony)
		{
			// Apply sub-modules
			KonamiCodeSubModule.Apply(harmony);
			NotRealisticLeaderboardSubModule.Apply(harmony);
			PayToPlaySubModule.Apply(harmony);
		}
	}
}
