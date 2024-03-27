using HarmonyLib;
using mouahrarasModuleCollection.Modules;

namespace mouahrarasModuleCollection.SubSections
{
	internal class ShopsSubSection
	{
		internal static void Apply(Harmony harmony)
		{
			// Apply modules
			BetterAnimalPurchaseModule.Apply(harmony);
			GeodesAutoProcessModule.Apply(harmony);
		}
	}
}
