using HarmonyLib;
using mouahrarasModuleCollection.Modules;

namespace mouahrarasModuleCollection.Sections
{
	internal class OtherSection
	{
		internal static void Apply(Harmony harmony)
		{
			// Apply modules
			BetterPorchRepairModule.Apply(harmony);
			EndTimeModule.Apply(harmony);
		}
	}
}
