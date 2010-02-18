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
    public class Beer : ParallelGameObject
    {
        public Beer()
            : base(((IntWeekGame)IntWeekGame.GameInstance).BeerTexture)
        {
            Origin = new Vector2(37, 70);
            CollisionMask = new Rectangle(0, 0, 37, 2);
        }
    }
}
