using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HarmonyLib;
using StardewModdingAPI;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Buildings;
using StardewValley.Extensions;
using StardewValley.Menus;
using QOLEssentials.Shops.BetterAnimalPurchase.Utilities;
using QOLEssentials.Utilities;
using Object = StardewValley.Object;

namespace QOLEssentials.Shops.BetterAnimalPurchase.Patches
{
	internal class PurchaseAnimalsMenuPatch
	{
		private static readonly PerScreen<ClickableTextureComponent>	previousVariantButton = new(() => null);
		private static readonly PerScreen<ClickableTextureComponent>	nextVariantButton = new(() => null);
		private static readonly PerScreen<List<string>>					alternatePurchaseTypes = new(() => new());
		private static readonly PerScreen<bool>							randomize = new(() => true);

		internal static ClickableTextureComponent PreviousVariantButton
		{
			get => previousVariantButton.Value;
			set => previousVariantButton.Value = value;
		}

		internal static ClickableTextureComponent NextVariantButton
		{
			get => nextVariantButton.Value;
			set => nextVariantButton.Value = value;
		}

		internal static List<string> AlternatePurchaseTypes
		{
			get => alternatePurchaseTypes.Value;
			set => alternatePurchaseTypes.Value = value;
		}

		internal static bool Randomize
		{
			get => randomize.Value;
			set => randomize.Value = value;
		}

		internal static void Apply(Harmony harmony)
		{
			if (Constants.TargetPlatform == GamePlatform.Android)
			{
				harmony.Patch(
					original: AccessTools.Method(typeof(PurchaseAnimalsMenu), "PlaceAnimalInSelectedBuilding"),
					prefix: new HarmonyMethod(typeof(PurchaseAnimalsMenuPatch), nameof(PlaceAnimalInSelectedBuildingPrefix))
				);
				harmony.Patch(
					original: AccessTools.Method(typeof(PurchaseAnimalsMenu), "selectItem", new Type[] { typeof(int) }),
					postfix: new HarmonyMethod(typeof(PurchaseAnimalsMenuPatch), nameof(SelectItemPostfix))
				);
				harmony.Patch(
					original: AccessTools.Method(typeof(PurchaseAnimalsMenu), "OnClickBuyAnimal"),
					postfix: new HarmonyMethod(typeof(PurchaseAnimalsMenuPatch), nameof(OnClickBuyAnimalPostfix))
				);
			}
			else
			{
				harmony.Patch(
					original: AccessTools.Method(typeof(PurchaseAnimalsMenu), nameof(PurchaseAnimalsMenu.receiveLeftClick), new Type[] { typeof(int), typeof(int), typeof(bool) }),
					prefix: new HarmonyMethod(typeof(PurchaseAnimalsMenuPatch), nameof(ReceiveLeftClickPrefix)),
					postfix: new HarmonyMethod(typeof(PurchaseAnimalsMenuPatch), nameof(ReceiveLeftClickPostfix))
				);
			}
			harmony.Patch(
				original: AccessTools.Constructor(typeof(PurchaseAnimalsMenu), new Type[] { typeof(List<Object>), typeof(GameLocation) }),
				postfix: new HarmonyMethod(typeof(PurchaseAnimalsMenuPatch), nameof(PurchaseAnimalsMenuPostfix))
			);
			harmony.Patch(
				original: AccessTools.Method(typeof(PurchaseAnimalsMenu), nameof(PurchaseAnimalsMenu.setUpForReturnAfterPurchasingAnimal)),
				prefix: new HarmonyMethod(typeof(PurchaseAnimalsMenuPatch), nameof(SetUpForReturnAfterPurchasingAnimalPrefix))
			);
			harmony.Patch(
				original: AccessTools.Method(typeof(PurchaseAnimalsMenu), nameof(PurchaseAnimalsMenu.performHoverAction)),
				postfix: new HarmonyMethod(typeof(PurchaseAnimalsMenuPatch), nameof(PerformHoverActionPostfix))
			);
			harmony.Patch(
				original: AccessTools.Method(typeof(PurchaseAnimalsMenu), nameof(PurchaseAnimalsMenu.receiveLeftClick), new Type[] { typeof(int), typeof(int), typeof(bool) }),
				postfix: new HarmonyMethod(typeof(PurchaseAnimalsMenuPatch), nameof(ReceiveLeftClickPostfixVariantButtons))
			);
			harmony.Patch(
				original: AccessTools.Method(typeof(PurchaseAnimalsMenu), nameof(PurchaseAnimalsMenu.draw), new Type[] { typeof(SpriteBatch) }),
				prefix: new HarmonyMethod(typeof(PurchaseAnimalsMenuPatch), nameof(DrawPrefix)),
				postfix: new HarmonyMethod(typeof(PurchaseAnimalsMenuPatch), nameof(DrawPostfix))
			);
		}

