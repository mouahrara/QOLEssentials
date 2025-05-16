using System;
using System.Reflection;
using HarmonyLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;
using StardewValley.Minigames;
using mouahrarasModuleCollection.ArcadeGames.PayToPlay.Managers;
using mouahrarasModuleCollection.ArcadeGames.PayToPlay.Utilities;
using mouahrarasModuleCollection.Utilities;

namespace mouahrarasModuleCollection.ArcadeGames.PayToPlay.Patches
{
	internal class AbigailGamePatch
	{
		internal static ClickableTextureComponent	buttonExit;

		internal static void Apply(Harmony harmony)
		{
			if (Constants.TargetPlatform == GamePlatform.Android)
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
					original: AccessTools.Method(typeof(AbigailGame), nameof(AbigailGame.draw), new Type[] { typeof(SpriteBatch) }),
					postfix: new HarmonyMethod(typeof(AbigailGamePatch), nameof(DrawPostfixButtonExit))
				);
				harmony.Patch(
					original: AccessTools.Method(typeof(AbigailGame), nameof(AbigailGame.receiveLeftClick), new Type[] { typeof(int), typeof(int), typeof(bool) }),
					postfix: new HarmonyMethod(typeof(AbigailGamePatch), nameof(ReceiveLeftClickPostfix))
				);
			}
			harmony.Patch(
				original: AccessTools.Method(typeof(AbigailGame), nameof(AbigailGame.draw), new Type[] { typeof(SpriteBatch) }),
				postfix: new HarmonyMethod(typeof(AbigailGamePatch), nameof(DrawPostfix))
			);
			harmony.Patch(
				original: AccessTools.Method(typeof(AbigailGame), nameof(AbigailGame.tick), new Type[] { typeof(GameTime) }),
				prefix: new HarmonyMethod(typeof(AbigailGamePatch), nameof(TickPrefix)),
				postfix: new HarmonyMethod(typeof(AbigailGamePatch), nameof(TickPostfix))
			);
			harmony.Patch(
				original: AccessTools.Method(typeof(AbigailGame), nameof(AbigailGame.receiveKeyPress), new Type[] { typeof(Keys) }),
				postfix: new HarmonyMethod(typeof(AbigailGamePatch), nameof(ReceiveKeyPressPostfix))
			);
			harmony.Patch(
				original: AccessTools.Method(typeof(AbigailGame), nameof(AbigailGame.forceQuit)),
				postfix: new HarmonyMethod(typeof(AbigailGamePatch), nameof(ForceQuitPostfix))
			);
		}

		private static void AbigailGamePostfix()
		{
			buttonExit = new ClickableTextureComponent("Cancel", new Rectangle(-100, -100, 80, 80), null, null, (Texture2D)typeof(Game1).GetField("mobileSpriteSheet", BindingFlags.Public | BindingFlags.Static).GetValue(null), new Rectangle(20, 0, 20, 20), 5f);
		}

		private static void ReceiveLeftClickPostfix(AbigailGame __instance, int x, int y)
		{
			if (buttonExit.containsPoint(x, y))
			{
				__instance.quit = true;
			}
		}

		private static void DrawPostfixButtonExit(SpriteBatch b)
		{
			if (!ModEntry.Config.ArcadeGamesPayToPlay || AbigailGame.playingWithAbigail || !AbigailGame.onStartMenu)
				return;

			b.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
			if (buttonExit is not null)
			{
				buttonExit.bounds.X = Game1.viewport.Width - (int)typeof(Game1).GetField("xEdge", BindingFlags.Public | BindingFlags.Static).GetValue(null) - 112;
				buttonExit.bounds.Y = Game1.viewport.Height - 112;
				buttonExit.draw(b, Color.White, 0.001f);
			}
			b.End();
		}

		private static void DrawPostfix(SpriteBatch b)
		{
			if (!ModEntry.Config.ArcadeGamesPayToPlay || AbigailGame.playingWithAbigail || !AbigailGame.onStartMenu)
				return;

			Rectangle insertCoinSourceRectangle = new(0, 0, 324, 27);
			Rectangle pressAnyButtonSourceRectangle = new(0, 30, 324, 27);
			Rectangle loadingSourceRectangle = new(0, 60, 324, 27);
			Rectangle credit0SourceRectangle = new(0, 90, 324, 27);
			Rectangle credit1SourceRectangle = new(0, 120, 324, 27);
			Rectangle freeSourceRectangle = new(0, 150, 324, 27);

			b.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp);
			if (Game1.currentLocation is not null && Game1.currentLocation.Name.Equals("Saloon"))
			{
				if (PayToPlayUtility.OnInsertCoinMenu)
				{
					b.Draw(AssetManager.JourneyOfThePrairieKing, new Vector2(Game1.viewport.Width / 2 - insertCoinSourceRectangle.Width / 2, Game1.viewport.Height / 2 - insertCoinSourceRectangle.Height / 2 + 8 * AbigailGame.baseTileSize), insertCoinSourceRectangle, Color.White * (Game1.currentGameTime.TotalGameTime.Seconds % 2 == 0 ? 0.5f : 1f), 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
					b.Draw(AssetManager.JourneyOfThePrairieKing, new Vector2(AbigailGame.topLeftScreenCoordinate.X + AbigailGame.baseTileSize, AbigailGame.topLeftScreenCoordinate.Y + 384 * 2 - credit0SourceRectangle.Height - AbigailGame.baseTileSize), credit0SourceRectangle, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
				}
				else
				{
					b.Draw(AssetManager.JourneyOfThePrairieKing, new Vector2(Game1.viewport.Width / 2 - loadingSourceRectangle.Width / 2, Game1.viewport.Height / 2 - loadingSourceRectangle.Height / 2 + 8 * AbigailGame.baseTileSize), loadingSourceRectangle, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
					b.Draw(AssetManager.JourneyOfThePrairieKing, new Vector2(AbigailGame.topLeftScreenCoordinate.X + AbigailGame.baseTileSize, AbigailGame.topLeftScreenCoordinate.Y + 384 * 2 - credit1SourceRectangle.Height - AbigailGame.baseTileSize), credit1SourceRectangle, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
				}
				if (Game1.player.jotpkProgress.Value is null)
				{
					GamePlatformUtility.DrawMoneyBox(b);
				}
			}
			else
			{
				if (PayToPlayUtility.OnInsertCoinMenu)
				{
					b.Draw(AssetManager.JourneyOfThePrairieKing, new Vector2(Game1.viewport.Width / 2 - pressAnyButtonSourceRectangle.Width / 2, Game1.viewport.Height / 2 - pressAnyButtonSourceRectangle.Height / 2 + 8 * AbigailGame.baseTileSize), pressAnyButtonSourceRectangle, Color.White * (Game1.currentGameTime.TotalGameTime.Seconds % 2 == 0 ? 0.5f : 1f), 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
				}
				else
				{
					b.Draw(AssetManager.JourneyOfThePrairieKing, new Vector2(Game1.viewport.Width / 2 - loadingSourceRectangle.Width / 2, Game1.viewport.Height / 2 - loadingSourceRectangle.Height / 2 + 8 * AbigailGame.baseTileSize), loadingSourceRectangle, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
				}
				b.Draw(AssetManager.JourneyOfThePrairieKing, new Vector2(AbigailGame.topLeftScreenCoordinate.X + AbigailGame.baseTileSize, AbigailGame.topLeftScreenCoordinate.Y + 384 * 2 - freeSourceRectangle.Height - AbigailGame.baseTileSize), freeSourceRectangle, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
			}
			b.End();
		}

		private static bool TickPrefix(AbigailGame __instance)
		{
			if (!ModEntry.Config.ArcadeGamesPayToPlay || AbigailGame.playingWithAbigail)
				return true;

			RestartIfRequired(__instance);
			if (Game1.player.jotpkProgress.Value is not null)
			{
				PayToPlayUtility.OnInsertCoinMenu = false;
				return true;
			}
			if (!AbigailGame.onStartMenu || !PayToPlayUtility.OnInsertCoinMenu)
			{
				return true;
			}
			AbigailGame.startTimer = int.MaxValue;
			InsertCoinMenuMusic();
			if (Game1.IsChatting || Game1.textEntry is not null || __instance.quit)
			{
				return true;
			}
			InsertCoinMenuInputs();
			return true;
		}

		private static void RestartIfRequired(AbigailGame __instance)
		{
			if ((AbigailGame.gameOver || __instance.gamerestartTimer > 0) && !AbigailGame.endCutscene)
			{
				__instance.unload();
				Game1.currentMinigame = new AbigailGame();
				PayToPlayUtility.Reset();
			}
		}

		private static void InsertCoinMenuMusic()
		{
			if (Game1.soundBank is not null)
			{
				ICue overworldSong = (ICue)typeof(AbigailGame).GetField("overworldSong", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);

				if (overworldSong is null || !overworldSong.IsPlaying)
				{
					overworldSong = Game1.soundBank.GetCue("Cowboy_OVERWORLD");
					overworldSong.Play();
					Game1.musicPlayerVolume = Game1.options.musicVolumeLevel;
					Game1.musicCategory.SetVolume(Game1.musicPlayerVolume);
					typeof(AbigailGame).GetField("overworldSong", BindingFlags.NonPublic | BindingFlags.Static).SetValue(null, overworldSong);
				}
			}
		}

		private static void InsertCoinMenuInputs()
		{
			bool justTryToInsertCoin = Game1.input.GetMouseState().LeftButton == ButtonState.Pressed || Game1.isOneOfTheseKeysDown(Game1.input.GetKeyboardState(), Game1.options.useToolButton) || Game1.isOneOfTheseKeysDown(Game1.input.GetKeyboardState(), Game1.options.actionButton) || Game1.input.GetKeyboardState().IsKeyDown(Keys.Space) || Game1.input.GetKeyboardState().IsKeyDown(Keys.LeftShift) || Game1.input.GetGamePadState().IsButtonDown(Buttons.A) || Game1.input.GetGamePadState().IsButtonDown(Buttons.B);

			if (justTryToInsertCoin && !PayToPlayUtility.TriedToInsertCoin)
			{
				if (Game1.currentLocation is not null && Game1.currentLocation.Name.Equals("Saloon"))
				{
					if (Game1.player.Money < ModEntry.Config.ArcadeGamesPayToPlayCoinPerJotPKGame)
					{
						Game1.dayTimeMoneyBox.moneyShakeTimer = 1000;
						Game1.playSound("cancel");
					}
					else
					{
						Game1.player.Money -= ModEntry.Config.ArcadeGamesPayToPlayCoinPerJotPKGame;
						Game1.playSound("Pickup_Coin15");
						AbigailGame.startTimer = 1500;
						PayToPlayUtility.OnInsertCoinMenu = false;
					}
				}
				else
				{
					Game1.playSound("Pickup_Coin15");
					AbigailGame.startTimer = 1500;
					PayToPlayUtility.OnInsertCoinMenu = false;
				}
			}
			PayToPlayUtility.TriedToInsertCoin = justTryToInsertCoin;
		}

		private static void TickPostfix(bool __result)
		{
			if (!ModEntry.Config.ArcadeGamesPayToPlay || AbigailGame.playingWithAbigail)
				return;

			if (__result)
			{
				PayToPlayUtility.Reset();
			}
		}

		private static void ReceiveKeyPressPostfix(AbigailGame __instance, Keys k)
		{
			if (!ModEntry.Config.ArcadeGamesPayToPlay || AbigailGame.playingWithAbigail)
				return;

			if (Game1.input.GetGamePadState().IsButtonDown(Buttons.Back) || k.Equals(Keys.Escape))
			{
				__instance.quit = true;
			}
		}

		private static void ForceQuitPostfix(bool __result)
		{
			if (!ModEntry.Config.ArcadeGamesPayToPlay || AbigailGame.playingWithAbigail)
				return;

			if (__result)
			{
				PayToPlayUtility.Reset();
			}
		}
	}
}
