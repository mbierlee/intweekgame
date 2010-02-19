using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IntWeekGame
{
    internal class StartScreen : DrawableGameComponent
    {
        private Rectangle screenBackgroundRectangle;
        private Texture2D screenBackgroundTexture;
        private SpriteBatch screenBatch;

        public StartScreen(Game game)
            : base(game)
        {
        }

        protected override void LoadContent()
        {
            screenBatch = new SpriteBatch(GraphicsDevice);
            Game.Content.Load<SpriteFont>("Fonts/Verdana18Bold");

            screenBackgroundTexture = Game.Content.Load<Texture2D>("Backgrounds/StartScreen");
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
            if (((IntWeekGame) IntWeekGame.GameInstance).Gamestate == Gamestate.Start)
            {
                screenBatch.Begin();
                screenBatch.Draw(screenBackgroundTexture, screenBackgroundRectangle, Color.White);
                screenBatch.End();
            }

            base.Draw(gameTime);
        }
    }
}