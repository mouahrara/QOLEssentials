using Microsoft.Xna.Framework.Graphics;
using StardewValley;

namespace QOLEssentials.ArcadeGames.PayToPlay.Managers
{
	internal static class AssetManager
	{
		public static Texture2D	JourneyOfThePrairieKing;

		internal static void Apply()
		{
			string texturePath = Game1.content.GetCurrentLanguage() switch
			{
				LocalizedContentManager.LanguageCode.fr => "assets/ArcadeGames/PayToPlay/JourneyOfThePrairieKing.fr-FR.png",
				LocalizedContentManager.LanguageCode.ko => "assets/ArcadeGames/PayToPlay/JourneyOfThePrairieKing.ko-KR.png",
				LocalizedContentManager.LanguageCode.zh => "assets/ArcadeGames/PayToPlay/JourneyOfThePrairieKing.zh-CN.png",
				_ => "assets/ArcadeGames/PayToPlay/JourneyOfThePrairieKing.png"
			};

			JourneyOfThePrairieKing = ModEntry.Helper.ModContent.Load<Texture2D>(texturePath);
		}
	}
}
