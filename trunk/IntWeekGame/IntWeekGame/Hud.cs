using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace IntWeekGame
{
    internal class Hud : DrawableGameComponent
    {
        private float balance;
        private float balanceOffset;
        private Rectangle hudBackgroundRectangle;
        private Texture2D hudBackgroundTexture;
        private Rectangle hudBalanceRectangle, hudBalanceIndicatorRectangle;
        private Texture2D hudBalanceTexture, hudBalanceIndicatorTexture;
        private SpriteBatch hudBatch;
        private SpriteFont hudFont;

        private Rectangle hudTirednessRectangle, hudTirednessSourceRectangle;
        private Texture2D hudTirednessTexture;

        private int tirednessOffset;

        public Hud(Game game)
            : base(game)
        {
        }

        public int Score { get; set; }

        public float Tiredness { get; set; }

        protected override void LoadContent()
        {
            hudBatch = new SpriteBatch(GraphicsDevice);
            hudFont = Game.Content.Load<SpriteFont>("Fonts/Verdana18Bold");

            hudBackgroundTexture = Game.Content.Load<Texture2D>("Backgrounds/hud");
            hudBackgroundRectangle = new Rectangle(600, 15, 192, 122);

            hudTirednessTexture = Game.Content.Load<Texture2D>("Sprites/Tiredness");

			hudBalanceTexture = Game.Content.Load<Texture2D>("Sprites/Balance");
			hudBalanceIndicatorTexture = Game.Content.Load<Texture2D>("Sprites/BalanceIndicator");

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            tirednessOffset = (int) (182*Tiredness);
            hudTirednessRectangle = new Rectangle(607, 102, tirednessOffset, 13);
            hudTirednessSourceRectangle = new Rectangle(182 - tirednessOffset, 0, tirednessOffset, 13);

            balance = ((IntWeekGame) IntWeekGame.GameInstance).Player.Balance;
            balance = (balance + 1)/2;

			float playerOffset = ((IntWeekGame)IntWeekGame.GameInstance).Player.XPosition - 111;
            balanceOffset = 160*balance; // 186 - 62
			hudBalanceRectangle = new Rectangle(Convert.ToInt32(playerOffset), 515, 222, 17);
			hudBalanceIndicatorRectangle = new Rectangle(Convert.ToInt32(playerOffset + balanceOffset), 500, 62, 47);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (((IntWeekGame) IntWeekGame.GameInstance).Gamestate != Gamestate.Start)
            {
                hudBatch.Begin();
                hudBatch.Draw(hudBackgroundTexture, hudBackgroundRectangle, Color.White);
                hudBatch.DrawString(hudFont, Score.ToString(), new Vector2(735, 42), new Color(51, 51, 51));

                hudBatch.Draw(hudTirednessTexture, hudTirednessRectangle, hudTirednessSourceRectangle, Color.White);

				hudBatch.Draw(hudBalanceTexture, hudBalanceRectangle, Color.White);
				hudBatch.Draw(hudBalanceIndicatorTexture, hudBalanceIndicatorRectangle, Color.White);

                hudBatch.End();
            }

            base.Draw(gameTime);
        }
    }
}