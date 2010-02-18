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
    public class Car : ParallelGameObject
    {
        public Car() : base(((IntWeekGame)IntWeekGame.GameInstance).CarTexture)
        {
            Direction = new Vector2(-165, 600)/600;

            Origin = new Vector2(139, 188);
            Speed = 5f;
            CollisionMask = new Rectangle(0, 0, 139, 2);
        }
    }
}
