using System;
using HarmonyLib;
using StardewModdingAPI;
using QOLEssentials.Machines.ResourceRecovery.Patches;

namespace QOLEssentials.Modules
{
	internal class ResourceRecoveryModule
	{
		internal static void Apply(Harmony harmony)
		{
			try
			{
				ObjectPatch.Apply(harmony);
			}
			catch (Exception e)
			{
				ModEntry.Monitor.Log($"Issue with Harmony patching of the {typeof(ResourceRecoveryModule)} module: {e}", LogLevel.Error);
				return;
			}
		}
	}
}
