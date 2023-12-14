using HarmonyLib;
using mouahrarasModuleCollection.Modules;

namespace mouahrarasModuleCollection.SubSections
{
	internal class UserInterfaceSubSection
	{
		internal static void Apply(Harmony harmony)
		{
			// Apply modules
			FastScrollingModule.Apply(harmony);
			ZoomModule.Apply(harmony);
		}
	}
}
