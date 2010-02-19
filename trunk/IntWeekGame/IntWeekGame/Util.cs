using System;
using Microsoft.Xna.Framework;

namespace IntWeekGame
{
    internal static class Util
    {
        /// <summary>
        /// Returns a direction vector which indicates the direction of a given angle.
        /// </summary>
        /// <param name="angle">Angle of direction in radians</param>
        /// <returns>Vector holding the correct direction.</returns>
        public static Vector2 GetDirectionVectorFromAngle(float angle)
        {
            return new Vector2((float) Math.Cos(angle), (float) Math.Sin(angle));
        }

        /// <summary>
        /// Interpolates the scale in relation to the Y coordinate, screen height and horizon.
        /// </summary>
        /// <param name="y">The current Y coordinate on screen.</param>
        /// <returns></returns>
        public static float GetParallelScaleFromY(float y)
        {
            Vector2 horizon = ((IntWeekGame) IntWeekGame.GameInstance).Horizon;

            if (y <= horizon.Y)
            {
                return 0f;
            }

            if (y < IntWeekGame.GameInstance.GraphicsDevice.Viewport.Height)
            {
                y -= horizon.Y;
                float max = IntWeekGame.GameInstance.GraphicsDevice.Viewport.Height - horizon.Y;

                return y/max;
            }

            return 1f;
        }
    }
}