using HarmonyLib;
using mouahrarasModuleCollection.FarmView.SubModules;

namespace mouahrarasModuleCollection.Modules
{
	internal class FarmViewModule
	{
		internal static void Apply(Harmony harmony)
		{
			// Apply sub-modules
			FastScrollingSubModule.Apply(harmony);
			ZoomSubModule.Apply(harmony);
		}
	}
}
