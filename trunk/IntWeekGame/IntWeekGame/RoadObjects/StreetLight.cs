namespace IntWeekGame.RoadObjects
{
    internal class StreetLight : ParallelGameObject
    {
        public StreetLight()
            : base(((IntWeekGame) IntWeekGame.GameInstance).StreetLightTexture)
        {
        }
    }
}