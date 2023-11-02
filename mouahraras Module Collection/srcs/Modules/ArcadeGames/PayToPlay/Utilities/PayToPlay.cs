using StardewModdingAPI.Utilities;

namespace mouahrarasModuleCollection.ArcadeGames.PayToPlay.Utilities
{
	internal class PayToPlayUtility
	{
		private static readonly PerScreen<bool> onInsertCoinMenu = new(() => true);
		private static readonly PerScreen<bool> triedToInsertCoin = new(() => true);

		internal static void Reset()
		{
			SetOnInsertCoinMenu(true);
			SetTriedToInsertCoin(true);
		}

		internal static void SetOnInsertCoinMenu(bool value)
		{
			onInsertCoinMenu.Value = value;
		}

		internal static bool GetOnInsertCoinMenu()
		{
			return onInsertCoinMenu.Value;
		}

		internal static void SetTriedToInsertCoin(bool value)
		{
			triedToInsertCoin.Value = value;
		}

		internal static bool GetTriedToInsertCoin()
		{
			return triedToInsertCoin.Value;
		}
	}
}
