using HarmonyLib;
using StardewValley.Menus;
using mouahrarasModuleCollection.TweaksAndFeatures.UserInterface.Zoom.Utilities;

namespace mouahrarasModuleCollection.TweaksAndFeatures.UserInterface.Zoom.Patches
{
	internal class PurchaseAnimalsMenuPatch
	{
		internal static void Apply(Harmony harmony)
		{
			harmony.Patch(
				original: AccessTools.Method(typeof(PurchaseAnimalsMenu), nameof(PurchaseAnimalsMenu.setUpForAnimalPlacement)),
				postfix: new HarmonyMethod(typeof(MenusPatchUtility), nameof(MenusPatchUtility.EnterFarmViewPostfix))
			);
			harmony.Patch(
				original: AccessTools.Method(typeof(PurchaseAnimalsMenu), nameof(PurchaseAnimalsMenu.setUpForReturnToShopMenu)),
				postfix: new HarmonyMethod(typeof(MenusPatchUtility), nameof(MenusPatchUtility.LeaveFarmViewPostfix))
			);
			harmony.Patch(
				original: AccessTools.Method(typeof(PurchaseAnimalsMenu), nameof(PurchaseAnimalsMenu.setUpForReturnAfterPurchasingAnimal)),
				postfix: new HarmonyMethod(typeof(PurchaseAnimalsMenuPatch), nameof(SetUpForReturnAfterPurchasingAnimalPostfix))
			);
		}

		private static void SetUpForReturnAfterPurchasingAnimalPostfix()
		{
			if (ModEntry.Config.ShopsBetterAnimalPurchase)
				return;

			MenusPatchUtility.LeaveFarmViewPostfix();
		}
	}
}
