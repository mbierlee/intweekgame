using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using WiimoteLib;
using System.Collections;
using Point = Microsoft.Xna.Framework.Point;
using IntWeekGame.RoadObjects;

namespace IntWeekGame
{
	public enum Gamestate { Start, Playing, GameOver };

	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class IntWeekGame : Microsoft.Xna.Framework.Game
	{
		GraphicsDeviceManager graphics;

		private Gamestate gamestate;

		private SpriteBatch standingObjectsSpriteBatch;
		private SpriteBatch flatObjectsSpriteBatch;
		private SpriteBatch backgroundBatch;

		private Texture2D backGroundImage;
		private Texture2D roadMarkTexture;
		public Texture2D StreetLightTexture;
		public Texture2D TrashCanTexture;
		public Texture2D CarTexture;
	    public Texture2D BrokenCarTexture;

	    //private int score;
	    //private float tiredness;

		public static Texture2D Pixel;
		public const bool DebugDrawCollisionBoxes = true;
		private readonly Rectangle backgroundRectangle;
		private Random random;

		private List<ParallelGameObject> parallelGameObjectCollection;

		private Player player;

		public Vector2 Horizon;
		private int roadMarkSpawnTicker;
		private int streetLightSpawnTicker;
		public float ScrollSpeed { get; private set; }
		private Rectangle viewPortRectangle;

		public readonly float balanceScale;
		private readonly float wiiBalanceScale;

		private float balanceModifier;

		private KeyboardState keyboardState;

		private double lastObstacleSpawn;

		private Wiimote Wiimote;
		private int wiiMoteRumbleState;
		private DateTime rumbleDateTime;
		private int wiiMoteRumbleMilliseconds;

		private SoundEffect soundFall;

		public IntWeekGame()
		{
			gamestate = Gamestate.Start;

			balanceScale = 0.01f;
			wiiBalanceScale = 0.01f;
			ScrollSpeed = 0.5f;

			GameInstance = this;

			graphics = new GraphicsDeviceManager(this);
			backgroundRectangle = new Rectangle(0, 0, 800, 600);

			Content.RootDirectory = "Content";

			Wiimote = new Wiimote();

			try
			{
				Wiimote.Connect();
				Wiimote.SetReportType(InputReport.IRAccel, true);
				wiiMoteRumbleState = 0;
				wiiMoteRumbleMilliseconds = 500;
			}
			catch { }
		}

		public static Game GameInstance { get; private set; }

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			random = new Random();
			parallelGameObjectCollection = new List<ParallelGameObject>();
			viewPortRectangle = new Rectangle(GraphicsDevice.Viewport.X, GraphicsDevice.Viewport.Y, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
			player = new Player { CollisionMask = new Rectangle(0, 0, 38, 2) };
			this.Components.Add(new Hud(this));
			this.Components.Add(new StartScreen(this));
			this.Components.Add(new GameOver(this));

			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			standingObjectsSpriteBatch = new SpriteBatch(GraphicsDevice);
			flatObjectsSpriteBatch = new SpriteBatch(GraphicsDevice);
			backgroundBatch = new SpriteBatch(GraphicsDevice);

			Horizon = new Vector2(((float)graphics.GraphicsDevice.Viewport.Width) / 2, 0);

			backGroundImage = Content.Load<Texture2D>("Backgrounds/bg");
			roadMarkTexture = Content.Load<Texture2D>("Sprites/RoadMark");
			//player = new Player(Content.Load<Texture2D>("Sprites/testplayer")) { CollisionMask = new Rectangle(0, 0, 40, 2) };
			player.LoadContent();
			StreetLightTexture = Content.Load<Texture2D>("Sprites/straatlantaarn");
			Pixel = Content.Load<Texture2D>("Pixel");
			TrashCanTexture = Content.Load<Texture2D>("Sprites/Trashcan");
			CarTexture = Content.Load<Texture2D>("Sprites/auto");
		    BrokenCarTexture = Content.Load<Texture2D>("Sprites/autokaput");

			soundFall = IntWeekGame.GameInstance.Content.Load<SoundEffect>("Audio/Bounce");

			// Cheat to getting scenery before the game begins.
			for (int i = 0; i < (2000 / ScrollSpeed); i++)
			{
				SpawnRoadObjects(null);
				UpdateParallelGameObjects();
			}

		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// all content.
		/// </summary>
		protected override void UnloadContent()
		{
			// TODO: Unload any non ContentManager content here
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			keyboardState = Keyboard.GetState();

			if (Wiimote.WiimoteState.ButtonState.Home || keyboardState.IsKeyDown(Keys.Escape))
			{
				this.Exit();
			}

			if (player.Fallen)
			{
				RumbleWiiMoteUpdate();
			}

			switch (gamestate)
			{
				case Gamestate.Start:
					if (Wiimote.WiimoteState.ButtonState.A || keyboardState.IsKeyDown(Keys.Enter))
					{
						gamestate = Gamestate.Playing;
					}
					break;

				case Gamestate.Playing:
					SpawnRoadObjects(gameTime);
					UpdateParallelGameObjects();

					ProcessUserInput();
					player.InfluenceFromBalance();
					CalculateBalance(gameTime);

					player.Update();

					if (player.Fallen)
					{
						ScrollSpeed = 0f;
						RumbleWiiMoteUpdate();
					}

					break;

				case Gamestate.GameOver:
					if (Wiimote.WiimoteState.ButtonState.A || keyboardState.IsKeyDown(Keys.Enter))
					{
						this.Exit();
					}
					break;
			}

			base.Update(gameTime);
		}

		private void CalculateBalance(GameTime gameTime)
		{
			if (player.Fallen)
			{
				return;
			}

			int balanceModificationDirectionChance = random.Next(1, 100);
			if (balanceModificationDirectionChance > 10 && balanceModificationDirectionChance < 15)
			{
				balanceModifier = (float)random.NextDouble();
			}
			else if (balanceModificationDirectionChance > 75 && balanceModificationDirectionChance < 80)
			{
				balanceModifier = (float)random.NextDouble() * -1f;
			}

			if (player.Balance == -1f || player.Balance == 1f)
			{
				PlayerFell();
			}
			else
			{
				player.Balance += (balanceModifier * (balanceScale * random.Next(1, 3))) / 2;
			}
		}

		private void ProcessUserInput()
		{
			if (Wiimote != null)
			{
				player.Balance += Wiimote.WiimoteState.AccelState.Values.X * wiiBalanceScale;
			}

			if (keyboardState.IsKeyDown(Keys.Left))
			{
				player.Balance -= balanceScale;
			}
			else if (keyboardState.IsKeyDown(Keys.Right))
			{
				player.Balance += balanceScale;
			}

			//Debug input
			if (keyboardState.IsKeyDown(Keys.F12))
			{
				if (ScrollSpeed != 50f)
				{
					ScrollSpeed = 50f;
				}
				else
				{
					ScrollSpeed = 0.5f;
				}
			}
		}

		private void UpdateParallelGameObjects()
		{
			if (player.Fallen)
			{
				return;
			}
			for (int i = 0; i < parallelGameObjectCollection.Count; i++)
			{
				ParallelGameObject parallelGameObject = parallelGameObjectCollection[i];

				parallelGameObject.Update();
				if (!viewPortRectangle.Intersects(parallelGameObject.DrawingArea) &&
					!viewPortRectangle.Contains(parallelGameObject.DrawingArea))
				{
					parallelGameObjectCollection.Remove(parallelGameObject);
				}
				else { player.CheckPlayerCollisionWithObject(parallelGameObject); }
			}
		}

		internal void PlayerFell()
		{
			soundFall.Play();
			PlayerHitObstacle();
		}
		internal void PlayerHitObstacle()
		{
			if (player.Fallen == false)
			{
				player.Fallen = true;
				RumbleWiiMote();
				gamestate = Gamestate.GameOver;
			}
		}

		private void SpawnRoadObjects(GameTime gameTime)
		{
			if (player.Fallen)
			{
				return;
			}

			roadMarkSpawnTicker++;
			streetLightSpawnTicker++;
			if (roadMarkSpawnTicker > (120 / ScrollSpeed))
			{
				ParallelGameObject roadMark = new ParallelGameObject(roadMarkTexture)
												  {
													  IsFlat = true,
													  Origin =
														  new Vector2(((float)roadMarkTexture.Width) / 2,
																	  roadMarkTexture.Height),
													  Position = Horizon,
													  Direction =
														  Util.GetDirectionVectorFromAngle(MathHelper.ToRadians(90))
												  };
				parallelGameObjectCollection.Add(roadMark);
				roadMarkSpawnTicker = 0;
			}

			if (streetLightSpawnTicker > (300 / ScrollSpeed))
			{
				StreetLight streetLight = new StreetLight()
											  {
												  CollisionMask = new Rectangle(0, 0, 10, 2),
												  Origin = new Vector2(10, 282),
												  Position = Horizon,
												  Direction = new Vector2(-382, 600) / 600
											  };
				StreetLight streetLight2 = new StreetLight()
											   {
												   CollisionMask = new Rectangle(0, 0, 10, 2),
												   SpriteEffects = SpriteEffects.FlipHorizontally,
												   Origin = new Vector2(88, 282),
												   Position = Horizon,
												   Direction = new Vector2(382, 600) / 600
											   };
				parallelGameObjectCollection.Add(streetLight);
				parallelGameObjectCollection.Add(streetLight2);
				streetLightSpawnTicker = 0;
			}

			if (gameTime != null)
			{
				if (gameTime.TotalGameTime.TotalSeconds - lastObstacleSpawn > 2)
				{
					int typeChance = random.Next(0, 100);

					if (typeChance > 0 && typeChance < 40)
					{
						TrashCan trashCan = new TrashCan
												{
													CollisionMask = new Rectangle(0, 0, 35, 2),
													Origin = new Vector2(47, 110),
													Position = Horizon,
													Direction = new Vector2(random.Next(-400, 400), 373) / 373
												};
						parallelGameObjectCollection.Add(trashCan);

					}
					else if (typeChance > 85 && typeChance < 100)
					{
						Car car = new Car
									  {
										  Direction = new Vector2(-165, 600) / 600,
										  Position = Horizon,
										  Origin = new Vector2(142, 189),
										  Speed = 5f,
										  CollisionMask = new Rectangle(0, 0, 139, 2)
									  };

						parallelGameObjectCollection.Add(car);
					}

					lastObstacleSpawn = gameTime.TotalGameTime.TotalSeconds;
				}
			}
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);
			backgroundBatch.Begin();
			flatObjectsSpriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Texture, SaveStateMode.None);
			standingObjectsSpriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.BackToFront, SaveStateMode.None);

