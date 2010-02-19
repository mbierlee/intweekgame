using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IntWeekGame
{
    internal class GameOver : DrawableGameComponent
    {
        private Rectangle screenBackgroundRectangle;
        private Texture2D screenBackgroundTexture;
        private SpriteBatch screenBatch;
		private SpriteFont screenFont;

        public GameOver(Game game)
            : base(game)
        {
        }

        protected override void LoadContent()
        {
            screenBatch = new SpriteBatch(GraphicsDevice);
            Game.Content.Load<SpriteFont>("Fonts/Verdana18Bold");

			screenFont = Game.Content.Load<SpriteFont>("Fonts/Verdana60");

			screenBackgroundTexture = Game.Content.Load<Texture2D>("Backgrounds/GameOver");
            //screenBackgroundRectangle = new Rectangle(9, 13, 782, 575);
            screenBackgroundRectangle = new Rectangle(0, 0, 800, 600);

            base.LoadContent();
        }

        /*
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }*/

        public override void Draw(GameTime gameTime)
        {
            if (((IntWeekGame) IntWeekGame.GameInstance).Gamestate == Gamestate.GameOver)
            {
                screenBatch.Begin();
                screenBatch.Draw(screenBackgroundTexture, screenBackgroundRectangle, Color.White);
				int score = ((Hud)Game.Services.GetService(typeof(Hud))).Score;
				int scoreWidth = score.ToString().Length * 50;
				screenBatch.DrawString(screenFont, score.ToString(), new Vector2(390 - (scoreWidth / 2), 286), Color.White);
                screenBatch.End();
            }

            base.Draw(gameTime);
        }
    }
}