using System;
using HarmonyLib;
using StardewModdingAPI;
using mouahrarasModuleCollection.Festivals.EndTime.Patches;

namespace mouahrarasModuleCollection.Festivals.SubModules
{
	internal class EndTimeSubModule
	{
		internal static void Apply(Harmony harmony)
		{
			// Load Harmony patches
			try
			{
				// Apply locations patches
				EventPatch.Apply(harmony);
			}
			catch (Exception e)
			{
				ModEntry.Monitor.Log($"Issue with Harmony patching of the {typeof(EndTimeSubModule)} module: {e}", LogLevel.Error);
				return;
			}
		}
	}
}
