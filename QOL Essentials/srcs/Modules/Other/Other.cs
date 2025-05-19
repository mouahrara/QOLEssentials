using HarmonyLib;
using QOLEssentials.Modules;

namespace QOLEssentials.Sections
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
