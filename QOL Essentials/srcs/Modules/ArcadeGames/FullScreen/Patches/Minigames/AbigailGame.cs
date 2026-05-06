using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.BellsAndWhistles;
using StardewValley.Minigames;

namespace QOLEssentials.ArcadeGames.FullScreen.Patches
{
	internal class AbigailGamePatch
	{
		private const int							windowWidthidth = 384;
		private const int							windowHeighteight = 384;
		private static readonly PerScreen<Vector2>	newTopLeft = new(() => Vector2.Zero);
		private static readonly PerScreen<float>	pixelScale = new(() => 1f);
		private static readonly PerScreen<Vector2>	previousVanillaTopLeft = new(() => Vector2.Zero);
		private static readonly PerScreen<float>	previousViewportHeight = new(() => 0f);

		internal static Vector2 NewTopLeft
		{
			get => newTopLeft.Value;
			set => newTopLeft.Value = value;
		}

		internal static float PixelScale
		{
			get => pixelScale.Value;
			set => pixelScale.Value = value;
		}

		private static Vector2 PreviousVanillaTopLeft
		{
			get => previousVanillaTopLeft.Value;
			set => previousVanillaTopLeft.Value = value;
		}

		internal static void Apply(Harmony harmony)
		{
			harmony.Patch(
				original: AccessTools.Constructor(typeof(AbigailGame), new Type[] { typeof(NPC) }),
				postfix: new HarmonyMethod(typeof(AbigailGamePatch), nameof(AbigailGamePostfix))
			);
			harmony.Patch(
				original: AccessTools.Constructor(typeof(AbigailGame), new Type[] { typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(bool), typeof(int) }),
				postfix: new HarmonyMethod(typeof(AbigailGamePatch), nameof(AbigailGamePostfix))
			);
			harmony.Patch(
				original: AccessTools.Method(typeof(AbigailGame), nameof(AbigailGame.ApplyLevelSpecificStates)),
				postfix: new HarmonyMethod(typeof(AbigailGamePatch), nameof(ApplyLevelSpecificStatesPostfix))
			);
			harmony.Patch(
				original: AccessTools.Method(typeof(AbigailGame), nameof(AbigailGame.changeScreenSize)),
				prefix: new HarmonyMethod(typeof(AbigailGamePatch), nameof(ChangeScreenSizePrefix))
			);
			harmony.Patch(
				original: AccessTools.Method(typeof(AbigailGame), nameof(AbigailGame.draw), new Type[] { typeof(SpriteBatch) }),
				prefix: new HarmonyMethod(typeof(AbigailGamePatch), nameof(DrawPrefix))
			);
		}

		private static void AbigailGamePostfix(AbigailGame __instance)
		{
			if (!ModEntry.Config.ArcadeGamesFullScreen)
				return;

			__instance.changeScreenSize();
		}

		private static void ApplyLevelSpecificStatesPostfix()
		{
			if (!ModEntry.Config.ArcadeGamesFullScreen)
				return;

			foreach (AbigailGame.CowboyMonster monster in AbigailGame.monsters)
			{
				if (monster is AbigailGame.Dracula dracula)
				{
					dracula.fullHealth = dracula.health;
				}
			}
		}

		private static bool ChangeScreenSizePrefix(AbigailGame __instance)
		{
			if (!ModEntry.Config.ArcadeGamesFullScreen)
				return true;

			float viewportWidth = Game1.game1.localMultiplayerWindow.Width / Game1.options.zoomLevel;
			float viewportHeight = Game1.game1.localMultiplayerWindow.Height / Game1.options.zoomLevel;
			Vector2 newVanillaTopLeft = new(viewportWidth / 2f - 384f, viewportHeight / 2f - 384f);
			Vector2 delta = newVanillaTopLeft - PreviousVanillaTopLeft;

			foreach (TemporaryAnimatedSprite temporarySprite in AbigailGame.temporarySprites)
			{
				temporarySprite.position += delta;
			}
			PreviousVanillaTopLeft = newVanillaTopLeft;
			AbigailGame.topLeftScreenCoordinate = newVanillaTopLeft;
			PixelScale = Math.Min(viewportWidth / (windowWidthidth + 96f), viewportHeight / (windowHeighteight + 36f)) * ModEntry.Config.ArcadeGamesFullScreenFillRatio;
			NewTopLeft = new Vector2((viewportWidth - windowWidthidth * PixelScale) / 2f, (viewportHeight - windowHeighteight * PixelScale) / 2f);
			if (previousViewportHeight.Value > 0 && __instance.abigailPortraitTimer > 0)
			{
				float offsetFromBottom = previousViewportHeight.Value - __instance.abigailPortraitYposition;

				__instance.abigailPortraitYposition = (int)(viewportHeight - offsetFromBottom);
			}
			previousViewportHeight.Value = viewportHeight;
			return false;
		}

