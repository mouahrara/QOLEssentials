using System;
using HarmonyLib;
using StardewModdingAPI;
using mouahrarasModuleCollection.TweaksAndFeatures.Shops.SimultaneousServices.Patches;

namespace mouahrarasModuleCollection.Modules
{
	internal class SimultaneousServicesModule
	{
		internal static void Apply(Harmony harmony)
		{
			// Load Harmony patches
			try
			{
				// Apply locations patches
				GameLocationPatch.Apply(harmony);
			}
			catch (Exception e)
			{
				ModEntry.Monitor.Log($"Issue with Harmony patching of the {typeof(SimultaneousServicesModule)} module: {e}", LogLevel.Error);
				return;
			}
		}
	}
}
