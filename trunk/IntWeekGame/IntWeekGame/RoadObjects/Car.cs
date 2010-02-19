using Microsoft.Xna.Framework;
using System;

namespace IntWeekGame.RoadObjects
{
    public class Car : ParallelGameObject
    {
        enum DriverState { Normal, Drunk, RoadKiller }

        private DriverState driverState;
        private float dirmod;
        private float dirscale;

        public Car() : base(((IntWeekGame) IntWeekGame.GameInstance).CarTexture)
        {
            Direction = new Vector2(-165, 600)/600;

            Random random = new Random();
            driverState = (DriverState)random.Next(0, 2);
            //driverState = DriverState.Drunk;

            Origin = new Vector2(139, 188);
            Speed = 5f;
            CollisionMask = new Rectangle(0, 0, 139, 5);
            dirscale = 8f;
        }


        public override void Update()
        {
            if (driverState == DriverState.Drunk)
            {
                dirmod += dirscale;
                Direction = new Vector2(dirmod, 600)/600;
                if (dirmod > 300 || dirmod < -300)
                {
                    dirscale *= -1;
                }
            }

            base.Update();
        } 
    }
}