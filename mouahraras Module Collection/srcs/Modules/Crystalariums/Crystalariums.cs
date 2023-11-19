using HarmonyLib;
using mouahrarasModuleCollection.Crystalariums.SubModules;

namespace mouahrarasModuleCollection.Modules
{
	internal class CrystalariumsModule
	{
		internal static void Apply(Harmony harmony)
		{
			// Apply sub-modules
			SafeReplacementSubModule.Apply(harmony);
		}
	}
}
