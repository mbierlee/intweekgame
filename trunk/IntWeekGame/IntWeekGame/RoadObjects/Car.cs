using Microsoft.Xna.Framework;

namespace IntWeekGame.RoadObjects
{
    public class Car : ParallelGameObject
    {
        public Car() : base(((IntWeekGame) IntWeekGame.GameInstance).CarTexture)
        {
            Direction = new Vector2(-165, 600)/600;

            Origin = new Vector2(139, 188);
            Speed = 5f;
            CollisionMask = new Rectangle(0, 0, 139, 2);
        }
    }
}