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
        public Car() : base(((IntWeekGame)IntWeekGame.GameInstance).carTexture)
        {
            Speed = 0;
        }
    }
}
