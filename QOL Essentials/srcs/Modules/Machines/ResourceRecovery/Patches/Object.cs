using System.Collections.Generic;
using HarmonyLib;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.GameData.Machines;

namespace QOLEssentials.Machines.ResourceRecovery.Patches
{
	internal class ObjectPatch
	{
		private class RecoveryState
		{
			internal List<Item>	Items = new();
			internal Item		HeldObject;
		}

		internal static void Apply(Harmony harmony)
		{
			harmony.Patch(
				original: AccessTools.Method(typeof(Object), nameof(Object.performToolAction), new System.Type[] { typeof(Tool) }),
				prefix: new HarmonyMethod(typeof(ObjectPatch), nameof(PerformToolActionPrefix)),
				postfix: new HarmonyMethod(typeof(ObjectPatch), nameof(PerformToolActionPostfix))
			);
		}

		private static void PerformToolActionPrefix(Object __instance, Tool t, ref RecoveryState __state)
		{
			if (!ModEntry.Config.MachinesResourceRecovery || __instance.isTemporarilyInvisible)
				return;

			MachineData machineData = __instance.GetMachineData();

			if (machineData is null || __instance.lastInputItem.Value is null)
				return;
			if (__instance.MinutesUntilReady <= 0 && !MachineHasOutputCollectedTrigger(machineData))
				return;
			if (!MachineDataUtility.TryGetMachineOutputRule(__instance, machineData, MachineOutputTrigger.ItemPlacedInMachine, __instance.lastInputItem.Value, t?.getLastFarmerToUse() ?? Game1.player, __instance.Location, out _, out MachineOutputTriggerRule triggerRule, out _, out _))
				return;

			__state = new()
			{
				HeldObject = __instance.MinutesUntilReady > 0 ? __instance.heldObject.Value : null
			};
			if (triggerRule.RequiredCount > 0)
			{
				Item lastInputItem = __instance.lastInputItem.Value.getOne();

				lastInputItem.Stack = triggerRule.RequiredCount;
				__state.Items.Add(lastInputItem);
			}
			if (machineData.AdditionalConsumedItems is not null)
			{
				foreach (MachineItemAdditionalConsumedItems additionalConsumedItem in machineData.AdditionalConsumedItems)
				{
					if (additionalConsumedItem.RequiredCount > 0)
					{
						Item item = ItemRegistry.Create(additionalConsumedItem.ItemId, additionalConsumedItem.RequiredCount, 0, true);

						if (item is not null)
						{
							__state.Items.Add(item);
						}
					}
				}
			}
		}

		private static bool MachineHasOutputCollectedTrigger(MachineData machineData)
		{
			if (machineData.OutputRules is not null)
			{
				foreach (MachineOutputRule rule in machineData.OutputRules)
				{
					if (rule.Triggers is not null)
					{
						foreach (MachineOutputTriggerRule trigger in rule.Triggers)
						{
							if (trigger.Trigger.HasFlag(MachineOutputTrigger.OutputCollected))
							{
								return true;
							}
						}
					}
				}
			}
			return false;
		}

		private static void PerformToolActionPostfix(Object __instance, RecoveryState __state, bool __result)
		{
			if (!ModEntry.Config.MachinesResourceRecovery || !__result || __state is null || __state.Items.Count == 0)
				return;

			if (__state.HeldObject is not null)
			{
				foreach (Debris debris in __instance.Location.debris)
				{
					if (ReferenceEquals(debris.item, __state.HeldObject))
						return;
				}
			}
			foreach (Item item in __state.Items)
			{
				__instance.Location.debris.Add(new Debris(item, __instance.TileLocation * Game1.tileSize + new Vector2(Game1.tileSize / 2, Game1.tileSize / 2)));
			}
		}
	}
}
