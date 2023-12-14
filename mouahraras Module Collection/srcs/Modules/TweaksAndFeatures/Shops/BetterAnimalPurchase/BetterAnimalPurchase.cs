using System;
using HarmonyLib;
using StardewModdingAPI;
using mouahrarasModuleCollection.TweaksAndFeatures.Shops.BetterAnimalPurchase.Patches;

namespace mouahrarasModuleCollection.Modules
{
	internal class AnimalPurchaseModule
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
				ModEntry.Monitor.Log($"Issue with Harmony patching of the {typeof(AnimalPurchaseModule)} module: {e}", LogLevel.Error);
				return;
			}
		}
	}
}
