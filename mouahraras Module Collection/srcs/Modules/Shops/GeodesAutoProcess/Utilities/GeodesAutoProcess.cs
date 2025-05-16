using System.Collections.Generic;
using StardewModdingAPI;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Menus;
using mouahrarasModuleCollection.Shops.GeodesAutoProcess.Handlers;

namespace mouahrarasModuleCollection.Shops.GeodesAutoProcess.Utilities
{
	internal class GeodesAutoProcessUtility
	{
		private static readonly PerScreen<GeodeMenu>	geodeMenu = new(() => null);
		private static readonly PerScreen<Item>			geodeBeingProcessed = new(() => null);
		private static readonly PerScreen<Item>			foundArtifact = new(() => null);

		internal static GeodeMenu GeodeMenu
		{
			get => geodeMenu.Value;
			set => geodeMenu.Value = value;
		}

		internal static Item GeodeBeingProcessed
		{
			get => geodeBeingProcessed.Value;
			set => geodeBeingProcessed.Value = value;
		}

		internal static Item FoundArtifact
		{
			get => foundArtifact.Value;
			set => foundArtifact.Value = value;
		}

		internal static void InitializeAfterOpeningGeodeMenu(GeodeMenu __instance)
		{
			GeodeMenu = __instance;
			FoundArtifact = null;
		}

		internal static void CleanBeforeClosingGeodeMenu()
		{
			EndGeodeProcessing();
			FoundArtifact = null;
			GeodeMenu = null;
		}

		internal static bool IsInventoryFullForGeodeProcessing(Farmer player, Item item)
		{
			int freeSpots = player.freeSpotsInInventory();
			int requiredSpots = Constants.TargetPlatform == GamePlatform.Android ? 1 : 2;

			return freeSpots < requiredSpots && (freeSpots != requiredSpots - 1 || item.Stack != 1);
		}

		internal static void StartGeodeProcessing()
		{
			if (Constants.TargetPlatform == GamePlatform.Android)
			{
				GeodeBeingProcessed = (Item)typeof(MenuWithInventory).GetField("heldItem").GetValue(GeodeMenu);
				typeof(MenuWithInventory).GetField("heldItem").SetValue(GeodeMenu, null);
			}
			else
			{
				GeodeBeingProcessed = GeodeMenu.heldItem;
				GeodeMenu.heldItem = null;
			}
			GeodeMenu.inventory.highlightMethod = (Item _) => false;
			ModEntry.Helper.Events.GameLoop.UpdateTicking += UpdateTickingHandler.Apply;
		}

		internal static void EndGeodeProcessing()
		{
			ModEntry.Helper.Events.GameLoop.UpdateTicking -= UpdateTickingHandler.Apply;
			GeodeMenu.inventory.highlightMethod = GeodeMenu.highlightGeodes;
			if (Constants.TargetPlatform != GamePlatform.Android && IsProcessing())
			{
				Game1.player.addItemToInventory(GeodeBeingProcessed);
			}
			GeodeBeingProcessed = null;
		}

		internal static bool IsProcessing()
		{
			return GeodeBeingProcessed is not null;
		}

		internal static void CrackGeodeSecure()
		{
			if (CanProcess())
			{
				GeodeMenu.geodeSpot.item = ItemRegistry.Create(GeodeBeingProcessed.QualifiedItemId);
				if (GeodeMenu.geodeSpot.item.QualifiedItemId == "(O)791" && !Game1.netWorldState.Value.GoldenCoconutCracked)
				{
					GeodeMenu.waitingForServerResponse = true;
					Game1.player.team.goldenCoconutMutex.RequestLock(delegate
					{
						GeodeMenu.waitingForServerResponse = false;
						GeodeMenu.geodeTreasureOverride = ItemRegistry.Create("(O)73");
						CrackGeode();
					}, delegate
					{
						GeodeMenu.waitingForServerResponse = false;
						CrackGeode();
					});
				}
				else
				{
					CrackGeode();
				}
				CanProcess();
			}
		}

		private static bool CanProcess()
		{
			if (GeodeMenu.waitingForServerResponse)
			{
				return false;
			}
			if (GeodeBeingProcessed is null || Game1.player.Money < 25)
			{
				EndGeodeProcessing();
				return false;
			}
			if (IsInventoryFullForGeodeProcessing(Game1.player, GeodeBeingProcessed))
			{
				EndGeodeProcessing();
				GeodeMenu.descriptionText = Game1.content.LoadString("Strings\\UI:GeodeMenu_InventoryFull");
				GeodeMenu.wiggleWordsTimer = 500;
				GeodeMenu.alertTimer = 1500;
				return false;
			}
			return true;
		}

		private static void CrackGeode()
		{
			if (--GeodeBeingProcessed.Stack <= 0)
			{
				if (Constants.TargetPlatform == GamePlatform.Android)
				{
					for (int i = 0; i < Game1.player.Items.Count; i++)
					{
						if (Game1.player.Items[i] is not null && Game1.player.Items[i].Stack <= 0)
						{
							Game1.player.Items[i] = null;
						}
					}
				}
				GeodeBeingProcessed = null;
			}
			Game1.player.Money -= 25;
			Game1.playSound("stoneStep");
			GeodeMenu.geodeAnimationTimer = 2700;
			GeodeMenu.clint.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
			{
				new(8, 300),
				new(9, 200),
				new(10, 80),
				new(11, 200),
				new(12, 100),
				new(8, 300)
			});
			GeodeMenu.clint.loop = false;
		}
	}
}
