using HarmonyLib;
using mouahrarasModuleCollection.ClintsShop.SubModules;

namespace mouahrarasModuleCollection.Modules
{
	internal class ClintsShopModule
	{
		internal static void Apply(Harmony harmony)
		{
			// Apply sub-modules
			GeodesAutoProcessSubModule.Apply(harmony);
			SimultaneousServicesSubModule.Apply(harmony);
		}
	}
}
