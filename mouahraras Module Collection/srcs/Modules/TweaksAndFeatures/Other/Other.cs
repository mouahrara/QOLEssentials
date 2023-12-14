using HarmonyLib;
using mouahrarasModuleCollection.Modules;

namespace mouahrarasModuleCollection.SubSections
{
	internal class OtherSubSection
	{
		internal static void Apply(Harmony harmony)
		{
			// Apply modules
			EndTimeModule.Apply(harmony);
		}
	}
}
