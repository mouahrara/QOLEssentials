using Microsoft.Xna.Framework.Input;
using StardewValley;
using StardewValley.Menus;
using QOLEssentials.UserInterface.Zoom.Utilities;

namespace QOLEssentials.UserInterface.FastScrolling.Utilities
{
	internal class MenusPatchUtility
	{
		internal static void ReceiveKeyPressPostfix(IClickableMenu __instance, Keys key)
		{
			if (ShouldProcess(__instance))
			{
				float consistentScrollingMultiplier = ModEntry.Config.UserInterfaceFastScrollingConsistentScrolling && ZoomUtility.ZoomLevel > 0 ? Game1.options.desiredBaseZoomLevel / ZoomUtility.ZoomLevel : 1f;
				int offset = 2 * (int)((ModEntry.Config.UserInterfaceFastScrollingMultiplier - 1) * 4 * consistentScrollingMultiplier);

				if (Game1.options.doesInputListContain(Game1.options.moveLeftButton, key))
				{
					Game1.panScreen(-offset, 0);
				}
				else if (Game1.options.doesInputListContain(Game1.options.moveRightButton, key))
				{
					Game1.panScreen(offset, 0);
				}
				else if (Game1.options.doesInputListContain(Game1.options.moveUpButton, key))
				{
					Game1.panScreen(0, -offset);
				}
				else if (Game1.options.doesInputListContain(Game1.options.moveDownButton, key))
				{
					Game1.panScreen(0, offset);
				}
			}
		}

		internal static void UpdatePostfix(IClickableMenu __instance)
		{
			if (ShouldProcess(__instance))
			{
				int x = Game1.getOldMouseX(ui_scale: false) + Game1.viewport.X;
				int y = Game1.getOldMouseY(ui_scale: false) + Game1.viewport.Y;
				float consistentScrollingMultiplier = ModEntry.Config.UserInterfaceFastScrollingConsistentScrolling && ZoomUtility.ZoomLevel > 0 ? Game1.options.desiredBaseZoomLevel / ZoomUtility.ZoomLevel : 1f;
				int offset = 2 * (int)((ModEntry.Config.UserInterfaceFastScrollingMultiplier - 1) * 4 * consistentScrollingMultiplier);

				if (x - Game1.viewport.X < 64)
				{
					Game1.panScreen(-offset, 0);
				}
				else if (x - (Game1.viewport.X + Game1.viewport.Width) >= -128)
				{
					Game1.panScreen(offset, 0);
				}
				if (y - Game1.viewport.Y < 64)
				{
					Game1.panScreen(0, -offset);
				}
				else if (y - (Game1.viewport.Y + Game1.viewport.Height) >= -64)
				{
					Game1.panScreen(0, offset);
				}
			}
		}

		private static bool ShouldProcess(IClickableMenu __instance)
		{
			if (!ModEntry.Config.UserInterfaceFastScrolling)
				return false;

			return (__instance is CarpenterMenu or PurchaseAnimalsMenu or AnimalQueryMenu) && ((__instance as CarpenterMenu)?.freeze != true) && ((__instance as PurchaseAnimalsMenu)?.freeze != true) && !Game1.IsFading() && __instance.shouldClampGamePadCursor() && __instance.overrideSnappyMenuCursorMovementBan();
		}
	}
}
