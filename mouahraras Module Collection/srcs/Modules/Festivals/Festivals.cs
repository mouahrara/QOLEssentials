using HarmonyLib;
using mouahrarasModuleCollection.Festivals.SubModules;

namespace mouahrarasModuleCollection.Modules
{
	internal class FestivalsModule
	{
		internal static void Apply(Harmony harmony)
		{
			// Apply sub-modules
			EndTimeSubModule.Apply(harmony);
		}
	}
}
