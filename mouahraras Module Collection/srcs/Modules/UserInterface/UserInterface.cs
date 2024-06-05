using HarmonyLib;
using mouahrarasModuleCollection.Modules;

namespace mouahrarasModuleCollection.Sections
{
	internal class UserInterfaceSection
	{
		internal static void Apply(Harmony harmony)
		{
			// Apply modules
			FastScrollingModule.Apply(harmony);
			ZoomModule.Apply(harmony);
		}
	}
}
