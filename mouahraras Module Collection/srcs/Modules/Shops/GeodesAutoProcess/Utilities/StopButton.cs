using System.Reflection;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;

namespace mouahrarasModuleCollection.Shops.GeodesAutoProcess.Utilities
{
	internal class StopButtonUtility
	{
		internal static Vector2 GetStopButtonPosition(GeodeMenu __instance, int width, int height)
		{
			if (Constants.TargetPlatform == GamePlatform.Android)
			{
				Rectangle infoBox = (Rectangle)typeof(GeodeMenu).GetField("infoBox", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(__instance);
				int x = infoBox.X + infoBox.Width / 2 - width * 4 / 2;
				int y = infoBox.Y + infoBox.Height - 64 - height * 4 / 2;

				return new Vector2(x, y);
			}
			else
			{
				int x = __instance.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth / 2 + 692 - width * 4 / 2;
				int y = __instance.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + 262 + GetStopButtonYOffset() - height * 4 / 2;

				return new Vector2(x, y);
			}
		}

		internal static int GetStopButtonWidthOffset()
		{
			return Game1.content.GetCurrentLanguage() switch
			{
				LocalizedContentManager.LanguageCode.de => 3,
				LocalizedContentManager.LanguageCode.fr => 6,
				LocalizedContentManager.LanguageCode.hu => 4,
				LocalizedContentManager.LanguageCode.it => 2,
				LocalizedContentManager.LanguageCode.pt => 10,
				LocalizedContentManager.LanguageCode.ru => 9,
				_ => 0
			};
		}

		internal static int GetStopButtonYOffset()
		{
			return Game1.content.GetCurrentLanguage() switch
			{
				LocalizedContentManager.LanguageCode.hu => 14,
				LocalizedContentManager.LanguageCode.ko => 50,
				LocalizedContentManager.LanguageCode.pt => 14,
				_ => 0
			};
		}
	}
}
