using System;
using HarmonyLib;
using StardewModdingAPI;
using QOLEssentials.UserInterface.FastScrolling.Patches;

namespace QOLEssentials.Modules
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
