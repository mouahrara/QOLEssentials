using System.Linq;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using mouahrarasModuleCollection.Shops.BetterAnimalPurchase.Patches;
using mouahrarasModuleCollection.Shops.BetterAnimalPurchase.Utilities;

namespace mouahrarasModuleCollection.Shops.BetterAnimalPurchase.Handlers
{
	internal static class ButtonPressedHandler
	{
		/// <inheritdoc cref="IInputEvents.ButtonPressed"/>
		/// <param name="sender">The event sender.</param>
		/// <param name="e">The event data.</param>
		internal static void Apply(object sender, ButtonPressedEventArgs e)
		{
			if (!ModEntry.Config.ShopsBetterAnimalPurchase || Game1.activeClickableMenu is not PurchaseAnimalsMenu purchaseAnimalsMenu || Game1.IsFading() || purchaseAnimalsMenu.freeze || !PurchaseAnimalsMenuPatch.AlternatePurchaseTypes.Any())
				return;

			if (purchaseAnimalsMenu.onFarm && !purchaseAnimalsMenu.namingAnimal)
			{
				if (e.Button == SButton.Left || e.Button == ModEntry.Config.ShopsBetterAnimalPurchasePreviousKey)
				{
					AlternatePurchaseTypesUtility.SelectPreviousVariant(purchaseAnimalsMenu);
				}
				else if (e.Button == SButton.Right || e.Button == ModEntry.Config.ShopsBetterAnimalPurchaseNextKey)
				{
					AlternatePurchaseTypesUtility.SelectNextVariant(purchaseAnimalsMenu);
				}
			}
		}
	}
}
