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
    class Player
    {
        private readonly Texture2D playerTexture;
        private readonly Vector2 origin;
        private Vector2 position;
        private readonly float YPosition;
        private float balance;
        public bool Fallen { get; set; }

        public Player(Texture2D playerTexture)
        {
            IntWeekGame Game = (IntWeekGame)IntWeekGame.GameInstance;

            this.playerTexture = playerTexture;
            XPosition = (float)Game.GraphicsDevice.Viewport.Width / 2;
            YPosition = 550f;
            origin = new Vector2(75, 250);
        }

        public void Update()
        {
            position = new Vector2(XPosition, YPosition);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(playerTexture, position - origin, Color.White);
            spriteBatch.Draw(IntWeekGame.testBall, new Vector2((balance * 400) + 400, 0), Color.White);
        }

        public void InfluenceFromBalance ()
        {
            XPosition += (5 * Balance);
        }

        private float xPosition;

        public float XPosition
        {
            get { return xPosition; }
            set { xPosition = MathHelper.Clamp(value, 0, 800); }
        }

        public float Balance
        {
            get { return balance; }
            set { balance = MathHelper.Clamp(value, -1f, 1f); }
        }
    }
}
