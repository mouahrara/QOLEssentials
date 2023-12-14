using StardewModdingAPI.Utilities;
using StardewValley;

namespace mouahrarasModuleCollection.TweaksAndFeatures.Machines.SafeReplacement.Utilities
{
	internal class SafeReplacementUtility
	{
		private static readonly PerScreen<Item> objectToRecover = new(() => null);

		internal static void Reset()
		{
			ObjectToRecover = null;
		}

		internal static Item ObjectToRecover
		{
			get => objectToRecover.Value;
			set => objectToRecover.Value = value;
		}
	}
}
