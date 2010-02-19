using Microsoft.Xna.Framework;

namespace IntWeekGame.RoadObjects
{
    internal class Coffee : ParallelGameObject
    {
        public Coffee()
            : base(((IntWeekGame) IntWeekGame.GameInstance).CoffeeTexture)
        {
            Origin = new Vector2(12, 23);
            CollisionMask = new Rectangle(0, 0, 12, 2);
        }
    }
}