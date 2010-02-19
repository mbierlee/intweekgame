using IntWeekGame.RoadObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace IntWeekGame
{
    public class Player
    {
        private readonly Vector2 fallenPlayerOrigin;
        private readonly Vector2 legsOrigin;
        private readonly float scale;
        private readonly float yPosition;
        private float balance;
        private Vector2 bodyOrigin;
        private Rectangle bodyTextureDrawArea;
        private Texture2D bodyTextureStrip;
        private Texture2D fallenPlayer;
        private int frameTick;
        private Rectangle legsTextureDrawArea;
        private Texture2D legsTextureStrip;
		private SoundEffect soundBurp;
		private SoundEffect soundSwallowing;
		private SoundEffect soundBeerOpening;
		private SoundEffect soundSlurp;
		private Vector2 position;

        private float xPosition;

        public Player()
        {
            var game = (IntWeekGame) IntWeekGame.GameInstance;

            XPosition = (float) game.GraphicsDevice.Viewport.Width/2;
            yPosition = 500f;
            scale = Util.GetParallelScaleFromY(yPosition);
            legsOrigin = new Vector2(43, 126);
            bodyOrigin = new Vector2(123, 132);
            fallenPlayerOrigin = new Vector2(30, 50);

            legsTextureDrawArea = new Rectangle(0, 0, 89, 130);
            bodyTextureDrawArea = new Rectangle(0, 0, 243, 138);
        }

        public bool Fallen { get; set; }
        public Rectangle CollisionMask { get; set; }

        public Rectangle CollisionArea
        {
            get
            {
                return new Rectangle((int) ((XPosition) - (CollisionMask.Width*scale)),
                                     (int) ((YPosition) - (CollisionMask.Height*scale)),
                                     (int) ((CollisionMask.Width*scale)*2), (int) ((CollisionMask.Height*scale)*2));
            }
        }

        public float XPosition
        {
            get { return xPosition; }
            set { xPosition = MathHelper.Clamp(value, 0, 800); }
        }

        public float Balance
        {
            get { return balance; }
            set { balance = MathHelper.Clamp(value, -1f, 1f); }
        }

        public float YPosition
        {
            get { return yPosition; }
        }

        public Vector2 LegsOrigin
        {
            get { return legsOrigin; }
        }

        public void LoadContent()
        {
            legsTextureStrip = IntWeekGame.GameInstance.Content.Load<Texture2D>("Sprites/playerlegs_strip15");
            //bodyTextureStrip = IntWeekGame.GameInstance.Content.Load<Texture2D>("Sprites/playerbody_strip10");
            bodyTextureStrip = IntWeekGame.GameInstance.Content.Load<Texture2D>("Sprites/testplayer");
            fallenPlayer = IntWeekGame.GameInstance.Content.Load<Texture2D>("Sprites/FallenPlayer");

			soundBurp = IntWeekGame.GameInstance.Content.Load<SoundEffect>("Audio/Burp");
			soundBeerOpening = IntWeekGame.GameInstance.Content.Load<SoundEffect>("Audio/BeerOpening");
			soundSwallowing = IntWeekGame.GameInstance.Content.Load<SoundEffect>("Audio/Swallowing");
			soundSlurp = IntWeekGame.GameInstance.Content.Load<SoundEffect>("Audio/Slurp");
        }

        public void Update()
        {
            position = new Vector2(XPosition, YPosition);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var bodyPosition = new Vector2(position.X,
                                           position.Y -
                                           ((legsTextureDrawArea.Height - (bodyTextureDrawArea.Height - bodyOrigin.Y))*
                                            scale));

            legsTextureDrawArea.X = (frameTick/(60/15))*legsTextureDrawArea.Width;
            bodyTextureDrawArea.X = (frameTick/(60/10))*bodyTextureDrawArea.Width;

            if (!Fallen)
            {
                spriteBatch.Draw(legsTextureStrip, position, legsTextureDrawArea, Color.White, 0, LegsOrigin, scale,
                                 SpriteEffects.None, 1 - scale);
                spriteBatch.Draw(bodyTextureStrip, bodyPosition, bodyTextureDrawArea, Color.White,
                                 MathHelper.ToRadians(MathHelper.Lerp(0, 40, Balance)), bodyOrigin, scale,
                                 SpriteEffects.None, 1 - scale);
                frameTick++;
                if (frameTick == 60)
                {
                    frameTick = 0;
                }
            }
            else
            {
                if (balance >= 0)
                {
                    spriteBatch.Draw(fallenPlayer, position, null, Color.White, 0, fallenPlayerOrigin, scale,
                                     SpriteEffects.None, 1 - scale);
                }
                else
                {
                    spriteBatch.Draw(fallenPlayer, position, null, Color.White, 0,
                                     new Vector2(fallenPlayer.Width, fallenPlayer.Height) - fallenPlayerOrigin, scale,
                                     SpriteEffects.FlipHorizontally, 1 - scale);
                }
            }

            /*
            if (IntWeekGame.DebugDrawCollisionBoxes)
            {
                spriteBatch.Draw(IntWeekGame.Pixel, CollisionArea, Color.Red);
            }*/
        }

        public void InfluenceFromBalance()
        {
            if (Fallen)
            {
                return;
            }

            XPosition += (4*Balance);
        }


        public void CheckPlayerCollisionWithObject(ParallelGameObject parallelGameObject)
        {
            var game = ((IntWeekGame) IntWeekGame.GameInstance);

            if (!Fallen && parallelGameObject.Collidable && (
                                                     parallelGameObject.CollisionArea.Contains(
                                                         CollisionArea) ||
                                                     parallelGameObject.CollisionArea.Intersects(
                                                         CollisionArea)))
            {
                if (parallelGameObject is StreetLight)
                {
                    game.PlayerHitObstacle();
                }
                else if (parallelGameObject is TrashCan)
                {
                    game.PlayerHitObstacle();
                    parallelGameObject.Texture2D = game.KnockedOverTrashCanTexture;
                    parallelGameObject.Origin = new Vector2(153, 76);
                }
                else if (parallelGameObject is Car)
                {
                    game.PlayerHitObstacle();
                    parallelGameObject.Speed = 0;
                    parallelGameObject.Texture2D = game.BrokenCarTexture;
                    parallelGameObject.Origin = new Vector2(159, 188);
                }
                else if (parallelGameObject is Beer)
                {
					soundBeerOpening.Play();
					soundSwallowing.Play();
					soundBurp.Play();

					game.Score += 200;
                    game.Tiredness += 0.2f;
                    game.RemoveGameObject(parallelGameObject);
                }
                else if (parallelGameObject is Coffee)
                {
					soundSlurp.Play();

					game.Score += 50;
                    game.Tiredness -= 0.3f;
                    game.RemoveGameObject(parallelGameObject);
                }
            }
        }
    }
}