using HarmonyLib;
using QOLEssentials.Modules;

namespace QOLEssentials.Sections
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
