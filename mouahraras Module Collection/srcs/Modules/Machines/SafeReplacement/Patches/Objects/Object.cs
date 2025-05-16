using HarmonyLib;
using StardewValley;
using mouahrarasModuleCollection.Machines.SafeReplacement.Utilities;

namespace mouahrarasModuleCollection.Machines.SafeReplacement.Patches
{
	internal class ObjectPatch
	{
		internal static void Apply(Harmony harmony)
		{
			harmony.Patch(
				original: AccessTools.Method(typeof(Object), nameof(Object.performObjectDropInAction), new System.Type[] { typeof(Item), typeof(bool), typeof(Farmer), typeof(bool) }),
				prefix: new HarmonyMethod(typeof(ObjectPatch), nameof(PerformObjectDropInActionPrefix))
			);
		}

		private static bool PerformObjectDropInActionPrefix(Object __instance, Item dropInItem, bool probe, Farmer who, ref bool __result)
		{
			if (!ModEntry.Config.MachinesSafeReplacement || __instance.isTemporarilyInvisible || dropInItem is not Object || !__instance.name.Equals("Crystalarium"))
				return true;

			if ((dropInItem.HasContextTag("category_gem") || dropInItem.HasContextTag("category_minerals")) && !dropInItem.HasContextTag("crystalarium_banned") && (__instance.heldObject.Value is null || __instance.heldObject.Value.QualifiedItemId != dropInItem.QualifiedItemId) && (__instance.heldObject.Value is null || __instance.MinutesUntilReady > 0))
			{
				if (!probe)
				{
					if (who.freeSpotsInInventory() > 0 || (who.freeSpotsInInventory() == 0 && dropInItem.Stack == 1))
					{
						SafeReplacementUtility.ObjectToRecover = __instance.heldObject.Value;
						__instance.heldObject.Value = null;
						return true;
					}
					if (who.couldInventoryAcceptThisItem(__instance.heldObject.Value))
					{
						SafeReplacementUtility.ObjectToRecover = __instance.heldObject.Value;
						__instance.heldObject.Value = null;
						return true;
					}
					__result = false;
					return false;
				}
			}
			return true;
		}
	}
}
