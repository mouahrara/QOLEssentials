using HarmonyLib;
using mouahrarasModuleCollection.Modules;

namespace mouahrarasModuleCollection.Sections
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
