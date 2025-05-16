using HarmonyLib;
using StardewModdingAPI;
using mouahrarasModuleCollection.Modules;

namespace mouahrarasModuleCollection.Sections
{
	internal class UserInterfaceSection
	{
		internal static void Apply(Harmony harmony)
		{
			// Apply modules
			if (Constants.TargetPlatform != GamePlatform.Android)
			{
				FastScrollingModule.Apply(harmony);
				ZoomModule.Apply(harmony);
			}
		}
	}
}
