using Microsoft.Xna.Framework;

namespace IntWeekGame.RoadObjects
{
    public class TrashCan : ParallelGameObject
    {
        public TrashCan()
            : base(((IntWeekGame) IntWeekGame.GameInstance).TrashCanTexture)
        {
            CollisionMask = new Rectangle(0, 0, 19, 2);
            Origin = new Vector2(38, 89);
        }
    }
}