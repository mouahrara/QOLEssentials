using System;
using HarmonyLib;
using StardewValley;
using mouahrarasModuleCollection.TweaksAndFeatures.ArcadeGames.KonamiCode.Utilities;

namespace mouahrarasModuleCollection.TweaksAndFeatures.ArcadeGames.KonamiCode.Patches
{
	internal class MultiplayerPatch
	{
		internal static void Apply(Harmony harmony)
		{
			harmony.Patch(
				original: AccessTools.Method(typeof(Multiplayer), nameof(Multiplayer.globalChatInfoMessage), new Type[] { typeof(string), typeof(string[]) }),
				prefix: new HarmonyMethod(typeof(MultiplayerPatch), nameof(GlobalChatInfoMessagePrefix))
			);
		}

		private static bool GlobalChatInfoMessagePrefix(string messageKey)
		{
			if (!ModEntry.Config.ArcadeGamesPayToPlayKonamiCode || !KonamiCodeUtility.InfiniteLivesMode)
				return true;

			if (messageKey.Equals("PrairieKing") || messageKey.Equals("JunimoKart"))
				return false;
			return true;
		}
	}
}