			backgroundBatch.Draw(backGroundImage, backgroundRectangle, Color.White);
			player.Draw(standingObjectsSpriteBatch);

			foreach (ParallelGameObject parallelGameObject in parallelGameObjectCollection)
			{
				if (parallelGameObject.IsFlat)
				{
					parallelGameObject.Draw(flatObjectsSpriteBatch);
				}
				else
				{
					parallelGameObject.Draw(standingObjectsSpriteBatch);
				}
			}

			backgroundBatch.End();
			flatObjectsSpriteBatch.End();
			standingObjectsSpriteBatch.End();

			base.Draw(gameTime);
		}
		/// <summary>
		/// Rumble the WiiMote for a certain duration
		/// </summary>
		private void RumbleWiiMote()
		{
			wiiMoteRumbleState = 1;
		}

		/// <summary>
		/// Update Rumble the WiiMote if the timer is set
		/// </summary>
		private void RumbleWiiMoteUpdate()
		{
			if (wiiMoteRumbleState == 1)
			{
				Wiimote.SetRumble(true);
				rumbleDateTime = DateTime.Now;
				wiiMoteRumbleState = 2;
			}
			else if (wiiMoteRumbleState == 2 && DateTime.Now.Subtract(rumbleDateTime).Milliseconds > wiiMoteRumbleMilliseconds)
			{
				Wiimote.SetRumble(false);
				wiiMoteRumbleState = 0;
			}
		}
		public Player Player
		{
			get
			{
				return player;
			}
		}
		public Gamestate Gamestate
		{
			get
			{
				return gamestate;
			}
		}
	}
}
