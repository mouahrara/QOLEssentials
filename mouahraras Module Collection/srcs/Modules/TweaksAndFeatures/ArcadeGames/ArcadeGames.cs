using HarmonyLib;
using mouahrarasModuleCollection.Modules;

namespace mouahrarasModuleCollection.SubSections
{
	internal class ArcadeGamesSubSection
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
