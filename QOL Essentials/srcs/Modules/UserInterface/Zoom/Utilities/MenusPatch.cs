using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Menus;
using QOLEssentials.UserInterface.Zoom.Handlers;

namespace QOLEssentials.UserInterface.Zoom.Utilities
{
	internal class MenusPatchUtility
	{
		private static readonly PerScreen<int>	afterResetTicks = new(() => -1);

		internal static int AfterResetTicks
		{
			get => afterResetTicks.Value;
			set => afterResetTicks.Value = value;
		}

		internal static void EnterFarmViewPostfix()
		{
			if (!ModEntry.Config.UserInterfaceZoom)
				return;

			ModEntry.Helper.Events.GameLoop.UpdateTicking -= UpdateTickingHandler.Apply;
			ModEntry.Helper.Events.GameLoop.UpdateTicking += UpdateTickingHandler.Apply;
		}

		internal static void LeaveFarmViewPostfix()
		{
			if (!ModEntry.Config.UserInterfaceZoom)
				return;

			ModEntry.Helper.Events.GameLoop.UpdateTicking -= UpdateTickingHandler.Apply;
			if (Game1.isWarping && Game1.locationRequest is not null)
			{
				Game1.locationRequest.OnLoad += ZoomUtility.Reset;
			}
			else
			{
				ZoomUtility.Reset();
			}
		}

		internal static bool ShouldProcess(IClickableMenu __instance)
		{
			if (!ModEntry.Config.UserInterfaceZoom)
				return false;

			return (__instance is CarpenterMenu or PurchaseAnimalsMenu or AnimalQueryMenu) && ((__instance as CarpenterMenu)?.freeze != true) && ((__instance as PurchaseAnimalsMenu)?.freeze != true) && !Game1.IsFading() && __instance.shouldClampGamePadCursor() && __instance.overrideSnappyMenuCursorMovementBan();
		}
	}
}
