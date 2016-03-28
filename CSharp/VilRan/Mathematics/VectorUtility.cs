using Microsoft.Xna.Framework;
using System;

namespace VilRan.Mathematics
{
    public static class VectorUtility
    {
        public static float WrappingDistanceSquared(Vector2 v1, Vector2 v2, float maxX)
        {
            float halfMaxX = maxX / 2;
            if (v2.X - v1.X > halfMaxX)
                v2.X -= maxX;
            if (v1.X - v2.X > halfMaxX)
                v1.X -= maxX;

            return (v2 - v1).LengthSquared();
        }

        public static float WrappingDistance(Vector2 v1, Vector2 v2, float maxX)
        {
            float halfMaxX = maxX / 2;
            if (v2.X - v1.X > halfMaxX)
                v2.X -= maxX;
            if (v1.X - v2.X > halfMaxX)
                v1.X -= maxX;

            return (v2 - v1).Length();
        }

        /// <summary>
        /// Returns null if interception is impossible.
        /// </summary>
        /// <param name="shooterPosition"></param>
        /// <param name="targetPosition"></param>
        /// <param name="targetVelocity"></param>
        /// <param name="projectileSpeed"></param>
        /// <returns></returns>
        public static Vector2? FindInterceptPoint(Vector2 shooterPosition, Vector2 targetPosition, Vector2 targetVelocity, float projectileSpeed)
        {
            return FindInterceptPoint(shooterPosition, Vector2.Zero, targetPosition, targetVelocity, projectileSpeed);
        }

        /// <summary>
        /// Returns null if interception is impossible.
        /// Don't use this overload unless the shooter's velocity is added to the projectile's velocity.
        /// </summary>
        /// <param name="shooterPosition"></param>
        /// <param name="shooterVelocity"></param>
        /// <param name="targetPosition"></param>
        /// <param name="targetVelocity"></param>
        /// <param name="projectileSpeed"></param>
        /// <returns></returns>
        public static Vector2? FindInterceptPoint(Vector2 shooterPosition, Vector2 shooterVelocity, Vector2 targetPosition, Vector2 targetVelocity, float projectileSpeed)
        {
            Vector2 relativePosition = targetPosition - shooterPosition;
            Vector2 relativeVelocity = targetVelocity - shooterVelocity;
            float a = projectileSpeed * projectileSpeed - relativeVelocity.LengthSquared();//Vector2.Dot(targetVelocity, targetVelocity);
            float b = -2 * Vector2.Dot(relativeVelocity, relativePosition);
            float c = -relativePosition.LengthSquared();// Vector2.Dot(-relativePosition, relativePosition);
            float d = b * b - 4 * a * c;
            if (d > 0)
            {
                float result = (b + (float)Math.Sqrt(d)) / (2 * a);
                return targetPosition + result * relativeVelocity;
            }
            return null;
        }
    }
}
