using HarmonyLib;
using QOLEssentials.Modules;

namespace QOLEssentials.Sections
{
	internal class ShopsSection
	{
		internal static void Apply(Harmony harmony)
		{
			// Apply modules
			BetterAnimalPurchaseModule.Apply(harmony);
			GeodesAutoProcessModule.Apply(harmony);
		}
	}
}
