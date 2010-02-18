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

namespace IntWeekGame.RoadObjects
{
    class Coffee : ParallelGameObject
    {
        public Coffee()
            : base(((IntWeekGame)IntWeekGame.GameInstance).CoffeeTexture)
        {
            Origin = new Vector2(12, 23);
            CollisionMask = new Rectangle(0, 0, 12, 2);
        }
    }
}