		private static bool PlaceAnimalInSelectedBuildingPrefix(PurchaseAnimalsMenu __instance)
		{
			if (!ModEntry.Config.ShopsBetterAnimalPurchase)
				return true;

			if (Game1.player.Money < __instance.priceOfAnimal)
			{
				Game1.dayTimeMoneyBox.moneyShakeTimer = 1000;
				Game1.playSound("cancel");
				return false;
			}
			return true;
		}

		private static void SelectItemPostfix(PurchaseAnimalsMenu __instance, int i)
		{
			if (!ModEntry.Config.ShopsBetterAnimalPurchase)
				return;

			List<ClickableComponent> itemBox = (List<ClickableComponent>)typeof(PurchaseAnimalsMenu).GetField("itemBox", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(__instance);

			if (itemBox[i] is not null)
			{
				if ((itemBox[i].item as Object).Type is null && Game1.player.Money >= itemBox[i].item.salePrice())
				{
					AlternatePurchaseTypesUtility.SetAlternatePurchaseTypes(itemBox[i].item.Name);
				}
			}
		}

		private static void OnClickBuyAnimalPostfix(PurchaseAnimalsMenu __instance)
		{
			if (!ModEntry.Config.ShopsBetterAnimalPurchase)
				return;

			AlternatePurchaseTypesUtility.SetVariantButtonBounds(__instance.animalBeingPurchased);
		}

		private static void PurchaseAnimalsMenuPostfix()
		{
			if (!ModEntry.Config.ShopsBetterAnimalPurchase)
				return;

			PreviousVariantButton = new ClickableTextureComponent(new Rectangle(-100, -100, 64, 64), Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 44), 1f);
			NextVariantButton = new ClickableTextureComponent(new Rectangle(-100, -100, 64, 64), Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 33), 1f);
		}

		private static bool SetUpForReturnAfterPurchasingAnimalPrefix(PurchaseAnimalsMenu __instance)
		{
			if (!ModEntry.Config.ShopsBetterAnimalPurchase)
				return true;

			Game1.addHUDMessage(new HUDMessage(__instance.animalBeingPurchased.isMale() ? Game1.content.LoadString("Strings\\StringsFromCSFiles:PurchaseAnimalsMenu.cs.11311", __instance.animalBeingPurchased.displayName) : Game1.content.LoadString("Strings\\StringsFromCSFiles:PurchaseAnimalsMenu.cs.11314", __instance.animalBeingPurchased.displayName), HUDMessage.achievement_type));
			Game1.playSound("purchaseClick");
			Game1.playSound("objectiveComplete");
			__instance.namingAnimal = false;
			__instance.textBox.Selected = false;
			__instance.textBox.OnEnterPressed -= (TextBoxEvent)typeof(PurchaseAnimalsMenu).GetField(Constants.TargetPlatform == GamePlatform.Android ? "e" : "textBoxEvent", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance).GetValue(__instance);
			__instance.animalBeingPurchased = new FarmAnimal(Randomize && AlternatePurchaseTypes.Any() ? Game1.random.ChooseFrom(AlternatePurchaseTypes) : __instance.animalBeingPurchased.type.Value, Game1.Multiplayer.getNewID(), __instance.animalBeingPurchased.ownerID.Value);
			AlternatePurchaseTypesUtility.SetVariantButtonBounds(__instance.animalBeingPurchased);
			return false;
		}

