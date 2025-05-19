using HarmonyLib;
using QOLEssentials.Modules;

namespace QOLEssentials.Sections
{
	internal class MachinesSection
	{
		internal static void Apply(Harmony harmony)
		{
			// Apply modules
			SafeReplacementModule.Apply(harmony);
		}
	}
}
