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
using IntWeekGame.RoadObjects;

namespace IntWeekGame
{
    public class Player
    {
        private Texture2D legsTextureStrip;
        private Texture2D bodyTextureStrip;

        private readonly Vector2 legsOrigin;
        private Vector2 bodyOrigin;
        private Vector2 position;
        private readonly float yPosition;
        private float balance;
        public bool Fallen { get; set; }
        private float scale;
        public Rectangle CollisionMask { get; set; }

        private Rectangle legsTextureDrawArea;
        private Rectangle bodyTextureDrawArea;
        private int frameTick;

        public Rectangle CollisionArea
        {
            get
            {
                return new Rectangle((int)((XPosition) - (CollisionMask.Width * scale)), (int)((YPosition) - (CollisionMask.Height * scale)), (int)((CollisionMask.Width * scale) * 2), (int)((CollisionMask.Height * scale) * 2));
            }
        }

        public Player()
        {
            IntWeekGame game = (IntWeekGame)IntWeekGame.GameInstance;

            XPosition = (float)game.GraphicsDevice.Viewport.Width / 2;
            //yPosition = 550f;
            yPosition = 500f;
            scale = Util.GetParallelScaleFromY(yPosition);
            legsOrigin = new Vector2(43, 126);
            bodyOrigin = new Vector2(123, 132);

            legsTextureDrawArea = new Rectangle(0, 0, 89, 130);
            bodyTextureDrawArea = new Rectangle(0, 0, 243, 138);
        }

        public void LoadContent()
        {
            legsTextureStrip = IntWeekGame.GameInstance.Content.Load<Texture2D>("Sprites/playerlegs_strip15");
            bodyTextureStrip = IntWeekGame.GameInstance.Content.Load<Texture2D>("Sprites/playerbody_strip10");
        }

        public void Update()
        {
            position = new Vector2(XPosition, YPosition);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 bodyPosition = new Vector2(position.X, position.Y - ((legsTextureDrawArea.Height - (bodyTextureDrawArea.Height - bodyOrigin.Y)) * scale));

            legsTextureDrawArea.X = (frameTick / (60 / 15)) * legsTextureDrawArea.Width;
            bodyTextureDrawArea.X = (frameTick / (60 / 10)) * bodyTextureDrawArea.Width;

            spriteBatch.Draw(legsTextureStrip, position, legsTextureDrawArea, Color.White, 0, LegsOrigin, scale, SpriteEffects.None, 1 - scale);
            spriteBatch.Draw(bodyTextureStrip, bodyPosition, bodyTextureDrawArea, Color.White, MathHelper.ToRadians(MathHelper.Lerp(0, 40, Balance)), bodyOrigin, scale, SpriteEffects.None, 1 - scale);
            ////spriteBatch.Draw(IntWeekGame.TestBall, new Vector2((balance * 400) + 400, 0), Color.White);

            if (IntWeekGame.DebugDrawCollisionBoxes)
            {
                spriteBatch.Draw(IntWeekGame.Pixel, CollisionArea, Color.Red);
            }

            frameTick++;
            if (frameTick == 60)
            {
                frameTick = 0;
            }
        }

        public void InfluenceFromBalance()
        {
            if (Fallen)
            {
                return;
            }

            XPosition += (4 * Balance);
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

        public float YPosition
        {
            get { return yPosition; }
        }

        public Vector2 LegsOrigin
        {
            get { return legsOrigin; }
        }


        public void CheckPlayerCollisionWithObject(ParallelGameObject parallelGameObject)
        {
            IntWeekGame game = ((IntWeekGame) IntWeekGame.GameInstance);

            if (parallelGameObject.Collidable && (
                                                                       parallelGameObject.CollisionArea.Contains(
                                                                           CollisionArea) ||
                                                                       parallelGameObject.CollisionArea.Intersects(
                                                                           CollisionArea)))
            {
                if (parallelGameObject is StreetLight || parallelGameObject is TrashCan)
                {
                    game.PlayerHitObstacle();
                }
                else if (parallelGameObject is Car)
                {
                    game.PlayerHitObstacle();
                    parallelGameObject.Speed = 0;
                    parallelGameObject.Texture2D = game.BrokenCarTexture;
                    parallelGameObject.Origin = new Vector2(159, 188);
                } else if (parallelGameObject is Beer)
                {
                    game.Score += 200;
                    game.Tiredness += 0.2f;
                    game.RemoveGameObject(parallelGameObject);
                }
            }
        }
    }
}