		private static void PerformHoverActionPostfix(PurchaseAnimalsMenu __instance, int x, int y)
		{
			if (!ModEntry.Config.ShopsBetterAnimalPurchase || Game1.IsFading() || __instance.freeze || !AlternatePurchaseTypes.Any())
				return;

			if (__instance.onFarm && !__instance.namingAnimal)
			{
				PreviousVariantButton.tryHover(x, y);
				NextVariantButton.tryHover(x, y);
			}
		}

		private static bool ReceiveLeftClickPrefix(PurchaseAnimalsMenu __instance, int x, int y)
		{
			if (!ModEntry.Config.ShopsBetterAnimalPurchase || Game1.IsFading() || __instance.freeze)
				return true;

			Vector2 tile = new((int)((Utility.ModifyCoordinateFromUIScale(x) + Game1.viewport.X) / 64f), (int)((Utility.ModifyCoordinateFromUIScale(y) + Game1.viewport.Y) / 64f));
			Building buildingAt = __instance.TargetLocation.getBuildingAt(tile);

			if (__instance.onFarm)
			{
				if (!__instance.namingAnimal && buildingAt?.GetIndoors() is AnimalHouse animalHouse && !buildingAt.isUnderConstruction())
				{
					if (__instance.animalBeingPurchased.CanLiveIn(buildingAt))
					{
						if (!animalHouse.isFull())
						{
							if (Game1.player.Money < __instance.priceOfAnimal)
							{
								Game1.dayTimeMoneyBox.moneyShakeTimer = 1000;
								Game1.playSound("cancel");
								return false;
							}
						}
					}
				}
			}
			else
			{
				foreach (ClickableTextureComponent item in __instance.animalsToPurchase)
				{
					if (!__instance.readOnly && item.containsPoint(x, y) && (item.item as Object).Type is null && Game1.player.Money >= item.item.salePrice())
					{
						AlternatePurchaseTypesUtility.SetAlternatePurchaseTypes(item.hoverText);
					}
				}
			}
			return true;
		}

		private static void ReceiveLeftClickPostfix(PurchaseAnimalsMenu __instance, int x, int y)
		{
			if (!ModEntry.Config.ShopsBetterAnimalPurchase || Game1.IsFading() || __instance.freeze)
				return;

			if (!__instance.onFarm)
			{
				foreach (ClickableTextureComponent item in __instance.animalsToPurchase)
				{
					if (!__instance.readOnly && item.containsPoint(x, y) && (item.item as Object).Type is null && Game1.player.Money >= item.item.salePrice())
					{
						AlternatePurchaseTypesUtility.SetVariantButtonBounds(__instance.animalBeingPurchased);
					}
				}
			}
		}

		private static void ReceiveLeftClickPostfixVariantButtons(PurchaseAnimalsMenu __instance, int x, int y)
		{
			if (!ModEntry.Config.ShopsBetterAnimalPurchase || Game1.IsFading() || __instance.freeze || !AlternatePurchaseTypes.Any())
				return;

			if (__instance.onFarm && !__instance.namingAnimal)
			{
				if (PreviousVariantButton.containsPoint(x, y))
				{
					AlternatePurchaseTypesUtility.SelectPreviousVariant(__instance);
				}
				else if (NextVariantButton.containsPoint(x, y))
				{
					AlternatePurchaseTypesUtility.SelectNextVariant(__instance);
				}
			}
		}

		private static void DrawPrefix(PurchaseAnimalsMenu __instance, SpriteBatch b)
		{
			if (!ModEntry.Config.ShopsBetterAnimalPurchase || Game1.IsFading() || __instance.freeze || !AlternatePurchaseTypes.Any())
				return;

			if (__instance.onFarm)
			{
				PreviousVariantButton.draw(b);
				NextVariantButton.draw(b);
			}
		}

		private static void DrawPostfix(PurchaseAnimalsMenu __instance, SpriteBatch b)
		{
			if (!ModEntry.Config.ShopsBetterAnimalPurchase || Game1.IsFading() || __instance.freeze)
				return;

			if (__instance.onFarm && (Constants.TargetPlatform != GamePlatform.Android || !__instance.namingAnimal))
			{
				GamePlatformUtility.DrawMoneyBox(b);
			}
		}
	}
}
