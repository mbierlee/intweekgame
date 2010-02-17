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
    public class ParallelGameObject 
    {
        public Texture2D Texture2D { get; private set; }
        public bool Disposing { get; private set;}
        public bool IsFlat { get; set; }
        public SpriteEffects SpriteEffects { get; set; }
        public bool Collidable { get; set; }

        public ParallelGameObject(Texture2D texture2D)
        {
            Texture2D = texture2D;
            DrawingArea = Rectangle.Empty;
            Position = Vector2.Zero;
            Direction = Vector2.Zero;
            SpriteEffects = SpriteEffects.None;
        }

        public Vector2 Position { get; set; }
        public Vector2 Direction { get; set; }
        public Vector2 Origin { get; set; }
        public float Speed { get; set; }
        public Rectangle DrawingArea { get; private set; }
        public int Depth { get; private set; }

        private float scale;

        /// <summary>
        /// Updates the game logic.
        /// </summary>
        public void Update()
        {
            float scrollSpeed = ((IntWeekGame) IntWeekGame.GameInstance).ScrollSpeed;

            scale = Util.GetParallelScaleFromY(Position.Y);
            Position += (Direction * (scale + 0.1f)) * (Speed + scrollSpeed);

            DrawingArea = new Rectangle((int)(Position.X - (Origin.X * scale)), (int)(Position.Y - (Origin.Y * scale)),
                                        (int)(Texture2D.Width * scale), (int)(Texture2D.Height * scale));
            //DrawingArea = new Rectangle((int)(Position.X), (int)(Position.Y),
              //                        (int)(Texture2D.Width * scale), (int)(Texture2D.Height * scale));
        }

        /// <summary>
        /// Drawns the object onto the given sprite batch.
        /// </summary>
        /// <param name="spriteBatch">Spritebatch that draws this object.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            if (!Disposing)
            {
                spriteBatch.Draw(Texture2D, DrawingArea, null, Color.White, 0, Vector2.Zero, SpriteEffects, 1 - scale);
            }
        }
        
        public void Dispose()
        {
            Texture2D.Dispose();

            Disposing = true;
        }
    }
}
