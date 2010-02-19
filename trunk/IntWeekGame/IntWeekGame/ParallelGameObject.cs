using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IntWeekGame
{
    public class ParallelGameObject
    {
        private float scale;

        public ParallelGameObject(Texture2D texture2D)
        {
            Texture2D = texture2D;
            DrawingArea = Rectangle.Empty;
            Position = Vector2.Zero;
            Direction = Vector2.Zero;
            SpriteEffects = SpriteEffects.None;
            CollisionMask = Rectangle.Empty;
            Origin = new Vector2(0, 0);
        }

        public Texture2D Texture2D { get; set; }
        private bool Disposing { get; set; }
        public bool IsFlat { get; set; }
        public SpriteEffects SpriteEffects { get; set; }

        public bool Collidable
        {
            get
            {
                if (CollisionMask != Rectangle.Empty)
                {
                    return true;
                }
                return false;
            }
        }

        public Vector2 Position { get; set; }
        public Vector2 Direction { get; set; }
        public Vector2 Origin { get; set; }
        public float Speed { get; set; }
        public Rectangle DrawingArea { get; private set; }

        /// <summary>
        /// Calculated from origin. X and Y properties not needed.
        /// </summary>
        public Rectangle CollisionMask { get; set; }

        public Rectangle CollisionArea
        {
            get
            {
                return new Rectangle((int) ((Position.X) - (CollisionMask.Width*scale)),
                                     (int) ((Position.Y) - (CollisionMask.Height*scale)),
                                     (int) ((CollisionMask.Width*scale)*2), (int) ((CollisionMask.Height*scale)*2));
            }
        }

        /// <summary>
        /// Updates the game logic.
        /// </summary>
        public void Update()
        {
            float scrollSpeed = ((IntWeekGame) IntWeekGame.GameInstance).ScrollSpeed;

            scale = Util.GetParallelScaleFromY(Position.Y);
            Position += (Direction*(scale + 0.1f))*(Speed + scrollSpeed);

            DrawingArea = new Rectangle((int) (Position.X - (Origin.X*scale)), (int) (Position.Y - (Origin.Y*scale)),
                                        (int) (Texture2D.Width*scale), (int) (Texture2D.Height*scale));
        }

        /// <summary>
        /// Drawns the object onto the given sprite batch.
        /// </summary>
        /// <param name="spriteBatch">Spritebatch that draws this object.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            if (!Disposing)
            {
                //spriteBatch.Draw(Texture2D, DrawingArea, null, Color.White, 0, Vector2.Zero, SpriteEffects, 1 - scale);
                spriteBatch.Draw(Texture2D, Position, null, Color.White, 0, Origin, new Vector2(scale),
                                 SpriteEffects, 1 - scale);

                /*
                if (IntWeekGame.DebugDrawCollisionBoxes)
                {
                    spriteBatch.Draw(IntWeekGame.Pixel, CollisionArea, Color.Red);
                }*/
            }
        }

        public void Dispose()
        {
            Texture2D.Dispose();

            Disposing = true;
        }
    }
}