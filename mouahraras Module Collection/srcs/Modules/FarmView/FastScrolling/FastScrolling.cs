using System;
using HarmonyLib;
using StardewModdingAPI;
using mouahrarasModuleCollection.FarmView.FastScrolling.Patches;

namespace mouahrarasModuleCollection.FarmView.SubModules
{
	internal class FastScrollingSubModule
	{
		internal static void Apply(Harmony harmony)
		{
			// Load Harmony patches
			try
			{
				// Apply menus patches
				CarpenterMenuPatch.Apply(harmony);
				PurchaseAnimalsMenuPatch.Apply(harmony);
				AnimalQueryMenuPatch.Apply(harmony);
			}
			catch (Exception e)
			{
				ModEntry.Monitor.Log($"Issue with Harmony patching of the {typeof(FastScrollingSubModule)} module: {e}", LogLevel.Error);
				return;
			}
		}
	}
}
