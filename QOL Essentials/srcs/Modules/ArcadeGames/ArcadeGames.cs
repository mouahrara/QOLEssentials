using HarmonyLib;
using StardewModdingAPI;
using QOLEssentials.Modules;

namespace QOLEssentials.Sections
{
	internal class ArcadeGamesSection
	{
		internal static void Apply(Harmony harmony)
		{
			// Apply modules
			if (Constants.TargetPlatform != GamePlatform.Android)
			{
				FullScreenModule.Apply(harmony);
				KonamiCodeModule.Apply(harmony);
			}
			NonRealisticLeaderboardModule.Apply(harmony);
			PayToPlayModule.Apply(harmony);
		}
	}
}
