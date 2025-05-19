using System;
using HarmonyLib;
using StardewModdingAPI;
using QOLEssentials.Machines.SafeReplacement.Patches;

namespace QOLEssentials.Modules
{
	internal class SafeReplacementModule
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
				ModEntry.Monitor.Log($"Issue with Harmony patching of the {typeof(SafeReplacementModule)} module: {e}", LogLevel.Error);
				return;
			}
		}
	}
}
