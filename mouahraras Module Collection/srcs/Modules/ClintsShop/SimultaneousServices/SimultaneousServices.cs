using System;
using HarmonyLib;
using StardewModdingAPI;
using mouahrarasModuleCollection.ClintsShop.SimultaneousServices.Patches;

namespace mouahrarasModuleCollection.ClintsShop.SubModules
{
	internal class SimultaneousServicesSubModule
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
				ModEntry.Monitor.Log($"Issue with Harmony patching of the {typeof(SimultaneousServicesSubModule)} module: {e}", LogLevel.Error);
				return;
			}
		}
	}
}
