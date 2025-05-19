using System;
using HarmonyLib;
using StardewModdingAPI;
using QOLEssentials.UserInterface.Zoom.Patches;

namespace QOLEssentials.Modules
{
	internal class ZoomModule
	{
		internal static void Apply(Harmony harmony)
		{
			// Load Harmony patches
			try
			{
				// Apply menus patches
				IClickableMenuPatch.Apply(harmony);
				CarpenterMenuPatch.Apply(harmony);
				PurchaseAnimalsMenuPatch.Apply(harmony);
				AnimalQueryMenuPatch.Apply(harmony);

				// Apply options patches
				OptionsPatch.Apply(harmony);
			}
			catch (Exception e)
			{
				ModEntry.Monitor.Log($"Issue with Harmony patching of the {typeof(ZoomModule)} module: {e}", LogLevel.Error);
				return;
			}
		}
	}
}
