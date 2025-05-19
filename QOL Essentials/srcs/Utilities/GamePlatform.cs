using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;
using StardewValley.Minigames;

namespace QOLEssentials.Utilities
{
	internal class GamePlatformUtility
	{
		internal static void DrawMoneyBox(SpriteBatch b, int overrideX = -1, int overrideY = -1)
		{
			if (Constants.TargetPlatform == GamePlatform.Android)
			{
				int x = overrideX != -1 ? overrideX : (Game1.currentMinigame is not null ? Game1.viewport.Width : Game1.uiViewport.Width) - ((Rectangle)typeof(DayTimeMoneyBox).GetField("sourceRect", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(Game1.dayTimeMoneyBox)).Width * 4 - (int)typeof(DayTimeMoneyBox).GetField("paddingX", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(Game1.dayTimeMoneyBox) + 64;
				int y = overrideY != -1 ? overrideY : 0;

				typeof(DayTimeMoneyBox).GetMethod("drawMoneyBox").Invoke(Game1.dayTimeMoneyBox, new object[] { b, x, y, true });
			}
			else
			{
				int x = overrideX != -1 ? overrideX : Game1.dayTimeMoneyBox.xPositionOnScreen;
				int y = overrideY != -1 ? overrideY : 0;

				Game1.dayTimeMoneyBox.drawMoneyBox(b, x, y);
			}
		}
	}
}
