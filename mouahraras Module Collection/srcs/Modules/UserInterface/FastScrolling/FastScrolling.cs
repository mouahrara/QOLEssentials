using System;
using HarmonyLib;
using StardewModdingAPI;
using mouahrarasModuleCollection.UserInterface.FastScrolling.Patches;

namespace mouahrarasModuleCollection.Modules
{
	internal class FastScrollingModule
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
				ModEntry.Monitor.Log($"Issue with Harmony patching of the {typeof(FastScrollingModule)} module: {e}", LogLevel.Error);
				return;
			}
		}
	}
}
