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
		private Texture2D hudTexture;
		private Rectangle hudRectangle;
		private SpriteFont hudFontTitle;
		private SpriteFont hudFontValue;

		private int score;
		private int tiredness;

		public Hud(Game game)
			: base(game)
		{

		}
		protected override void LoadContent()
		{
			hudBatch = new SpriteBatch(GraphicsDevice);
			hudTexture = this.Game.Content.Load<Texture2D>("Backgrounds/hud");
			hudRectangle = new Rectangle(600, 15, 192, 122);
			hudFontTitle = this.Game.Content.Load<SpriteFont>("Fonts/Verdana15");
			hudFontValue = this.Game.Content.Load<SpriteFont>("Fonts/Verdana18Bold");

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
			hudBatch.Draw(hudTexture, hudRectangle, Color.White);
			hudBatch.DrawString(hudFontTitle, "Score", new Vector2(670, 20), new Color(51, 51, 51));
			hudBatch.DrawString(hudFontTitle, "Tiredness", new Vector2(650, 80), new Color(51, 51, 51));
			hudBatch.DrawString(hudFontValue, score.ToString(), new Vector2(677, 45), Color.Black);
			hudBatch.DrawString(hudFontValue, tiredness.ToString(), new Vector2(677, 100), Color.Black);
			hudBatch.End();

			base.Draw(gameTime);
		}
	}
}
