using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using HarmonyLib;
using StardewValley.Menus;
using QOLEssentials.UserInterface.FastScrolling.Utilities;

namespace QOLEssentials.UserInterface.FastScrolling.Patches
{
	internal class CarpenterMenuPatch
	{
		internal static void Apply(Harmony harmony)
		{
			harmony.Patch(
				original: AccessTools.Method(typeof(CarpenterMenu), nameof(CarpenterMenu.receiveKeyPress), new Type[] { typeof(Keys) }),
				postfix: new HarmonyMethod(typeof(MenusPatchUtility), nameof(MenusPatchUtility.ReceiveKeyPressPostfix))
			);
			harmony.Patch(
				original: AccessTools.Method(typeof(CarpenterMenu), nameof(CarpenterMenu.update), new Type[] { typeof(GameTime) }),
				postfix: new HarmonyMethod(typeof(MenusPatchUtility), nameof(MenusPatchUtility.UpdatePostfix))
			);
		}
	}
}
