using System;
using System.Reflection;
using HarmonyLib;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Minigames;
using QOLEssentials.ArcadeGames.FullScreen.Utilities;

namespace QOLEssentials.ArcadeGames.FullScreen.Patches
{
	internal class MineCartPatch
	{
		private const int	screenWidth = 400;
		private const int	screenHeight = 225;

		internal static void Apply(Harmony harmony)
		{
			harmony.Patch(
				original: AccessTools.Method(typeof(MineCart), nameof(MineCart.changeScreenSize)),
				prefix: new HarmonyMethod(typeof(MineCartPatch), nameof(ChangeScreenSizePrefix))
			);
			harmony.Patch(
				original: AccessTools.Method(typeof(MineCart), nameof(MineCart.QuitGame)),
				postfix: new HarmonyMethod(typeof(MineCartPatch), nameof(QuitGamePostfix))
			);
			harmony.Patch(
				original: AccessTools.Method(typeof(MineCart), nameof(MineCart.forceQuit)),
				postfix: new HarmonyMethod(typeof(MineCartPatch), nameof(ForceQuitPostfix))
			);
		}

		private static bool ChangeScreenSizePrefix(MineCart __instance)
		{
			if (!ModEntry.Config.ArcadeGamesFullScreen)
				return true;

			float windowWidth = Game1.game1.localMultiplayerWindow.Width;
			float windowHeight = Game1.game1.localMultiplayerWindow.Height;
			float rawScale = Math.Min(windowWidth / screenWidth, windowHeight / screenHeight) * ModEntry.Config.ArcadeGamesFullScreenFillRatio;
			int pixelScale = Math.Max(1, (int)Math.Round(rawScale));
			float zoom = rawScale / pixelScale;
			float viewportWidth = windowWidth / zoom;
			float viewportHeight = windowHeight / zoom;

			ZoomUtility.SetZoomLevel(zoom / Game1.game1.zoomModifier, Game1.currentMinigame is not MineCart);
			__instance.tileSize = 16;
			__instance.pixelScale = pixelScale;
			typeof(MineCart).GetField("screenWidth", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(__instance, screenWidth);
			typeof(MineCart).GetField("screenHeight", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(__instance, screenHeight);
			__instance.upperLeft = new Vector2((viewportWidth - screenWidth * pixelScale) / 2f, (viewportHeight - screenHeight * pixelScale) / 2f);
			typeof(MineCart).GetField("ytileOffset", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(__instance, screenHeight / 2 / __instance.tileSize);
			return false;
		}

		private static void QuitGamePostfix()
		{
			if (!ModEntry.Config.ArcadeGamesFullScreen)
				return;

			ZoomUtility.Reset();
		}

		private static void ForceQuitPostfix(bool __result)
		{
			if (!ModEntry.Config.ArcadeGamesFullScreen)
				return;

			if (__result)
			{
				ZoomUtility.Reset();
			}
		}
	}
}