		private static bool DrawPrefix(AbigailGame __instance, SpriteBatch b)
		{
			if (!ModEntry.Config.ArcadeGamesFullScreen)
				return true;

			float drawScale = PixelScale / 2f;
			float translationX = NewTopLeft.X - AbigailGame.topLeftScreenCoordinate.X * drawScale;
			float translationY = NewTopLeft.Y - AbigailGame.topLeftScreenCoordinate.Y * drawScale;
			Matrix transformMatrix = Matrix.CreateScale(drawScale) * Matrix.CreateTranslation(translationX, translationY, 0f);

			b.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, transformMatrix);
			if (AbigailGame.onStartMenu)
			{
				b.Draw(Game1.staminaRect, new Rectangle((int)AbigailGame.topLeftScreenCoordinate.X, (int)AbigailGame.topLeftScreenCoordinate.Y, 16 * AbigailGame.TileSize, 16 * AbigailGame.TileSize), Game1.staminaRect.Bounds, Color.Black, 0f, Vector2.Zero, SpriteEffects.None, 0.97f);
				b.Draw(Game1.mouseCursors, new Vector2(Game1.viewport.Width / 2 - 3 * AbigailGame.TileSize, AbigailGame.topLeftScreenCoordinate.Y + 5f * AbigailGame.TileSize), new Rectangle(128, 1744, 96, 56), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 1f);
			}
			else if ((AbigailGame.gameOver || __instance.gamerestartTimer > 0) && !AbigailGame.endCutscene)
			{
				string gameOver = Game1.content.LoadString("Strings\\StringsFromCSFiles:AbigailGame.cs.11914");
				string restart = Game1.content.LoadString("Strings\\StringsFromCSFiles:AbigailGame.cs.11917");
				string quit = Game1.content.LoadString("Strings\\StringsFromCSFiles:AbigailGame.cs.11919");

				b.Draw(Game1.staminaRect, new Rectangle((int)AbigailGame.topLeftScreenCoordinate.X, (int)AbigailGame.topLeftScreenCoordinate.Y, 16 * AbigailGame.TileSize, 16 * AbigailGame.TileSize), Game1.staminaRect.Bounds, Color.Black, 0f, Vector2.Zero, SpriteEffects.None, 0.0001f);
				b.DrawString(Game1.dialogueFont, gameOver, AbigailGame.topLeftScreenCoordinate + new Vector2(6f, 7f) * AbigailGame.TileSize, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
				b.DrawString(Game1.dialogueFont, gameOver, AbigailGame.topLeftScreenCoordinate + new Vector2(6f, 7f) * AbigailGame.TileSize + new Vector2(-1f, 0f), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
				b.DrawString(Game1.dialogueFont, gameOver, AbigailGame.topLeftScreenCoordinate + new Vector2(6f, 7f) * AbigailGame.TileSize + new Vector2(1f, 0f), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
				if (__instance.gameOverOption == 0)
				{
					restart = "> " + restart;
				}
				if (__instance.gameOverOption == 1)
				{
					quit = "> " + quit;
				}
				if (__instance.gamerestartTimer <= 0 || __instance.gamerestartTimer / 500 % 2 == 0)
				{
					b.DrawString(Game1.smallFont, restart, AbigailGame.topLeftScreenCoordinate + new Vector2(6f, 9f) * AbigailGame.TileSize, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
				}
				b.DrawString(Game1.smallFont, quit, AbigailGame.topLeftScreenCoordinate + new Vector2(6f, 9f) * AbigailGame.TileSize + new Vector2(0f, AbigailGame.TileSize * 2 / 3), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
			}
			else if (AbigailGame.endCutscene)
			{
				switch (AbigailGame.endCutscenePhase)
				{
					case 0:
						b.Draw(Game1.staminaRect, new Rectangle((int)AbigailGame.topLeftScreenCoordinate.X, (int)AbigailGame.topLeftScreenCoordinate.Y, 16 * AbigailGame.TileSize, 16 * AbigailGame.TileSize), Game1.staminaRect.Bounds, Color.Black, 0f, Vector2.Zero, SpriteEffects.None, 0.0001f);
						b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + __instance.playerPosition + new Vector2(0f, -AbigailGame.TileSize / 4), new Rectangle(384, 1760, 16, 16), Color.White * ((AbigailGame.endCutsceneTimer < 2000) ? (AbigailGame.endCutsceneTimer / 2000f) : 1f), 0f, Vector2.Zero, 3f, SpriteEffects.None, __instance.playerPosition.Y / 10000f + 0.001f);
						b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + __instance.playerPosition + new Vector2(0f, -AbigailGame.TileSize * 2 / 3) + new Vector2(0f, -AbigailGame.TileSize / 4), new Rectangle(320 + AbigailGame.itemToHold * 16, 1776, 16, 16), Color.White * ((AbigailGame.endCutsceneTimer < 2000) ? (AbigailGame.endCutsceneTimer / 2000f) : 1f), 0f, Vector2.Zero, 3f, SpriteEffects.None, __instance.playerPosition.Y / 10000f + 0.002f);
						break;
					case 4:
					case 5:
						b.Draw(Game1.staminaRect, new Rectangle((int)AbigailGame.topLeftScreenCoordinate.X, (int)AbigailGame.topLeftScreenCoordinate.Y, 16 * AbigailGame.TileSize, 16 * AbigailGame.TileSize), Game1.staminaRect.Bounds, Color.Black, 0f, Vector2.Zero, SpriteEffects.None, 0.97f);
						b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2(6 * AbigailGame.TileSize, 3 * AbigailGame.TileSize), new Rectangle(224, 1744, 64, 48), Color.White * ((AbigailGame.endCutsceneTimer > 0) ? (1f - ((float)AbigailGame.endCutsceneTimer - 2000f) / 2000f) : 1f), 0f, Vector2.Zero, 3f, SpriteEffects.None, 1f);
						if (AbigailGame.endCutscenePhase == 5 && __instance.gamerestartTimer <= 0)
							b.DrawString(Game1.smallFont, Game1.content.LoadString("Strings\\Locations:Saloon_Arcade_PK_NewGame+"), AbigailGame.topLeftScreenCoordinate + new Vector2(3f, 10f) * AbigailGame.TileSize, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
						break;
					case 1:
					case 2:
					case 3:
					{
						for (int i = 0; i < 16; i++)
						{
							for (int j = 0; j < 16; j++)
							{
								b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2(i, j) * 16f * 3f + new Vector2(0f, AbigailGame.newMapPosition - 16 * AbigailGame.TileSize), new Rectangle(464 + 16 * AbigailGame.map[i, j] + ((AbigailGame.map[i, j] == 5 && __instance.cactusDanceTimer > 800f) ? 16 : 0), 1680 - AbigailGame.world * 16, 16, 16), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0f);
							}
						}
						b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2(6 * AbigailGame.TileSize, 3 * AbigailGame.TileSize), new Rectangle(288, 1697, 64, 80), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.01f);
						if (AbigailGame.endCutscenePhase == 3)
						{
							b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2(9 * AbigailGame.TileSize, 7 * AbigailGame.TileSize), new Rectangle(544, 1792, 32, 32), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.05f);
							if (AbigailGame.endCutsceneTimer < 3000)
							{
								b.Draw(Game1.staminaRect, new Rectangle((int)AbigailGame.topLeftScreenCoordinate.X, (int)AbigailGame.topLeftScreenCoordinate.Y, 16 * AbigailGame.TileSize, 16 * AbigailGame.TileSize), Game1.staminaRect.Bounds, Color.Black * (1f - (float)AbigailGame.endCutsceneTimer / 3000f), 0f, Vector2.Zero, SpriteEffects.None, 1f);
							}
							break;
						}
						b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2(10 * AbigailGame.TileSize, 8 * AbigailGame.TileSize), new Rectangle(272 - AbigailGame.endCutsceneTimer / 300 % 4 * 16, 1792, 16, 16), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.02f);
						if (AbigailGame.endCutscenePhase == 2)
						{
							b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + __instance.playerPosition + new Vector2(4f, 13f) * 3f, new Rectangle(484, 1760 + (int)(__instance.playerMotionAnimationTimer / 100f) * 3, 8, 3), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, __instance.playerPosition.Y / 10000f + 0.002f);
							b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + __instance.playerPosition, new Rectangle(384, 1760, 16, 13), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, __instance.playerPosition.Y / 10000f + 0.003f);
							b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + __instance.playerPosition + new Vector2(0f, -AbigailGame.TileSize * 2 / 3 - AbigailGame.TileSize / 4), new Rectangle(320 + AbigailGame.itemToHold * 16, 1776, 16, 16), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, __instance.playerPosition.Y / 10000f + 0.005f);
						}
						b.Draw(Game1.staminaRect, new Rectangle((int)AbigailGame.topLeftScreenCoordinate.X, (int)AbigailGame.topLeftScreenCoordinate.Y, 16 * AbigailGame.TileSize, 16 * AbigailGame.TileSize), Game1.staminaRect.Bounds, Color.Black * ((AbigailGame.endCutscenePhase == 1 && AbigailGame.endCutsceneTimer > 12500) ? ((float)(AbigailGame.endCutsceneTimer - 12500) / 3000f) : 0f), 0f, Vector2.Zero, SpriteEffects.None, 1f);
						break;
					}
				}
			}
			else
			{
				if (AbigailGame.zombieModeTimer > 8200)
				{
					b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + __instance.playerPosition, new Rectangle(384 + ((AbigailGame.zombieModeTimer / 200 % 2 == 0) ? 16 : 0), 1760, 16, 16), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 1f);
					for (int num = (int)(__instance.playerPosition.Y - AbigailGame.TileSize); num > -AbigailGame.TileSize; num -= AbigailGame.TileSize)
					{
						b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2(__instance.playerPosition.X, num), new Rectangle(368 + ((num / AbigailGame.TileSize % 3 == 0) ? 16 : 0), 1744, 16, 16), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 1f);
					}
					b.End();
					return false;
				}
				for (int k = 0; k < 16; k++)
				{
					for (int l = 0; l < 16; l++)
					{
						b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2(k, l) * 16f * 3f + new Vector2(0f, AbigailGame.newMapPosition - 16 * AbigailGame.TileSize), new Rectangle(464 + 16 * AbigailGame.map[k, l] + ((AbigailGame.map[k, l] == 5 && __instance.cactusDanceTimer > 800f) ? 16 : 0), 1680 - AbigailGame.world * 16, 16, 16), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0f);
					}
				}
				if (AbigailGame.scrollingMap)
				{
					for (int m = 0; m < 16; m++)
					{
						for (int n = 0; n < 16; n++)
						{
							b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2(m, n) * 16f * 3f + new Vector2(0f, AbigailGame.newMapPosition), new Rectangle(464 + 16 * AbigailGame.nextMap[m, n] + ((AbigailGame.nextMap[m, n] == 5 && __instance.cactusDanceTimer > 800f) ? 16 : 0), 1680 - AbigailGame.world * 16, 16, 16), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0f);
						}
					}
					b.Draw(Game1.staminaRect, new Rectangle((int)AbigailGame.topLeftScreenCoordinate.X, -1, 16 * AbigailGame.TileSize, (int)AbigailGame.topLeftScreenCoordinate.Y + 1), Game1.staminaRect.Bounds, Color.Black, 0f, Vector2.Zero, SpriteEffects.None, 1f);
					b.Draw(Game1.staminaRect, new Rectangle((int)AbigailGame.topLeftScreenCoordinate.X, (int)AbigailGame.topLeftScreenCoordinate.Y + 16 * AbigailGame.TileSize, 16 * AbigailGame.TileSize, (int)AbigailGame.topLeftScreenCoordinate.Y + 2), Game1.staminaRect.Bounds, Color.Black, 0f, Vector2.Zero, SpriteEffects.None, 1f);
				}
				if (AbigailGame.deathTimer <= 0f && (AbigailGame.playerInvincibleTimer <= 0 || AbigailGame.playerInvincibleTimer / 100 % 2 == 0))
				{
					if (AbigailGame.holdItemTimer > 0)
					{
						b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + __instance.playerPosition + new Vector2(0f, -AbigailGame.TileSize / 4), new Rectangle(384, 1760, 16, 16), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, __instance.playerPosition.Y / 10000f + 0.001f);
						b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + __instance.playerPosition + new Vector2(0f, -AbigailGame.TileSize * 2 / 3) + new Vector2(0f, -AbigailGame.TileSize / 4), new Rectangle(320 + AbigailGame.itemToHold * 16, 1776, 16, 16), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, __instance.playerPosition.Y / 10000f + 0.002f);
					}
					else if (AbigailGame.zombieModeTimer > 0)
					{
						b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + __instance.playerPosition + new Vector2(0f, -AbigailGame.TileSize / 4), new Rectangle(352 + ((AbigailGame.zombieModeTimer / 50 % 2 == 0) ? 16 : 0), 1760, 16, 16), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, __instance.playerPosition.Y / 10000f + 0.001f);
					}
					else if (AbigailGame.playerMovementDirections.Count == 0 && AbigailGame.playerShootingDirections.Count == 0)
					{
						b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + __instance.playerPosition + new Vector2(0f, -AbigailGame.TileSize / 4), new Rectangle(496, 1760, 16, 16), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, __instance.playerPosition.Y / 10000f + 0.001f);
					}
					else
					{
						int playerFacingDirection = (AbigailGame.playerShootingDirections.Count == 0) ? AbigailGame.playerMovementDirections.ElementAt(0) : AbigailGame.playerShootingDirections.Last();

						b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + __instance.playerPosition + new Vector2(0f, -AbigailGame.TileSize / 4) + new Vector2(4f, 13f) * 3f, new Rectangle(483, 1760 + (int)(__instance.playerMotionAnimationTimer / 100f) * 3, 10, 3), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, __instance.playerPosition.Y / 10000f + 0.002f);
						b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + __instance.playerPosition + new Vector2(3f, -AbigailGame.TileSize / 4), new Rectangle(464 + playerFacingDirection * 16, 1744, 16, 16), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, __instance.playerPosition.Y / 10000f + 0.003f);
					}
				}
				if (AbigailGame.playingWithAbigail && __instance.player2deathtimer <= 0 && (__instance.player2invincibletimer <= 0 || __instance.player2invincibletimer / 100 % 2 == 0))
				{
					if (__instance.player2MovementDirections.Count == 0 && __instance.player2ShootingDirections.Count == 0)
					{
						b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + AbigailGame.player2Position + new Vector2(0f, -AbigailGame.TileSize / 4), new Rectangle(256, 1728, 16, 16), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, AbigailGame.player2Position.Y / 10000f + 0.001f);
					}
					else
					{
						int player2FacingDirection = (__instance.player2ShootingDirections.Count == 0) ? __instance.player2MovementDirections[0] : __instance.player2ShootingDirections[0];

						b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + AbigailGame.player2Position + new Vector2(0f, -AbigailGame.TileSize / 4) + new Vector2(4f, 13f) * 3f, new Rectangle(243, 1728 + __instance.player2AnimationTimer / 100 * 3, 10, 3), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, AbigailGame.player2Position.Y / 10000f + 0.002f);
						b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + AbigailGame.player2Position + new Vector2(0f, -AbigailGame.TileSize / 4), new Rectangle(224 + player2FacingDirection * 16, 1712, 16, 16), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, AbigailGame.player2Position.Y / 10000f + 0.003f);
					}
				}
				foreach (TemporaryAnimatedSprite temporarySprite in AbigailGame.temporarySprites)
				{
					temporarySprite.draw(b, localPosition: true);
				}
				foreach (AbigailGame.CowboyPowerup powerup in AbigailGame.powerups)
				{
					powerup.draw(b);
				}
				foreach (AbigailGame.CowboyBullet bullet in __instance.bullets)
				{
					b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2(bullet.position.X, bullet.position.Y), new Rectangle(518, 1760 + (__instance.bulletDamage - 1) * 4, 4, 4), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.9f);
				}
				foreach (AbigailGame.CowboyBullet enemyBullet in AbigailGame.enemyBullets)
				{
					b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2(enemyBullet.position.X, enemyBullet.position.Y), new Rectangle(523, 1760, 5, 5), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.9f);
				}
				if (AbigailGame.shopping)
				{
					if ((AbigailGame.merchantArriving || AbigailGame.merchantLeaving) && !AbigailGame.merchantShopOpen)
					{
						b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2(__instance.merchantBox.Location.X, __instance.merchantBox.Location.Y), new Rectangle(464 + ((AbigailGame.shoppingTimer / 100 % 2 == 0) ? 16 : 0), 1728, 16, 16), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, (float)__instance.merchantBox.Y / 10000f + 0.001f);
					}
					else
					{
						int merchantFacingDirection = (__instance.playerBoundingBox.X - __instance.merchantBox.X > AbigailGame.TileSize) ? 2 : ((__instance.merchantBox.X - __instance.playerBoundingBox.X > AbigailGame.TileSize) ? 1 : 0);

						b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2(__instance.merchantBox.Location.X, __instance.merchantBox.Location.Y), new Rectangle(496 + merchantFacingDirection * 16, 1728, 16, 16), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, (float)__instance.merchantBox.Y / 10000f + 0.001f);
						b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2(__instance.merchantBox.Location.X - AbigailGame.TileSize, __instance.merchantBox.Location.Y + AbigailGame.TileSize), new Rectangle(529, 1744, 63, 32), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, (float)__instance.merchantBox.Y / 10000f + 0.001f);
						foreach (KeyValuePair<Rectangle, int> storeItem in __instance.storeItems)
						{
							string price = __instance.getPriceForItem(storeItem.Value).ToString() ?? "";
							float priceOffsetX = (float)(storeItem.Key.Location.X + AbigailGame.TileSize / 2) - Game1.smallFont.MeasureString(price).X / 2f;

							b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2(storeItem.Key.Location.X, storeItem.Key.Location.Y), new Rectangle(320 + storeItem.Value * 16, 1776, 16, 16), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, (float)storeItem.Key.Location.Y / 10000f);
							b.DrawString(Game1.smallFont, price, AbigailGame.topLeftScreenCoordinate + new Vector2(priceOffsetX, storeItem.Key.Location.Y + AbigailGame.TileSize + 3), new Color(88, 29, 43), 0f, Vector2.Zero, 1f, SpriteEffects.None, (float)storeItem.Key.Location.Y / 10000f + 0.002f);
							b.DrawString(Game1.smallFont, price, AbigailGame.topLeftScreenCoordinate + new Vector2(priceOffsetX - 1f, storeItem.Key.Location.Y + AbigailGame.TileSize + 3), new Color(88, 29, 43), 0f, Vector2.Zero, 1f, SpriteEffects.None, (float)storeItem.Key.Location.Y / 10000f + 0.002f);
							b.DrawString(Game1.smallFont, price, AbigailGame.topLeftScreenCoordinate + new Vector2(priceOffsetX + 1f, storeItem.Key.Location.Y + AbigailGame.TileSize + 3), new Color(88, 29, 43), 0f, Vector2.Zero, 1f, SpriteEffects.None, (float)storeItem.Key.Location.Y / 10000f + 0.002f);
						}
					}
				}
				if (AbigailGame.waitingForPlayerToMoveDownAMap && (AbigailGame.merchantShopOpen || AbigailGame.merchantLeaving || !AbigailGame.shopping) && AbigailGame.shoppingTimer < 250)
				{
					b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2(8.5f, 15f) * AbigailGame.TileSize + new Vector2(-12f, 0f), new Rectangle(355, 1750, 8, 8), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.001f);
				}
				foreach (AbigailGame.CowboyMonster monster in AbigailGame.monsters)
				{
					monster.draw(b);
				}
				if (AbigailGame.gopherRunning)
				{
					b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2(AbigailGame.gopherBox.X, AbigailGame.gopherBox.Y), new Rectangle(320 + AbigailGame.waveTimer / 100 % 4 * 16, 1792, 16, 16), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, (float)AbigailGame.gopherBox.Y / 10000f + 0.001f);
				}
				if (AbigailGame.gopherTrain && AbigailGame.gopherTrainPosition > -AbigailGame.TileSize)
				{
					b.Draw(Game1.staminaRect, new Rectangle((int)AbigailGame.topLeftScreenCoordinate.X, (int)AbigailGame.topLeftScreenCoordinate.Y, 16 * AbigailGame.TileSize, 16 * AbigailGame.TileSize), Game1.staminaRect.Bounds, Color.Black, 0f, Vector2.Zero, SpriteEffects.None, 0.95f);
					b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2(__instance.playerPosition.X - AbigailGame.TileSize / 2f, AbigailGame.gopherTrainPosition), new Rectangle(384 + AbigailGame.gopherTrainPosition / 30 % 4 * 16, 1792, 16, 16), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.96f);
					b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2(__instance.playerPosition.X + AbigailGame.TileSize / 2f, AbigailGame.gopherTrainPosition), new Rectangle(384 + AbigailGame.gopherTrainPosition / 30 % 4 * 16, 1792, 16, 16), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.96f);
					b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2(__instance.playerPosition.X, AbigailGame.gopherTrainPosition - AbigailGame.TileSize * 3), new Rectangle(320 + AbigailGame.gopherTrainPosition / 30 % 4 * 16, 1792, 16, 16), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.96f);
					b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2(__instance.playerPosition.X - AbigailGame.TileSize / 2f, AbigailGame.gopherTrainPosition - AbigailGame.TileSize), new Rectangle(400, 1728, 32, 32), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.97f);
					if (AbigailGame.holdItemTimer > 0)
					{
						b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + __instance.playerPosition + new Vector2(0f, -AbigailGame.TileSize / 4), new Rectangle(384, 1760, 16, 16), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.98f);
						b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + __instance.playerPosition + new Vector2(0f, -AbigailGame.TileSize * 2 / 3) + new Vector2(0f, -AbigailGame.TileSize / 4), new Rectangle(320 + AbigailGame.itemToHold * 16, 1776, 16, 16), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.99f);
					}
					else
					{
						b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + __instance.playerPosition + new Vector2(0f, -AbigailGame.TileSize / 4), new Rectangle(464, 1760, 16, 16), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.98f);
					}
				}
				else
				{
					b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate - new Vector2(AbigailGame.TileSize + 27, 0f), new Rectangle(294, 1782, 22, 22), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.25f);
					if (__instance.heldItem != null)
					{
						b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate - new Vector2(AbigailGame.TileSize + 18, -9f), new Rectangle(272 + __instance.heldItem.which * 16, 1808, 16, 16), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.5f);
					}
					b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate - new Vector2(AbigailGame.TileSize * 2, -AbigailGame.TileSize - 18), new Rectangle(400, 1776, 16, 16), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.5f);
					b.DrawString(Game1.smallFont, "x" + Math.Max(__instance.lives, 0), AbigailGame.topLeftScreenCoordinate - new Vector2(AbigailGame.TileSize, -AbigailGame.TileSize - AbigailGame.TileSize / 4 - 18), Color.White);
					b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate - new Vector2(AbigailGame.TileSize * 2, -AbigailGame.TileSize * 2 - 18), new Rectangle(272, 1808, 16, 16), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.5f);
					b.DrawString(Game1.smallFont, "x" + __instance.coins, AbigailGame.topLeftScreenCoordinate - new Vector2(AbigailGame.TileSize, -AbigailGame.TileSize * 2 - AbigailGame.TileSize / 4 - 18), Color.White);
					for (int waveDotIndex = 0; waveDotIndex < AbigailGame.whichWave + __instance.whichRound * 12; waveDotIndex++)
					{
						b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2(AbigailGame.TileSize * 16 + 3 + waveDotIndex / 43 * 18, waveDotIndex % 43 * 18), new Rectangle(512, 1760, 5, 5), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.5f);
					}
					b.Draw(Game1.mouseCursors, new Vector2((int)AbigailGame.topLeftScreenCoordinate.X, (int)AbigailGame.topLeftScreenCoordinate.Y - AbigailGame.TileSize / 2 - 12), new Rectangle(595, 1748, 9, 11), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.5f);
					if (!AbigailGame.shootoutLevel)
					{
						b.Draw(Game1.staminaRect, new Rectangle((int)AbigailGame.topLeftScreenCoordinate.X + 30, (int)AbigailGame.topLeftScreenCoordinate.Y - AbigailGame.TileSize / 2 + 3, (int)((16 * AbigailGame.TileSize - 30) * (AbigailGame.waveTimer / 80000f)), AbigailGame.TileSize / 4), Game1.staminaRect.Bounds, (AbigailGame.waveTimer < 8000) ? new Color(188, 51, 74) : new Color(147, 177, 38), 0f, Vector2.Zero, SpriteEffects.None, 0.5f);
					}
					if (AbigailGame.betweenWaveTimer > 0 && AbigailGame.whichWave == 0 && !AbigailGame.scrollingMap)
					{
						float hintKeyboardWidth = 80 * 3f * drawScale;
						float hintKeyboardHeight = 48 * 3f * drawScale;
						Vector2 hintPos = new(NewTopLeft.X + windowWidthidth * PixelScale / 2f - hintKeyboardWidth / 2f, NewTopLeft.Y + windowHeighteight * PixelScale - hintKeyboardHeight - 3f * drawScale);

						b.End();
						b.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp);
						if (!Game1.options.gamepadControls)
						{
							b.Draw(Game1.mouseCursors, hintPos, new Rectangle(352, 1648, 80, 48), Color.White, 0f, Vector2.Zero, 3f * drawScale, SpriteEffects.None, 0.99f);
						}
						else
						{
							b.Draw(Game1.controllerMaps, hintPos, Utility.controllerMapSourceRect(new Rectangle(681, 157, 160, 96)), Color.White, 0f, Vector2.Zero, 1.5f * drawScale, SpriteEffects.None, 0.99f);
						}
						b.End();
						b.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, transformMatrix);
					}
					if (__instance.bulletDamage > 1)
					{
						b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2(-AbigailGame.TileSize - 3, 16 * AbigailGame.TileSize - AbigailGame.TileSize), new Rectangle(416 + (__instance.ammoLevel - 1) * 16, 1776, 16, 16), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.5f);
					}
					if (__instance.fireSpeedLevel > 0)
					{
						b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2(-AbigailGame.TileSize - 3, 16 * AbigailGame.TileSize - AbigailGame.TileSize * 2), new Rectangle(320 + (__instance.fireSpeedLevel - 1) * 16, 1776, 16, 16), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.5f);
					}
					if (__instance.runSpeedLevel > 0)
					{
						b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2(-AbigailGame.TileSize - 3, 16 * AbigailGame.TileSize - AbigailGame.TileSize * 3), new Rectangle(368 + (__instance.runSpeedLevel - 1) * 16, 1776, 16, 16), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.5f);
					}
					if (__instance.spreadPistol)
					{
						b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2(-AbigailGame.TileSize - 3, 16 * AbigailGame.TileSize - AbigailGame.TileSize * 4), new Rectangle(464, 1776, 16, 16), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.5f);
					}
				}
				if (AbigailGame.screenFlash > 0)
				{
					b.Draw(Game1.staminaRect, new Rectangle((int)AbigailGame.topLeftScreenCoordinate.X, (int)AbigailGame.topLeftScreenCoordinate.Y, 16 * AbigailGame.TileSize, 16 * AbigailGame.TileSize), Game1.staminaRect.Bounds, new Color(255, 214, 168), 0f, Vector2.Zero, SpriteEffects.None, 1f);
				}
			}
			b.End();
			if (__instance.fadethenQuitTimer > 0)
			{
				b.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp);
				b.Draw(Game1.staminaRect, new Rectangle(0, 0, Game1.viewport.Width, Game1.viewport.Height), Game1.staminaRect.Bounds, Color.Black * (1f - __instance.fadethenQuitTimer / 2000f), 0f, Vector2.Zero, SpriteEffects.None, 1f);
				b.End();
			}
			if (__instance.abigailPortraitTimer > 0)
			{
				float viewportWidth = Game1.game1.localMultiplayerWindow.Width / Game1.options.zoomLevel;
				float viewportHeight = Game1.game1.localMultiplayerWindow.Height / Game1.options.zoomLevel;
				float vanillaPortraitGameX = AbigailGame.topLeftScreenCoordinate.X + 16f * AbigailGame.TileSize;
				float portraitScreenRight = (vanillaPortraitGameX + 256f) * drawScale + translationX;
				float xOverflow = Math.Max(0f, portraitScreenRight - viewportWidth);
				float portraitGameX = vanillaPortraitGameX - xOverflow / drawScale;
				float rawOffset = viewportHeight - __instance.abigailPortraitYposition;
				float portraitTopScreen = viewportHeight - rawOffset * drawScale;
				float portraitGameY = (portraitTopScreen - translationY) / drawScale;

				b.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, transformMatrix);
				b.Draw(__instance.abigail.Portrait, new Vector2((int)portraitGameX, (int)portraitGameY), new Rectangle(64 * (__instance.abigailPortraitExpression % 2), 64 * (__instance.abigailPortraitExpression / 2), 64, 64), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
				b.End();
				if (__instance.abigailPortraitTimer < 5500 && __instance.abigailPortraitTimer > 500)
				{
					float portraitScreenX = portraitGameX * drawScale + translationX;
					int widthOfString = SpriteText.getWidthOfString("0" + __instance.AbigailDialogue + "0");
					float screenOverflow = Math.Max(0f, portraitScreenX + widthOfString * drawScale - viewportWidth);
					float textScreenX = Math.Max(0f, portraitScreenX - screenOverflow);
					float textScreenY = portraitTopScreen - 80f * drawScale;
					int yPassIn = SpriteText.FontPixelZoom < 4f ? -(int)((4f - SpriteText.FontPixelZoom) * 4f) : 0;
					Matrix textTransform = Matrix.CreateScale(drawScale) * Matrix.CreateTranslation(textScreenX, textScreenY, 0f);

					b.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, textTransform);
					SpriteText.drawString(b, __instance.AbigailDialogue, 0, yPassIn, 999999, widthOfString, 999999, 1f, 0.88f, junimoText: false, -1, "", SpriteText.color_Purple);
					b.End();
				}
			}
			b.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp);
			if (Game1.IsMultiplayer)
			{
				string timeStr = Game1.getTimeOfDayString(Game1.timeOfDay);
				Vector2 clockPos = new(Game1.viewport.Width - Game1.dialogueFont.MeasureString(timeStr).X - 16f, 16f);

				b.DrawString(Game1.dialogueFont, timeStr, clockPos, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.01f);
				foreach (Vector2 outlineOffset in new[] { new Vector2(-3f, -3f), new Vector2(-2f, -2f), new Vector2(-1f, -1f), new Vector2(-3.5f, -3.5f), new Vector2(-1.5f, -1.5f), new Vector2(-2.5f, -2.5f) })
				{
					b.DrawString(Game1.dialogueFont, timeStr, clockPos + new Vector2(1f, 1f) + outlineOffset, Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.02f);
				}
			}
			b.End();
			return false;
		}
	}
}
