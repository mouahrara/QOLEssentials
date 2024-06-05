using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using HarmonyLib;
using StardewValley.Menus;
using mouahrarasModuleCollection.UserInterface.FastScrolling.Utilities;

namespace mouahrarasModuleCollection.UserInterface.FastScrolling.Patches
{
	internal class AnimalQueryMenuPatch
	{
		internal static void Apply(Harmony harmony)
		{
			harmony.Patch(
				original: AccessTools.Method(typeof(AnimalQueryMenu), nameof(AnimalQueryMenu.receiveKeyPress), new Type[] { typeof(Keys) }),
				postfix: new HarmonyMethod(typeof(MenusPatchUtility), nameof(MenusPatchUtility.ReceiveKeyPressPostfix))
			);
			harmony.Patch(
				original: AccessTools.Method(typeof(AnimalQueryMenu), nameof(AnimalQueryMenu.update), new Type[] { typeof(GameTime) }),
				postfix: new HarmonyMethod(typeof(MenusPatchUtility), nameof(MenusPatchUtility.UpdatePostfix))
			);
		}
	}
}
