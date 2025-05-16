using System;
using HarmonyLib;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;
using mouahrarasModuleCollection.Shops.GeodesAutoProcess.Utilities;

namespace mouahrarasModuleCollection.Shops.GeodesAutoProcess.Patches
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
			if (!Context.IsWorldReady || !ModEntry.Config.ShopsGeodesAutoProcess || __instance is not GeodeMenu)
				return;

			__instance.allClickableComponents.Add(GeodeMenuPatch.stopButton);
		}

		private static bool ReceiveKeyPressPrefix(IClickableMenu __instance, Keys key)
		{
			if (!Context.IsWorldReady || !ModEntry.Config.ShopsGeodesAutoProcess || key == 0 || __instance is not GeodeMenu geodeMenu)
				return true;

			if (Game1.options.doesInputListContain(Game1.options.menuButton, key) && !geodeMenu.readyToClose())
			{
				GeodesAutoProcessUtility.EndGeodeProcessing();
				return false;
			}
			return true;
		}

		private static void ExitThisMenuPostfix(IClickableMenu __instance)
		{
			if (!Context.IsWorldReady || !ModEntry.Config.ShopsGeodesAutoProcess || __instance is not GeodeMenu)
				return;

			if (GeodesAutoProcessUtility.FoundArtifact is not null)
			{
				Game1.player.holdUpItemThenMessage(GeodesAutoProcessUtility.FoundArtifact);
			}
			GeodesAutoProcessUtility.CleanBeforeClosingGeodeMenu();
		}
	}
}
