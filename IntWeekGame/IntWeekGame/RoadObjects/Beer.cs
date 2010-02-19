using Microsoft.Xna.Framework;

namespace IntWeekGame.RoadObjects
{
    public class Beer : ParallelGameObject
    {
        public Beer()
            : base(((IntWeekGame) IntWeekGame.GameInstance).BeerTexture)
        {
            Origin = new Vector2(37, 70);
            CollisionMask = new Rectangle(0, 0, 37, 2);
        }
    }
}