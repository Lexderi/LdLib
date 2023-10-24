using Silk.NET.Maths;

namespace LdLib.Vector;

/// <summary>
/// A 2 dimensional vector containing ints
/// </summary>
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
    /// <summary>
    /// X component of the vector
    /// </summary>
    public int X { get; set; }
    /// <summary>
    /// Y component of the vector
    /// </summary>
    public int Y { get; set; }
    /// <summary>
    /// Magnitude of the vector
    /// </summary>
    public float Magnitude
    {
        readonly get => MathF.Sqrt(SqrMagnitude);
        set => this *= value / Magnitude;
    }
    /// <summary>
    /// Squared magnitude of the vector, better performance than normal magnitude
    /// </summary>
    public float SqrMagnitude
    {
        readonly get => X * X + Y * Y;
        set => this *= MathF.Sqrt(value) / Magnitude;
    }
    /// <summary>
    /// The vector shortened down to have a magnitude of 1
    /// </summary>
    public readonly Vector2Int Normalized => new Vector2Int(X, Y) / Magnitude;
    /// <summary>
    /// A 2 dimensional vector containing floats
    /// </summary>
    /// <param name="xy">X and y component of the vector</param>
    public Vector2Int(int xy)
    {
        this = new(xy, xy);
    }
    /// <summary>
    /// A 2 dimensional vector containing floats
    /// </summary>
    /// <param name="x">X component of the vector</param>
    /// <param name="y">Y component of the vector</param>
    public Vector2Int(int x, int y)
    {
        X = x;
        Y = y;
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

    public static implicit operator Vector2Int(Vector2D<int> v) => new(v.X, v.Y);
    public static implicit operator Vector2D<int>(Vector2Int v)
    {
        return new(v.X, v.Y);
    }

    #endregion
    
    /// <summary>
    /// Limits the magnitude of the vector
    /// </summary>
    /// <param name="maxMagnitude">the maximum magnitude the vector can have</param>
    /// <returns>The vector</returns>
    public Vector2Int Limit(float maxMagnitude)
    {
        if (Magnitude > maxMagnitude) Magnitude = maxMagnitude;

        return this;
    }
    /// <summary>
    /// Checks if the x and y components equal each other;
    /// </summary>
    /// <param name="other">the vector that should be compared with</param>
    /// <returns>If the vectors are equal</returns>
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
    /// <summary>
    /// Gets the version of the vector, where the x and y components are positive
    /// </summary>
    /// <param name="value">the vector</param>
    /// <returns>A version of the vector, where the x and y components are positive</returns>
    public static Vector2Int Abs(Vector2Int value)
    {
        return new(Math.Abs(value.X), Math.Abs(value.Y));
    }
    /// <summary>
    /// Interpolates between value1 and value2
    /// </summary>
    /// <param name="value1">Start of the interpolation</param>
    /// <param name="value2">End of the interpolation</param>
    /// <param name="amount">How much of the interpolation should be done (0 = value1, 1 = value2)</param>
    /// <returns>The interpolated vector</returns>
    public static Vector2Int Lerp(Vector2Int value1, Vector2Int value2, float amount)
    {
        return (Vector2)value1 * (1.0f - amount) + (Vector2)value2 * amount;
    }
    
    /// <summary>
    /// Gets the bigger vector of the two
    /// </summary>
    /// <param name="value1">First vector</param>
    /// <param name="value2">Second vector</param>
    /// <returns>The bigger vector</returns>
    public static Vector2Int Max(Vector2Int value1, Vector2Int value2)
    {
        return new(
            value1.X > value2.X ? value1.X : value2.X,
            value1.Y > value2.Y ? value1.Y : value2.Y
        );
    }
    /// <summary>
    /// Gets the smaller vector of the two
    /// </summary>
    /// <param name="value1">First vector</param>
    /// <param name="value2">Second vector</param>
    /// <returns>The smaller vector</returns>
    public static Vector2Int Min(Vector2Int value1, Vector2Int value2)
    {
        return new(
            value1.X < value2.X ? value1.X : value2.X,
            value1.Y < value2.Y ? value1.Y : value2.Y
        );
    }

    public static Vector2 Sqrt(Vector2Int value)
    {
        return new(MathF.Sqrt(value.X), MathF.Sqrt(value.Y));
    }
    
    /// <summary>
    /// Turns the vector into a string
    /// </summary>
    /// <returns>string version of the vector</returns>
    public override string ToString()
    {
        return $"[{X}; {Y}]";
    }
}