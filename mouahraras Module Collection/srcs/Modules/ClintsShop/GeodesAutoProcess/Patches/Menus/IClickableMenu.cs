using System;
using HarmonyLib;
using Microsoft.Xna.Framework.Input;
using StardewValley;
using StardewValley.Menus;
using mouahrarasModuleCollection.ClintsShop.GeodesAutoProcess.Utilities;

namespace mouahrarasModuleCollection.ClintsShop.GeodesAutoProcess.Patches
{
	internal class IClickableMenuPatch
	{
		internal static void Apply(Harmony harmony)
		{
			harmony.Patch(
				original: AccessTools.Method(typeof(IClickableMenu), nameof(IClickableMenu.populateClickableComponentList)),
				postfix: new HarmonyMethod(typeof(IClickableMenuPatch), nameof(PopulateClickableComponentListPostfix))
			);
			harmony.Patch(
				original: AccessTools.Method(typeof(IClickableMenu), nameof(IClickableMenu.receiveKeyPress), new Type[] { typeof(Keys) }),
				prefix: new HarmonyMethod(typeof(IClickableMenuPatch), nameof(ReceiveKeyPressPrefix))
			);
			harmony.Patch(
				original: AccessTools.Method(typeof(IClickableMenu), nameof(IClickableMenu.exitThisMenu)),
				postfix: new HarmonyMethod(typeof(IClickableMenuPatch), nameof(ExitThisMenuPostfix))
			);
		}

		private static void PopulateClickableComponentListPostfix(IClickableMenu __instance)
		{
			if (!ModEntry.Config.ClintsShopGeodesAutoProcess)
				return;

			if (__instance.GetType() == typeof(GeodeMenu))
			{
				__instance.allClickableComponents.Add(GeodeMenuPatch.stopButton);
			}
		}

		private static bool ReceiveKeyPressPrefix(IClickableMenu __instance, Keys key)
		{
			if (!ModEntry.Config.ClintsShopGeodesAutoProcess)
				return true;
			if (key == 0)
				return true;

			if (__instance.GetType() == typeof(GeodeMenu))
			{
				if (Game1.options.doesInputListContain(Game1.options.menuButton, key) && !(__instance as GeodeMenu).readyToClose())
				{
					GeodesAutoProcessUtility.EndGeodeProcessing();
					return false;
				}
			}
			return true;
		}

		private static void ExitThisMenuPostfix(IClickableMenu __instance)
		{
			if (!ModEntry.Config.ClintsShopGeodesAutoProcess)
				return;

			if (__instance.GetType() == typeof(GeodeMenu))
			{
				if (GeodesAutoProcessUtility.GetFoundArtifact() != null)
				{
					Game1.player.holdUpItemThenMessage(GeodesAutoProcessUtility.GetFoundArtifact());
				}
				GeodesAutoProcessUtility.CleanBeforeClosingGeodeMenu();
			}
		}
	}
}
