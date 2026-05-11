using HarmonyLib;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;

namespace QOLEssentials.UserInterface.Zoom.Patches
{
	internal class OptionsPatch
	{
		internal static void Apply(Harmony harmony)
		{
			harmony.Patch(
				original: AccessTools.PropertyGetter(typeof(Options), nameof(Options.zoomLevel)),
				postfix: new HarmonyMethod(typeof(OptionsPatch), nameof(ZoomLevelPostfix))
			);
		}

		private static void ZoomLevelPostfix()
		{
			if (!Context.IsWorldReady || !ModEntry.Config.UserInterfaceZoom || Game1.currentLocation is null)
				return;

			if (Game1.activeClickableMenu is CarpenterMenu or PurchaseAnimalsMenu or AnimalQueryMenu && Game1.activeClickableMenu.shouldClampGamePadCursor())
			{
				ClickableTextureComponent cancelButton = (Game1.activeClickableMenu as CarpenterMenu)?.cancelButton ?? (Game1.activeClickableMenu as PurchaseAnimalsMenu)?.okButton ?? (Game1.activeClickableMenu as AnimalQueryMenu)?.okButton;

				cancelButton?.setPosition(new Vector2(Game1.uiViewport.Width - cancelButton.bounds.Width - 64, Game1.uiViewport.Height - cancelButton.bounds.Height - 64));
			}
		}
	}
}
