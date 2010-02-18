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
	class Hud : DrawableGameComponent
	{
		private SpriteBatch hudBatch;
		private Texture2D hudBackgroundTexture;
		private Rectangle hudBackgroundRectangle;
		private SpriteFont hudFont;

		private Texture2D hudBalanceTexture;
		private Rectangle hudBalanceRectangle, hudBalanceSourceRectangle;

		private int score;
		private int tiredness;

		public Hud(Game game)
			: base(game)
		{

		}
		protected override void LoadContent()
		{
			hudBatch = new SpriteBatch(GraphicsDevice);
			hudBackgroundTexture = this.Game.Content.Load<Texture2D>("Backgrounds/hud");
			hudBackgroundRectangle = new Rectangle(600, 15, 192, 162);
			hudFont = this.Game.Content.Load<SpriteFont>("Fonts/Verdana18Bold");

			hudBalanceTexture = this.Game.Content.Load<Texture2D>("Sprites/Balance");
			hudBalanceRectangle = new Rectangle(600, 15, 192, 162);
			hudBalanceSourceRectangle = new Rectangle(600, 15, 192, 162);

			base.LoadContent();
		}

		public override void Update(GameTime gameTime)
		{
			score = 80;
			tiredness = 50;

			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime)
		{
			hudBatch.Begin();
			hudBatch.Draw(hudBackgroundTexture, hudBackgroundRectangle, Color.White);
			hudBatch.DrawString(hudFont, score.ToString(), new Vector2(735, 42), new Color(51, 51, 51));

			hudBatch.Draw(hudBackgroundTexture, hudBalanceRectangle, hudBalanceSourceRectangle, Color.White);


			hudBatch.DrawString(hudFont, tiredness.ToString(), new Vector2(677, 100), Color.Black);
			hudBatch.End();

			base.Draw(gameTime);
		}
	}
}
