using System;

namespace Packages.com.terallite.gamelogic.Runtime
{
    public readonly struct Vector2
    {
        // ReSharper disable once InconsistentNaming
        public readonly float x;

        // ReSharper disable once InconsistentNaming
        public readonly float y;

        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public static Vector2 operator +(Vector2 lhs, Vector2 rhs)
        {
            return new Vector2(lhs.x + rhs.x, lhs.y + rhs.y);
        }

        public static Vector2 operator -(Vector2 lhs, Vector2 rhs)
        {
            return new Vector2(lhs.x - rhs.x, lhs.y - rhs.y);
        }

        public static Vector2 operator *(Vector2 lhs, float rhs)
        {
            return new Vector2(lhs.x * rhs, lhs.y * rhs);
        }

        public static Vector2 operator /(Vector2 lhs, float rhs)
        {
            return new Vector2(lhs.x / rhs, lhs.y / rhs);
        }

        public float Norm()
        {
            return MathF.Sqrt(x * x + y * y);
        }

        public Vector2 Normalized()
        {
            return this / Norm();
        }

        public float Dot(Vector2 rhs)
        {
            return x * rhs.x + y * rhs.y;
        }
    }
}