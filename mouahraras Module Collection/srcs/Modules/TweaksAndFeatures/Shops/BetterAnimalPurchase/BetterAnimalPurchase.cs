using System;
using HarmonyLib;
using StardewModdingAPI;
using mouahrarasModuleCollection.TweaksAndFeatures.Shops.BetterAnimalPurchase.Patches;

namespace mouahrarasModuleCollection.Modules
{
	internal class BetterAnimalPurchaseModule
	{
		internal static void Apply(Harmony harmony)
		{
			// Load Harmony patches
			try
			{
				// Apply menus patches
				PurchaseAnimalsMenuPatch.Apply(harmony);
			}
			catch (Exception e)
			{
				ModEntry.Monitor.Log($"Issue with Harmony patching of the {typeof(BetterAnimalPurchaseModule)} module: {e}", LogLevel.Error);
				return;
			}
		}
	}
}
