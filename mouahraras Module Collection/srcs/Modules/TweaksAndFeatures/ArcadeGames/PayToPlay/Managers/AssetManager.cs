using System;
using Microsoft.Xna.Framework.Graphics;

namespace mouahrarasModuleCollection.TweaksAndFeatures.ArcadeGames.PayToPlay.Managers
{
	internal static class AssetManager
	{
		public static Texture2D JourneyOfThePrairieKing;

		internal static void Apply()
		{
			if (ModEntry.Helper.Translation.Locale.Equals("fr-FR", StringComparison.OrdinalIgnoreCase))
				JourneyOfThePrairieKing = ModEntry.Helper.ModContent.Load<Texture2D>("assets/ArcadeGames/PayToPlay/JourneyOfThePrairieKing.fr-FR.png");
			else if (ModEntry.Helper.Translation.Locale.Equals("zh-CN", StringComparison.OrdinalIgnoreCase))
				JourneyOfThePrairieKing = ModEntry.Helper.ModContent.Load<Texture2D>("assets/ArcadeGames/PayToPlay/JourneyOfThePrairieKing.zh-CN.png");
			else
				JourneyOfThePrairieKing = ModEntry.Helper.ModContent.Load<Texture2D>("assets/ArcadeGames/PayToPlay/JourneyOfThePrairieKing.png");
		}
	}
}
