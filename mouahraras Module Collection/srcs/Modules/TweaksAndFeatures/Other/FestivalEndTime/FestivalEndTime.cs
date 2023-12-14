using System;
using HarmonyLib;
using StardewModdingAPI;
using mouahrarasModuleCollection.TweaksAndFeatures.Other.FestivalEndTime.Patches;

namespace mouahrarasModuleCollection.Modules
{
	internal class EndTimeModule
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
				ModEntry.Monitor.Log($"Issue with Harmony patching of the {typeof(EndTimeModule)} module: {e}", LogLevel.Error);
				return;
			}
		}
	}
}
