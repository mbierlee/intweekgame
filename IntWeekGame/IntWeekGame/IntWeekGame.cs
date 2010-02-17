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

namespace IntWeekGame
{
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class IntWeekGame : Microsoft.Xna.Framework.Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		private Texture2D backGroundImage;
		private Texture2D roadMarkTexture;
		private readonly Rectangle backgroundRectangle;

		private List<ParallelGameObject> parallelGameObjectCollection;

		private Player player;
        //private ParallelGameObject testBall;

		public Vector2 Horizon;
		private byte roadMarksInterval;
		private float scrollSpeed;
		private Rectangle viewPortRectangle;

	    public static Texture2D testBall;

	    private readonly float balanceScale;
	    private readonly float wiiBalanceScale;
	    private readonly float movementScale;
	    private readonly float wiiMovementScale;

	    private float balanceModifierDirection;
	    private float balanceModifierStrength;

	    private KeyboardState keyboardState;

		Wiimote wm;

		public IntWeekGame()
		{
		    balanceScale = 0.05f;
		    wiiBalanceScale = 0.05f;
		    movementScale = 10;
		    wiiMovementScale = 50;

			GameInstance = this;

			graphics = new GraphicsDeviceManager(this);
			backgroundRectangle = new Rectangle(0, 0, 800, 600);

			Content.RootDirectory = "Content";

			wm = new Wiimote();
			try
			{
				wm.Connect();
				wm.SetReportType(InputReport.IRAccel, true);
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
			scrollSpeed = 0.5f;
			parallelGameObjectCollection = new List<ParallelGameObject>();
			viewPortRectangle = new Rectangle(GraphicsDevice.Viewport.X, GraphicsDevice.Viewport.Y, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);
			Horizon = new Vector2(((float)graphics.GraphicsDevice.Viewport.Width) / 2, 0);

			backGroundImage = Content.Load<Texture2D>("Backgrounds/bg");
			roadMarkTexture = Content.Load<Texture2D>("Sprites/RoadMark");
			player = new Player(Content.Load<Texture2D>("Sprites/testplayer"));
			//testBall = new ParallelGameObject(roadMarkTexture) { Origin = new Vector2(((float)roadMarkTexture.Width) / 2, (float)roadMarkTexture.Height), Position = Horizon, Direction = Util.GetDirectionVectorFromAngle(MathHelper.ToRadians(90)) };
			//parallelGameObjectCollection.Add(testBall);

		    testBall = Content.Load<Texture2D>("Sprites/testball");

			// Cheat to getting roadmarks on the map before the game begins.
			for (int i = 0; i < (2000 / scrollSpeed); i++)
			{
				UpdateRoadMarks();
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

			UpdateRoadMarks();
			UpdateParallelGameObjects();

            if (wm.WiimoteState.ButtonState.A)
            {
                this.Exit();
            }

			if (wm != null)
			{
				player.XPosition += wm.WiimoteState.AccelState.Values.Y * wiiMovementScale;
                player.Balance += wm.WiimoteState.AccelState.Values.Y * wiiBalanceScale;
			}

			if (keyboardState.IsKeyDown(Keys.Left))
			{
				player.XPosition -= movementScale;
			    player.Balance -= balanceScale;
			}
			else if (keyboardState.IsKeyDown(Keys.Right))
			{
				player.XPosition += movementScale;
                player.Balance += balanceScale;
			}

			player.Update();

			base.Update(gameTime);
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

		private void UpdateRoadMarks()
		{
			roadMarksInterval++;
			if (roadMarksInterval > (120 / scrollSpeed))
			{
				ParallelGameObject roadMark = new ParallelGameObject(roadMarkTexture) { Speed = scrollSpeed, Origin = new Vector2(((float)roadMarkTexture.Width) / 2, (float)roadMarkTexture.Height), Position = Horizon, Direction = Util.GetDirectionVectorFromAngle(MathHelper.ToRadians(90)) };
				parallelGameObjectCollection.Add(roadMark);
				roadMarksInterval = 0;
			}
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			spriteBatch.Begin();
			spriteBatch.Draw(backGroundImage, backgroundRectangle, Color.White);

			foreach (ParallelGameObject parallelGameObject in parallelGameObjectCollection)
			{
				parallelGameObject.Draw(spriteBatch);
			}

			player.Draw(spriteBatch);
			spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
