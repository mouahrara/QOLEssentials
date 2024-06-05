using HarmonyLib;
using mouahrarasModuleCollection.Modules;

namespace mouahrarasModuleCollection.Sections
{
	internal class ArcadeGamesSection
	{
		internal static void Apply(Harmony harmony)
		{
			// Apply modules
			KonamiCodeModule.Apply(harmony);
			NonRealisticLeaderboardModule.Apply(harmony);
			PayToPlayModule.Apply(harmony);
		}
	}
}
