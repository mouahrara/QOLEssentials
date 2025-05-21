using System.Collections.Generic;
using System.Linq;
using StardewValley;
using StardewValley.BellsAndWhistles;
using StardewValley.GameData.FarmAnimals;
using StardewValley.Menus;
using QOLEssentials.Shops.BetterAnimalPurchase.Patches;

namespace QOLEssentials.Shops.BetterAnimalPurchase.Utilities
{
	internal class AlternatePurchaseTypesUtility
	{
		internal static void SetAlternatePurchaseTypes(string key)
		{
			PurchaseAnimalsMenuPatch.AlternatePurchaseTypes.Clear();
			PurchaseAnimalsMenuPatch.Randomize = true;
			if (Game1.farmAnimalData.TryGetValue(key, out FarmAnimalData value) && value.AlternatePurchaseTypes is not null)
			{
				foreach (AlternatePurchaseAnimals alternatePurchaseType in value.AlternatePurchaseTypes)
				{
					if (GameStateQuery.CheckConditions(alternatePurchaseType.Condition, null, null, null, null, null, new HashSet<string> { "RANDOM" }))
					{
						PurchaseAnimalsMenuPatch.AlternatePurchaseTypes.AddRange(alternatePurchaseType.AnimalIds);
					}
				}
			}
		}

		internal static void SelectPreviousVariant(PurchaseAnimalsMenu purchaseAnimalsMenu)
		{
			int index = PurchaseAnimalsMenuPatch.AlternatePurchaseTypes.FindIndex(type => type == purchaseAnimalsMenu.animalBeingPurchased.type.Value);

			if (index >= 0)
			{
				SetVariant(purchaseAnimalsMenu, index - 1);
			}
		}

		internal static void SelectNextVariant(PurchaseAnimalsMenu purchaseAnimalsMenu)
		{
			int index = PurchaseAnimalsMenuPatch.AlternatePurchaseTypes.FindIndex(type => type == purchaseAnimalsMenu.animalBeingPurchased.type.Value);

			if (index >= 0)
			{
				SetVariant(purchaseAnimalsMenu, index + 1);
			}
		}

		private static void SetVariant(PurchaseAnimalsMenu purchaseAnimalsMenu, int index)
		{
			if (PurchaseAnimalsMenuPatch.AlternatePurchaseTypes.Any())
			{
				index = (index + PurchaseAnimalsMenuPatch.AlternatePurchaseTypes.Count) % PurchaseAnimalsMenuPatch.AlternatePurchaseTypes.Count;
				purchaseAnimalsMenu.animalBeingPurchased = new FarmAnimal(PurchaseAnimalsMenuPatch.AlternatePurchaseTypes[index], purchaseAnimalsMenu.animalBeingPurchased.myID.Value, purchaseAnimalsMenu.animalBeingPurchased.ownerID.Value);
				PurchaseAnimalsMenuPatch.Randomize = false;
				Game1.playSound("shwip");
				SetVariantButtonBounds(purchaseAnimalsMenu.animalBeingPurchased);
			}
		}

		internal static void SetVariantButtonBounds(FarmAnimal farmAnimal = null)
		{
			farmAnimal ??= Game1.activeClickableMenu is PurchaseAnimalsMenu purchaseAnimalsMenu ? purchaseAnimalsMenu.animalBeingPurchased : null;
			if (farmAnimal is not null && PurchaseAnimalsMenuPatch.PreviousVariantButton is not null && PurchaseAnimalsMenuPatch.NextVariantButton is not null)
			{
				string s = Game1.content.LoadString("Strings\\StringsFromCSFiles:PurchaseAnimalsMenu.cs.11355", farmAnimal.displayHouse, farmAnimal.displayType);

				PurchaseAnimalsMenuPatch.PreviousVariantButton.bounds.X = Game1.uiViewport.Width / 2 - SpriteText.getWidthOfString(s) / 2 - 115;
				PurchaseAnimalsMenuPatch.PreviousVariantButton.bounds.Y = 12;
				PurchaseAnimalsMenuPatch.NextVariantButton.bounds.X = Game1.uiViewport.Width / 2 + SpriteText.getWidthOfString(s) / 2 + 43;
				PurchaseAnimalsMenuPatch.NextVariantButton.bounds.Y = 12;
			}
		}
	}
}
