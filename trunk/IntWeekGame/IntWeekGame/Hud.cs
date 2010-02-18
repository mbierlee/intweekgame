﻿using System;
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

		private Texture2D hudTirednessTexture;
		private Rectangle hudTirednessRectangle, hudTirednessSourceRectangle;

		private Texture2D hudBalanceTexture;
		private Rectangle hudBalanceRectangle;

	    public int Score { get; set; }

	    public float Tiredness { get; set; }

	    private int tirednessOffset;
		private float balance;
		private int balanceOffset;

		public Hud(Game game)
			: base(game)
		{

		}
		protected override void LoadContent()
		{
			hudBatch = new SpriteBatch(GraphicsDevice);
			hudFont = this.Game.Content.Load<SpriteFont>("Fonts/Verdana18Bold");

			hudBackgroundTexture = this.Game.Content.Load<Texture2D>("Backgrounds/hud");
			hudBackgroundRectangle = new Rectangle(600, 15, 192, 162);

			hudTirednessTexture = this.Game.Content.Load<Texture2D>("Sprites/Tiredness");

			hudBalanceTexture = this.Game.Content.Load<Texture2D>("Sprites/Balance");

			base.LoadContent();
		}

		public override void Update(GameTime gameTime)
		{
			tirednessOffset = (int)(182 * Tiredness);
			hudTirednessRectangle = new Rectangle(607, 98, tirednessOffset, 13);
			hudTirednessSourceRectangle = new Rectangle(182 - tirednessOffset, 0, tirednessOffset, 13);

			balance = ((IntWeekGame)IntWeekGame.GameInstance).Player.Balance;
			balance = (balance + 1) / 2;

			balanceOffset = (int)(152 * balance); // 186 - 32
			hudBalanceRectangle = new Rectangle(603 + balanceOffset, 147, 32, 15);

			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime)
		{
			if (((IntWeekGame)IntWeekGame.GameInstance).Gamestate != Gamestate.Start)
			{
				hudBatch.Begin();
				hudBatch.Draw(hudBackgroundTexture, hudBackgroundRectangle, Color.White);
				hudBatch.DrawString(hudFont, Score.ToString(), new Vector2(735, 42), new Color(51, 51, 51));

				hudBatch.Draw(hudTirednessTexture, hudTirednessRectangle, hudTirednessSourceRectangle, Color.White);

				hudBatch.Draw(hudBalanceTexture, hudBalanceRectangle, Color.White);

				hudBatch.End();
			}

			base.Draw(gameTime);
		}
	}
}
