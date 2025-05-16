using System;
using System.Reflection;
using HarmonyLib;
using Microsoft.Xna.Framework.Input;
using StardewValley.Minigames;
using mouahrarasModuleCollection.ArcadeGames.KonamiCode.Utilities;

namespace mouahrarasModuleCollection.ArcadeGames.KonamiCode.Patches
{
	internal class MineCartPatch
	{
		internal static void Apply(Harmony harmony)
		{
			harmony.Patch(
				original: AccessTools.Method(typeof(MineCart), nameof(MineCart.ResetState)),
				postfix: new HarmonyMethod(typeof(MineCartPatch), nameof(ResetStatePostfix))
			);
			harmony.Patch(
				original: AccessTools.Method(typeof(MineCart), "restartLevel", new Type[] { typeof(bool) }),
				postfix: new HarmonyMethod(typeof(MineCartPatch), nameof(RestartLevelPostfix))
			);
			harmony.Patch(
				original: AccessTools.Method(typeof(MineCart), nameof(MineCart.receiveKeyPress), new Type[] { typeof(Keys) }),
				postfix: new HarmonyMethod(typeof(MineCartPatch), nameof(ReceiveKeyPressPostfix))
			);
			harmony.Patch(
				original: AccessTools.Method(typeof(MineCart), nameof(MineCart.UpdateFruitsSummary), new Type[] { typeof(float) }),
				postfix: new HarmonyMethod(typeof(MineCartPatch), nameof(UpdateFruitsSummaryPostfix))
			);
			harmony.Patch(
				original: AccessTools.Method(typeof(MineCart), nameof(MineCart.CollectCoin), new Type[] { typeof(int) }),
				postfix: new HarmonyMethod(typeof(MineCartPatch), nameof(CollectCoinPostfix))
			);
			harmony.Patch(
				original: AccessTools.Method(typeof(MineCart), nameof(MineCart.Die)),
				prefix: new HarmonyMethod(typeof(MineCartPatch), nameof(DiePrefixPostfix)),
				postfix: new HarmonyMethod(typeof(MineCartPatch), nameof(DiePrefixPostfix))
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

		private static void ResetStatePostfix(MineCart __instance)
		{
			if (!ModEntry.Config.ArcadeGamesPayToPlayKonamiCode || !KonamiCodeUtility.InfiniteLivesMode)
				return;

			typeof(MineCart).GetField("livesLeft", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(__instance, 5);
		}

		private static void RestartLevelPostfix(MineCart __instance)
		{
			if (!ModEntry.Config.ArcadeGamesPayToPlayKonamiCode || !KonamiCodeUtility.InfiniteLivesMode)
				return;

			typeof(MineCart).GetField("livesLeft", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(__instance, 5);
		}

		private static void ReceiveKeyPressPostfix(MineCart __instance, Keys k)
		{
			if ((int)typeof(MineCart).GetField("gameMode", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(__instance) == MineCart.infiniteMode)
				return;

			KonamiCodeUtility.ReceiveKeyPressPostfix(k);
			if (KonamiCodeUtility.InfiniteLivesMode)
			{
				typeof(MineCart).GetField("livesLeft", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(__instance, 5);;
			}
		}

		private static void UpdateFruitsSummaryPostfix(MineCart __instance)
		{
			if (!ModEntry.Config.ArcadeGamesPayToPlayKonamiCode || !KonamiCodeUtility.InfiniteLivesMode)
				return;

			typeof(MineCart).GetField("livesLeft", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(__instance, 5);
		}

		private static void CollectCoinPostfix(MineCart __instance)
		{
			if (!ModEntry.Config.ArcadeGamesPayToPlayKonamiCode || !KonamiCodeUtility.InfiniteLivesMode)
				return;

			typeof(MineCart).GetField("livesLeft", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(__instance, 5);
		}

		private static void DiePrefixPostfix(MineCart __instance)
		{
			if (!ModEntry.Config.ArcadeGamesPayToPlayKonamiCode || !KonamiCodeUtility.InfiniteLivesMode)
				return;

			typeof(MineCart).GetField("livesLeft", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(__instance, 5);
		}

		private static void QuitGamePostfix()
		{
			if (!ModEntry.Config.ArcadeGamesPayToPlayKonamiCode || !KonamiCodeUtility.InfiniteLivesMode)
				return;

			KonamiCodeUtility.Reset();
		}

		private static void ForceQuitPostfix(bool __result)
		{
			if (!ModEntry.Config.ArcadeGamesPayToPlayKonamiCode || !KonamiCodeUtility.InfiniteLivesMode)
				return;

			if (__result)
			{
				KonamiCodeUtility.Reset();
			}
		}
	}
}
