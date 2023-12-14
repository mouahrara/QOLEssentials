using HarmonyLib;
using mouahrarasModuleCollection.SubSections;

namespace mouahrarasModuleCollection.Sections
{
	internal class TweaksAndFeaturesSection
	{
		internal static void Apply(Harmony harmony)
		{
			// Apply sub-sections
			ArcadeGamesSubSection.Apply(harmony);
			MachinesSubSection.Apply(harmony);
			ShopsSubSection.Apply(harmony);
			UserInterfaceSubSection.Apply(harmony);
			OtherSubSection.Apply(harmony);
		}
	}
}
