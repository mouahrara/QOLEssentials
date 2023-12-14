using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using HarmonyLib;
using StardewValley.Menus;
using mouahrarasModuleCollection.TweaksAndFeatures.UserInterface.FastScrolling.Utilities;

namespace mouahrarasModuleCollection.TweaksAndFeatures.UserInterface.FastScrolling.Patches
{
	internal class PurchaseAnimalsMenuPatch
	{
		internal static void Apply(Harmony harmony)
		{
			harmony.Patch(
				original: AccessTools.Method(typeof(PurchaseAnimalsMenu), nameof(PurchaseAnimalsMenu.receiveKeyPress), new Type[] { typeof(Keys) }),
				postfix: new HarmonyMethod(typeof(MenusPatchUtility), nameof(MenusPatchUtility.ReceiveKeyPressPostfix))
			);
			harmony.Patch(
				original: AccessTools.Method(typeof(PurchaseAnimalsMenu), nameof(PurchaseAnimalsMenu.update), new Type[] { typeof(GameTime) }),
				postfix: new HarmonyMethod(typeof(MenusPatchUtility), nameof(MenusPatchUtility.UpdatePostfix))
			);
		}
	}
}
