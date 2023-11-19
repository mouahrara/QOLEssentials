using System;
using HarmonyLib;
using StardewModdingAPI;
using mouahrarasModuleCollection.Crystalariums.SafeReplacement.Patches;

namespace mouahrarasModuleCollection.Crystalariums.SubModules
{
	internal class SafeReplacementSubModule
	{
		internal static void Apply(Harmony harmony)
		{
			// Load Harmony patches
			try
			{
				// Apply objects patches
				ObjectPatch.Apply(harmony);

				// Apply locations patches
				GameLocationPatch.Apply(harmony);
			}
			catch (Exception e)
			{
				ModEntry.Monitor.Log($"Issue with Harmony patching of the {typeof(SafeReplacementSubModule)} module: {e}", LogLevel.Error);
				return;
			}
		}
	}
}
