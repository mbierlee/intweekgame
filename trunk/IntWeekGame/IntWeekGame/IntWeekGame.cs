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

namespace IntWeekGame
{
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class IntWeekGame : Microsoft.Xna.Framework.Game
	{
		GraphicsDeviceManager graphics;
		private SpriteBatch standingObjectsSpriteBatch;
	    private SpriteBatch flatObjectsSpriteBatch;
	    private SpriteBatch backgroundBatch;

		private Texture2D backGroundImage;
		private Texture2D roadMarkTexture;
	    private Texture2D streetLightTexture;
		private readonly Rectangle backgroundRectangle;

		private List<ParallelGameObject> parallelGameObjectCollection;

		private Player player;
		//private ParallelGameObject testBall;

		public Vector2 Horizon;
		private int roadMarkSpawnTicker;
	    private int streetLightSpawnTicker; 
		public float ScrollSpeed { get; private set; }
		private Rectangle viewPortRectangle;

		public static Texture2D testBall;

		private readonly float balanceScale;
	    private readonly float wiiBalanceScale;

		private float balanceModifier;

		private KeyboardState keyboardState;

		private Wiimote Wiimote;
		//private bool rumble;
		private DateTime rumbleDateTime;

		public IntWeekGame()
		{
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
				//rumble = false;
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
            parallelGameObjectCollection = new List<ParallelGameObject>();
			viewPortRectangle = new Rectangle(GraphicsDevice.Viewport.X, GraphicsDevice.Viewport.Y, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

			this.Components.Add(new Hud(this));

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
			player = new Player(Content.Load<Texture2D>("Sprites/testplayer"));
            streetLightTexture = Content.Load<Texture2D>("Sprites/straatlantaarn");

			testBall = Content.Load<Texture2D>("Sprites/testball");

			// Cheat to getting scenery before the game begins.
			for (int i = 0; i < (2000 / ScrollSpeed); i++)
			{
				SpawnRoadObjects();
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

			SpawnRoadObjects();
			UpdateParallelGameObjects();

			ProcessUserInput();
		    player.InfluenceFromBalance();
            CalculateBalance(gameTime);

			player.Update();

			base.Update(gameTime);
		}

		private void CalculateBalance(GameTime gameTime)
		{
			Random random = new Random((int)gameTime.TotalRealTime.Ticks);
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
				ScrollSpeed = 0f;

				if (Wiimote != null)
				{
					if (player.Fallen == false)
					{
						Wiimote.SetRumble(true);
						rumbleDateTime = DateTime.Now;
					}
					else if (rumbleDateTime != DateTime.MinValue && player.Fallen == true && DateTime.Now.Subtract(rumbleDateTime).Milliseconds > 500)
					{
						Wiimote.SetRumble(false);
						rumbleDateTime = DateTime.MinValue;
					}
				}
				player.Fallen = true;
			}
			else
			{
                player.Balance += (balanceModifier * (balanceScale * random.Next(1, 3))) / 2;
			}
		}

		private void ProcessUserInput()
		{
			if (Wiimote.WiimoteState.ButtonState.A)
			{
				this.Exit();
			}

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
		}

		private void UpdateParallelGameObjects()
		{
			for (int i = 0; i < parallelGameObjectCollection.Count; i++)
			{
				ParallelGameObject parallelGameObject = parallelGameObjectCollection[i];
				parallelGameObject.Update();
				if (!viewPortRectangle.Intersects(parallelGameObject.DrawingArea) && !viewPortRectangle.Contains(parallelGameObject.DrawingArea))
				{
					parallelGameObjectCollection.Remove(parallelGameObject);
				}
			}
		}

	    private void SpawnRoadObjects()
		{
			if (ScrollSpeed == 0f)
			{
				return;
			}

			roadMarkSpawnTicker++;
		    streetLightSpawnTicker++;
			if (roadMarkSpawnTicker > (120 / ScrollSpeed))
			{
				ParallelGameObject roadMark = new ParallelGameObject(roadMarkTexture) { IsFlat = true, Origin = new Vector2(((float)roadMarkTexture.Width) / 2, (float)roadMarkTexture.Height), Position = Horizon, Direction = Util.GetDirectionVectorFromAngle(MathHelper.ToRadians(90)) };
				parallelGameObjectCollection.Add(roadMark);
				roadMarkSpawnTicker = 0;
			}
            
            if (streetLightSpawnTicker > (300 / ScrollSpeed))
            {
                ParallelGameObject streetLight = new ParallelGameObject(streetLightTexture) { Origin = new Vector2(10, 282), Position = Horizon, Direction = new Vector2(-382, 600) / 600 };
                ParallelGameObject streetLight2 = new ParallelGameObject(streetLightTexture) { SpriteEffects = SpriteEffects.FlipHorizontally ,Origin = new Vector2(88, 282), Position = Horizon, Direction = new Vector2(382, 600) / 600 };
                parallelGameObjectCollection.Add(streetLight);
                parallelGameObjectCollection.Add(streetLight2);
                streetLightSpawnTicker = 0;
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

			foreach (ParallelGameObject parallelGameObject in parallelGameObjectCollection)
			{
                if (parallelGameObject.IsFlat)
                {
                    parallelGameObject.Draw(flatObjectsSpriteBatch);
                } else
                {
                    parallelGameObject.Draw(standingObjectsSpriteBatch);
                }
			}

            player.Draw(standingObjectsSpriteBatch);
            backgroundBatch.End();
            flatObjectsSpriteBatch.End();
			standingObjectsSpriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
