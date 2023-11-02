using System;
using HarmonyLib;
using StardewModdingAPI;
using mouahrarasModuleCollection.MarniesShop.AnimalPurchase.Patches;

namespace mouahrarasModuleCollection.MarniesShop.SubModules
{
	internal class AnimalPurchaseSubModule
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
				ModEntry.Monitor.Log($"Issue with Harmony patching of the {typeof(AnimalPurchaseSubModule)} module: {e}", LogLevel.Error);
				return;
			}
		}
	}
}
