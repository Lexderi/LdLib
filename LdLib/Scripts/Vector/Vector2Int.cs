using Silk.NET.Maths;

namespace LdLib.Vector;

public struct Vector2Int
{
    #region Readonly

    public readonly Vector2Int Zero => new(0);
    public readonly Vector2Int One => new(1);
    public readonly Vector2Int Right => new(1, 0);
    public readonly Vector2Int Up => new(0, 1);
    public readonly Vector2Int Left => new(-1, 0);
    public readonly Vector2Int Down => new(0, -1);

    #endregion

    public int X { get; set; }
    public int Y { get; set; }

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

    public readonly Vector2Int Normalized => new Vector2Int(X, Y) / Magnitude;

    public Vector2Int(int xy)
    {
        this = new(xy, xy);
    }

    public Vector2Int(int x, int y)
    {
        X = x;
        Y = y;
    }

    public Vector2Int(Vector2 v)
    {
        X = (int)v.X;
        Y = (int)v.Y;
    }

    public Vector2Int(Vector2D<int> v)
    {
        X = v.X;
        Y = v.Y;
    }

    #region Operators

    public static Vector2Int operator +(Vector2Int a, Vector2Int b)
    {
        return new(a.X + b.X, a.Y + b.Y);
    }

    public static Vector2Int operator -(Vector2Int a, Vector2Int b)
    {
        return new(a.X - b.X, a.Y - b.Y);
    }

    public static Vector2Int operator -(Vector2Int a)
    {
        return new(-a.X, -a.Y);
    }

    public static Vector2Int operator *(Vector2Int v, int scalar)
    {
        return new(v.X * scalar, v.Y * scalar);
    }

    public static Vector2Int operator *(Vector2Int v, float scalar)
    {
        return new((int)(v.X * scalar), (int)(v.Y * scalar));
    }

    public static Vector2Int operator /(Vector2Int v, int scalar)
    {
        return new(v.X / scalar, v.Y / scalar);
    }

    public static Vector2Int operator /(Vector2Int v, float scalar)
    {
        return new((int)(v.X / scalar), (int)(v.Y / scalar));
    }

    public static Vector2Int operator *(Vector2Int a, Vector2Int b)
    {
        return new(a.X * b.X, a.Y * b.Y);
    }

    public static Vector2Int operator /(Vector2Int a, Vector2Int b)
    {
        return new(a.X / b.X, a.Y / b.Y);
    }

    public static bool operator ==(Vector2Int left, Vector2Int right)
    {
        return left.X.Equals(right.X) && left.Y.Equals(right.Y);
    }

    public static bool operator !=(Vector2Int left, Vector2Int right)
    {
        return !(left == right);
    }

    public static implicit operator Vector2Int(Vector2 v)
    {
        return new((int)v.X, (int)v.Y);
    }

    public static implicit operator Vector2D<int>(Vector2Int v)
    {
        return new(v.X, v.Y);
    }

    #endregion

    public Vector2Int Limit(float maxMagnitude)
    {
        if (Magnitude > maxMagnitude) Magnitude = maxMagnitude;

        return this;
    }

    public bool Equals(Vector2Int other)
    {
        return X.Equals(other.X) && Y.Equals(other.Y);
    }

    public override bool Equals(object? obj)
    {
        return obj is Vector2Int other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }

    public static Vector2Int Abs(Vector2Int value)
    {
        return new(Math.Abs(value.X), Math.Abs(value.Y));
    }

    public static Vector2Int Lerp(Vector2Int value1, Vector2Int value2, float amount)
    {
        return new((Vector2)value1 * (1.0f - amount) + (Vector2)value2 * amount);
    }

    public static Vector2Int Max(Vector2Int value1, Vector2Int value2)
    {
        return new(
            value1.X > value2.X ? value1.X : value2.X,
            value1.Y > value2.Y ? value1.Y : value2.Y
        );
    }

    public static Vector2Int Min(Vector2Int value1, Vector2Int value2)
    {
        return new(
            value1.X < value2.X ? value1.X : value2.X,
            value1.Y < value2.Y ? value1.Y : value2.Y
        );
    }

    public static Vector2Int Sqrt(Vector2Int value)
    {
        return new((int)Math.Sqrt(value.X), (int)Math.Sqrt(value.Y));
    }

    public override string ToString()
    {
        return $"<{X}; {Y}>";
    }
}