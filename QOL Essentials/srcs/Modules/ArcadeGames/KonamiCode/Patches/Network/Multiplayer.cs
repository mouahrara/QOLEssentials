using System;
using HarmonyLib;
using StardewValley;
using QOLEssentials.ArcadeGames.KonamiCode.Utilities;

namespace QOLEssentials.ArcadeGames.KonamiCode.Patches
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

			return messageKey != "PrairieKing" && messageKey != "JunimoKart";
		}
	}
}
