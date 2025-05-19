using HarmonyLib;
using StardewModdingAPI;
using StardewValley.Menus;
using QOLEssentials.Shops.BetterAnimalPurchase.Utilities;

namespace QOLEssentials.Shops.BetterAnimalPurchase.Patches
{
	internal class IClickableMenuPatch
	{
		internal static void Apply(Harmony harmony)
		{
			harmony.Patch(
				original: AccessTools.Method(typeof(IClickableMenu), nameof(IClickableMenu.populateClickableComponentList)),
				postfix: new HarmonyMethod(typeof(IClickableMenuPatch), nameof(PopulateClickableComponentListPostfix))
			);
			harmony.Patch(
				original: AccessTools.Method(typeof(IClickableMenu), nameof(IClickableMenu.gameWindowSizeChanged)),
				postfix: new HarmonyMethod(typeof(IClickableMenuPatch), nameof(GameWindowSizeChangedPostfix))
			);
		}

		private static void PopulateClickableComponentListPostfix(IClickableMenu __instance)
		{
			if (!Context.IsWorldReady || !ModEntry.Config.ShopsBetterAnimalPurchase)
				return;

			if (__instance is PurchaseAnimalsMenu)
			{
				__instance.allClickableComponents.Add(PurchaseAnimalsMenuPatch.PreviousVariantButton);
				__instance.allClickableComponents.Add(PurchaseAnimalsMenuPatch.NextVariantButton);
			}
		}

		private static void GameWindowSizeChangedPostfix()
		{
			AlternatePurchaseTypesUtility.SetVariantButtonBounds();
		}
	}
}
