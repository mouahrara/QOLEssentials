using StardewModdingAPI.Utilities;
using StardewValley;

namespace mouahrarasModuleCollection.Crystalariums.SafeReplacement.Utilities
{
	internal class SafeReplacementUtility
	{
		private static readonly PerScreen<Item> objectToRecover = new(() => null);

		internal static void Reset()
		{
			SetObjectToRecover(null);
		}

		internal static void SetObjectToRecover(Item value)
		{
			objectToRecover.Value = value;
		}

		internal static Item GetObjectToRecover()
		{
			return objectToRecover.Value;
		}
	}
}
