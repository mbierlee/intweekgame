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
        private readonly Texture2D playerTexture;
        private readonly Vector2 origin;
        private Vector2 position;
        private readonly float yPosition;
        private float balance;
        public bool Fallen { get; set; }
        private float scale;
        public Rectangle CollisionMask { get; set; }
        public Rectangle CollisionArea
        {
            get
            {
                return new Rectangle((int)((XPosition) - (CollisionMask.Width)), (int)((YPosition) - (CollisionMask.Height)), (int)((CollisionMask.Width) * 2), (int)((CollisionMask.Height) * 2));
            }
        }

        public Player(Texture2D playerTexture)
        {
            IntWeekGame game = (IntWeekGame)IntWeekGame.GameInstance;

            this.playerTexture = playerTexture;
            XPosition = (float)game.GraphicsDevice.Viewport.Width / 2;
            yPosition = 550f;
            scale = Util.GetParallelScaleFromY(yPosition);
            origin = new Vector2(75, 250);
        }

        public void Update()
        {
            position = new Vector2(XPosition, YPosition);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(playerTexture, position - Origin, null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 1 - scale);
            spriteBatch.Draw(IntWeekGame.TestBall, new Vector2((balance * 400) + 400, 0), Color.White);

            if (IntWeekGame.DebugDrawCollisionBoxes)
            {
                spriteBatch.Draw(IntWeekGame.pixel, CollisionArea, Color.Red);
            }
        }

        public void InfluenceFromBalance ()
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

        public Vector2 Origin
        {
            get { return origin; }
        }


        public void CheckPlayerCollisionWithObject(ParallelGameObject parallelGameObject)
        {
            if (parallelGameObject.Collidable && (
                                                                       parallelGameObject.CollisionArea.Contains(
                                                                           CollisionArea) ||
                                                                       parallelGameObject.CollisionArea.Intersects(
                                                                           CollisionArea)))
            {
                if (parallelGameObject is StreetLight || parallelGameObject is TrashCan)
                {
                    ((IntWeekGame)IntWeekGame.GameInstance).PlayerHitObstacle();
                } else if (parallelGameObject is Car)
                {
                    ((IntWeekGame)IntWeekGame.GameInstance).PlayerHitObstacle();
                    parallelGameObject.Speed = 0;
                }
            }
        }
    }
}
