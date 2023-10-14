
using System;
using Silk.NET.Maths;

namespace LdLib.Vector
{
    public struct Vector2
    {
        #region Readonly
        public static readonly Vector2 Zero = new(0);
        public static readonly Vector2 One = new(1);
        public static readonly Vector2 Right = new(1, 0);
        public static readonly Vector2 Up = new(0, 1);
        public static readonly Vector2 Left = new(-1, 0);
        public static readonly Vector2 Down = new(0, -1);
        #endregion

        public float X { get; set; }
        public float Y { get; set; }

        public float Magnitude
        { 
            readonly get => MathF.Sqrt(SqrMagnitude);
            set => this *= value / Magnitude;
        }

        public float SqrMagnitude
        {
            readonly get => X * X + Y * Y;
            set => this *= MathF.Sqrt(value) / Magnitude;
        }

        public readonly Vector2 Normalized => new Vector2(X, Y) / Magnitude;

        public Vector2(float xy) => this = new(xy, xy);

        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }
        
        #region Operators

        public static Vector2 operator +(Vector2 a, Vector2 b) => new(a.X + b.X, a.Y + b.Y);
        public static Vector2 operator -(Vector2 a, Vector2 b) => new(a.X - b.X, a.Y - b.Y);
        public static Vector2 operator -(Vector2 a) => new(-a.X, -a.Y);
        public static Vector2 operator *(Vector2 v, float scalar) => new(v.X * scalar, v.Y * scalar);
        public static Vector2 operator /(Vector2 v, float scalar) => new(v.X / scalar, v.Y / scalar);
        public static Vector2 operator *(Vector2 a, Vector2 b) => new(a.X * b.X, a.Y * b.Y);
        public static Vector2 operator /(Vector2 a, Vector2 b) => new(a.X / b.X, a.Y / b.Y);
        public static bool operator ==(Vector2 left, Vector2 right) => left.X.Equals(right.X) && left.Y.Equals(right.Y);
        public static bool operator !=(Vector2 left, Vector2 right) => !(left == right);
        public static implicit operator Vector2(Vector2Int v) => new(v.X, v.Y);

        public static Vector2 operator *(Matrix2X2<float> m, Vector2 v)
        {
            float x = m.M11 * v.X + m.M12 * v.Y;
            float y = m.M21 * v.X + m.M22 * v.Y;

            return new(x, y);
        }

        #endregion

        public Vector2 Limit(float maxMagnitude)
        {
            if (Magnitude > maxMagnitude)
            {
                Magnitude = maxMagnitude;
            }

            return this;
        }
        public bool Equals(Vector2 other)
        {
            return X.Equals(other.X) && Y.Equals(other.Y);
        }
        public override bool Equals(object? obj)
        {
            return obj is Vector2 other && Equals(other);
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        public static Vector2 Abs(Vector2 value) => new (Math.Abs(value.X),Math.Abs(value.Y));
        public static Vector2 Lerp(Vector2 value1, Vector2 value2, float amount) => value1 * (1.0f - amount) + value2 * amount;
        public static Vector2 Max(Vector2 value1, Vector2 value2) => new(
            (value1.X > value2.X) ? value1.X : value2.X,
            (value1.Y > value2.Y) ? value1.Y : value2.Y
        );
        public static Vector2 Min(Vector2 value1, Vector2 value2) => new(
                (value1.X < value2.X) ? value1.X : value2.X,
                (value1.Y < value2.Y) ? value1.Y : value2.Y
        );
        public static Vector2 Sqrt(Vector2 value) => new(MathF.Sqrt(value.X), MathF.Sqrt(value.Y));

        public Vector2 Rotate(float angle)
        {
            // pre-calculate sin and cos
            float s = MathF.Sin(angle);
            float c = MathF.Cos(angle);

            // rotation matrix
            float newX = X * c - Y * s;
            float newY = X * s + Y * c;

            X = newX;
            Y = newY;

            return this;
        }

        public override string ToString() => $"<{X}; {Y}>";
    }
}
