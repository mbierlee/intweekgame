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

namespace IntWeekGame
{
    sealed class Util
    {
        /// <summary>
        /// Returns a direction vector which indicates the direction of a given angle.
        /// </summary>
        /// <param name="angle">Angle of direction in radians</param>
        /// <returns>Vector holding the correct direction.</returns>
        public static Vector2 GetDirectionVectorFromAngle(float angle)
        {
            return new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
        }

        /// <summary>
        /// Interpolates the scale in relation to the Y coordinate, screen height and horizon.
        /// </summary>
        /// <param name="y">The current Y coordinate on screen.</param>
        /// <returns></returns>
        public static float GetParallelScaleFromY(float y)
        {
            Vector2 horizon = ((IntWeekGame)IntWeekGame.GameInstance).Horizon;

            if (y <= horizon.Y)
            {
                return 0f;
            }

            if (y < IntWeekGame.GameInstance.GraphicsDevice.Viewport.Height)
            {
                y -= horizon.Y;
                float max = (float)IntWeekGame.GameInstance.GraphicsDevice.Viewport.Height - horizon.Y;

                return y / max;
            }

            return 1f;
        }
    }
}
