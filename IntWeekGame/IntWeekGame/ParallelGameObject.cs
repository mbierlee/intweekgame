using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IntWeekGame
{
    public class ParallelGameObject
    {
        public float Scale { get; private set; }

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
                return new Rectangle((int) ((Position.X) - (CollisionMask.Width*Scale)),
                                     (int) ((Position.Y) - (CollisionMask.Height*Scale)),
                                     (int) ((CollisionMask.Width*Scale)*2), (int) ((CollisionMask.Height*Scale)*2));
            }
        }

        /// <summary>
        /// Updates the game logic.
        /// </summary>
        public virtual void Update()
        {
            float scrollSpeed = ((IntWeekGame) IntWeekGame.GameInstance).ScrollSpeed;

            Scale = Util.GetParallelScaleFromY(Position.Y);
            Position += (Direction*(Scale + 0.1f))*(Speed + scrollSpeed);

            DrawingArea = new Rectangle((int) (Position.X - (Origin.X*Scale)), (int) (Position.Y - (Origin.Y*Scale)),
                                        (int) (Texture2D.Width*Scale), (int) (Texture2D.Height*Scale));
        }

        /// <summary>
        /// Drawns the object onto the given sprite batch.
        /// </summary>
        /// <param name="spriteBatch">Spritebatch that draws this object.</param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (!Disposing)
            {
                //spriteBatch.Draw(Texture2D, DrawingArea, null, Color.White, 0, Vector2.Zero, SpriteEffects, 1 - scale);
                spriteBatch.Draw(Texture2D, Position, null, Color.White, 0, Origin, new Vector2(Scale),
                                 SpriteEffects, 1 - Scale);

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