using HarmonyLib;
using mouahrarasModuleCollection.Modules;

namespace mouahrarasModuleCollection.SubSections
{
	internal class ShopsSubSection
	{
		internal static void Apply(Harmony harmony)
		{
			// Apply modules
			AnimalPurchaseModule.Apply(harmony);
			GeodesAutoProcessModule.Apply(harmony);
			SimultaneousServicesModule.Apply(harmony);
		}
	}
}
