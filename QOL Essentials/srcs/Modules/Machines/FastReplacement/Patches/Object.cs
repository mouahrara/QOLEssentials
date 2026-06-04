using System.Reflection;
using HarmonyLib;
using StardewValley;
using StardewValley.GameData.Machines;

namespace QOLEssentials.Machines.FastReplacement.Patches
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
			if (!ModEntry.Config.MachinesFastReplacement || __instance.isTemporarilyInvisible || probe)
				return true;

			MachineData machineData = __instance.GetMachineData();

			if (machineData is null || __instance.MinutesUntilReady <= 0 || __instance.lastInputItem.Value is null)
				return true;
			if (!MachineDataUtility.TryGetMachineOutputRule(__instance, machineData, MachineOutputTrigger.ItemPlacedInMachine, __instance.lastInputItem.Value, who, __instance.Location, out _, out MachineOutputTriggerRule currentTriggerRule, out _, out _))
				return true;
			if (!MachineDataUtility.TryGetMachineOutputRule(__instance, machineData, MachineOutputTrigger.ItemPlacedInMachine, dropInItem, who, __instance.Location, out MachineOutputRule newRule, out MachineOutputTriggerRule newTriggerRule, out _, out _))
				return true;
			if (__instance.lastInputItem.Value.canStackWith(dropInItem))
				return true;

			Item lastInputItem = null;

			if (currentTriggerRule.RequiredCount > 0)
			{
				lastInputItem = __instance.lastInputItem.Value.getOne();
				lastInputItem.Stack = currentTriggerRule.RequiredCount;
			}
			if (lastInputItem is not null)
			{
				bool canFit = (newTriggerRule.RequiredCount > 0 && dropInItem.Stack == newTriggerRule.RequiredCount) || who.couldInventoryAcceptThisItem(lastInputItem);

				if (!canFit)
				{
					__result = false;
					return false;
				}
			}

			Object savedHeldObject = __instance.heldObject.Value;

			__instance.heldObject.Value = null;
			if (!__instance.OutputMachine(machineData, newRule, dropInItem, who, __instance.Location, false))
			{
				__instance.heldObject.Value = savedHeldObject;
				__result = false;
				return false;
			}
			if (newTriggerRule.RequiredCount > 0)
			{
				typeof(Object).GetMethod(nameof(Object.ConsumeInventoryItem), BindingFlags.Public | BindingFlags.Static).Invoke(null, new object[] { who, dropInItem, newTriggerRule.RequiredCount });
			}
			if (lastInputItem is not null)
			{
				who.addItemToInventory(lastInputItem);
			}
			if (machineData.LoadEffects is not null)
			{
				foreach (MachineEffects loadEffect in machineData.LoadEffects)
				{
					if (loadEffect.Condition is null || GameStateQuery.CheckConditions(loadEffect.Condition, __instance.Location, null, __instance.heldObject.Value, __instance.lastInputItem.Value))
					{
						if (loadEffect.Sounds is not null)
						{
							foreach (MachineSoundData sound in loadEffect.Sounds)
							{
								if (sound.Delay <= 0)
								{
									__instance.Location?.playSound(sound.Id, __instance.TileLocation);
								}
								else
								{
									DelayedAction.playSoundAfterDelay(sound.Id, sound.Delay, __instance.Location, __instance.TileLocation);
								}
							}
						}
						break;
					}
				}
			}
			MachineDataUtility.UpdateStats(machineData.StatsToIncrementWhenLoaded, dropInItem, 1);
			who.ignoreItemConsumptionThisFrame = true;
			__result = false;
			return false;
		}
	}
}
