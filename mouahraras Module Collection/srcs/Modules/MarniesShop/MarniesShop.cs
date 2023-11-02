using HarmonyLib;
using mouahrarasModuleCollection.MarniesShop.SubModules;

namespace mouahrarasModuleCollection.Modules
{
	internal class MarniesShopModule
	{
		internal static void Apply(Harmony harmony)
		{
			// Apply sub-modules
			AnimalPurchaseSubModule.Apply(harmony);
		}
	}
}
