using StardewModdingAPI.Utilities;

namespace mouahrarasModuleCollection.ArcadeGames.PayToPlay.Utilities
{
	internal class PayToPlayUtility
	{
		private static readonly PerScreen<bool> onInsertCoinMenu = new(() => true);
		private static readonly PerScreen<bool> triedToInsertCoin = new(() => true);

		internal static void Reset()
		{
			OnInsertCoinMenu = true;
			TriedToInsertCoin = true;
		}

		internal static bool OnInsertCoinMenu
		{
			get => onInsertCoinMenu.Value;
			set => onInsertCoinMenu.Value = value;
		}

		internal static bool TriedToInsertCoin
		{
			get => triedToInsertCoin.Value;
			set => triedToInsertCoin.Value = value;
		}
	}
}
