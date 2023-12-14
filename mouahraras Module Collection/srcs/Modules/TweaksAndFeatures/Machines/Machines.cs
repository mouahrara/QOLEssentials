using HarmonyLib;
using mouahrarasModuleCollection.Modules;

namespace mouahrarasModuleCollection.SubSections
{
	internal class MachinesSubSection
	{
		internal static void Apply(Harmony harmony)
		{
			// Apply modules
			SafeReplacementModule.Apply(harmony);
		}
	}
}
