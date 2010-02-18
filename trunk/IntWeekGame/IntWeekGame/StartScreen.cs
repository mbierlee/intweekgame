using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace IntWeekGame
{
	class StartScreen : DrawableGameComponent
	{
		private SpriteBatch screenBatch;
		private Texture2D screenBackgroundTexture;
		private Rectangle screenBackgroundRectangle;
		private SpriteFont screenFont;

		public StartScreen(Game game)
			: base(game)
		{

		}
		protected override void LoadContent()
		{
			screenBatch = new SpriteBatch(GraphicsDevice);
			screenFont = this.Game.Content.Load<SpriteFont>("Fonts/Verdana18Bold");

			screenBackgroundTexture = this.Game.Content.Load<Texture2D>("Backgrounds/StartScreen");
			screenBackgroundRectangle = new Rectangle(9, 13, 782, 575);

			base.LoadContent();
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime)
		{
			if (((IntWeekGame)IntWeekGame.GameInstance).Gamestate == Gamestate.Start)
			{
				screenBatch.Begin();
				screenBatch.Draw(screenBackgroundTexture, screenBackgroundRectangle, Color.White);
				screenBatch.End();
			}

			base.Draw(gameTime);
		}
	}
}
