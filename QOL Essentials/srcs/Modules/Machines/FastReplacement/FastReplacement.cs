using System;
using HarmonyLib;
using StardewModdingAPI;
using QOLEssentials.Machines.FastReplacement.Patches;

namespace QOLEssentials.Modules
{
	internal class FastReplacementModule
	{
		internal static void Apply(Harmony harmony)
		{
			try
			{
				ObjectPatch.Apply(harmony);
			}
			catch (Exception e)
			{
				ModEntry.Monitor.Log($"Issue with Harmony patching of the {typeof(FastReplacementModule)} module: {e}", LogLevel.Error);
				return;
			}
		}
	}
}
