using HarmonyLib;
using mouahrarasModuleCollection.Modules;

namespace mouahrarasModuleCollection.Sections
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
