using Silk.NET.Maths;

namespace LdLib.Vector;

/// <summary>
/// A 2 dimensional vector containing floats
/// </summary>
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

    /// <summary>
    /// X component of the vector
    /// </summary>
    public float X { get; set; }
    /// <summary>
    /// Y component of the vector
    /// </summary>
    public float Y { get; set; }

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
    public readonly Vector2 Normalized => new Vector2(X, Y) / Magnitude;
    
    /// <summary>
    /// Rotation of the vector in radians
    /// </summary>
    public readonly float Rotation
    {
        get => MathF.Atan2(Y, X);
        set => new Vector2(Magnitude, 0).Rotate(value);
    }
    
    /// <summary>
    /// A 2 dimensional vector containing floats
    /// </summary>
    /// <param name="xy">X and y component of the vector</param>
    public Vector2(float xy)
    {
        this = new(xy, xy);
    }
    /// <summary>
    /// A 2 dimensional vector containing floats
    /// </summary>
    /// <param name="x">X component of the vector</param>
    /// <param name="y">Y component of the vector</param>
    public Vector2(float x, float y)
    {
        X = x;
        Y = y;
    }

    #region Operators

    public static Vector2 operator +(Vector2 a, Vector2 b)
    {
        return new(a.X + b.X, a.Y + b.Y);
    }

    public static Vector2 operator -(Vector2 a, Vector2 b)
    {
        return new(a.X - b.X, a.Y - b.Y);
    }

    public static Vector2 operator -(Vector2 a)
    {
        return new(-a.X, -a.Y);
    }

    public static Vector2 operator *(Vector2 v, float scalar)
    {
        return new(v.X * scalar, v.Y * scalar);
    }

    public static Vector2 operator /(Vector2 v, float scalar)
    {
        return new(v.X / scalar, v.Y / scalar);
    }

    public static Vector2 operator *(Vector2 a, Vector2 b)
    {
        return new(a.X * b.X, a.Y * b.Y);
    }

    public static Vector2 operator /(Vector2 a, Vector2 b)
    {
        return new(a.X / b.X, a.Y / b.Y);
    }

    public static bool operator ==(Vector2 left, Vector2 right)
    {
        return left.X.Equals(right.X) && left.Y.Equals(right.Y);
    }

    public static bool operator !=(Vector2 left, Vector2 right)
    {
        return !(left == right);
    }

    public static implicit operator Vector2(Vector2Int v)
    {
        return new(v.X, v.Y);
    }

    public static implicit operator Vector2(System.Numerics.Vector2 v)
    {
        return new(v.X, v.Y);
    }

    public static Vector2 operator *(Matrix2X2<float> m, Vector2 v)
    {
        float x = m.M11 * v.X + m.M12 * v.Y;
        float y = m.M21 * v.X + m.M22 * v.Y;

        return new(x, y);
    }

    #endregion
    
    /// <summary>
    /// Limits the magnitude of the vector
    /// </summary>
    /// <param name="maxMagnitude">the maximum magnitude the vector can have</param>
    /// <returns>The vector</returns>
    public Vector2 Limit(float maxMagnitude)
    {
        if (Magnitude > maxMagnitude) Magnitude = maxMagnitude;

        return this;
    }
    /// <summary>
    /// Checks if the x and y components equal each other;
    /// </summary>
    /// <param name="other">the vector that should be compared with</param>
    /// <returns>If the vectors are equal</returns>
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
    
    /// <summary>
    /// Gets the version of the vector, where the x and y components are positive
    /// </summary>
    /// <param name="value">the vector</param>
    /// <returns>A version of the vector, where the x and y components are positive</returns>
    public static Vector2 Abs(Vector2 value)
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
    public static Vector2 Lerp(Vector2 value1, Vector2 value2, float amount)
    {
        return value1 * (1.0f - amount) + value2 * amount;
    }
    
    /// <summary>
    /// Gets the bigger vector of the two
    /// </summary>
    /// <param name="value1">First vector</param>
    /// <param name="value2">Second vector</param>
    /// <returns>The bigger vector</returns>
    public static Vector2 Max(Vector2 value1, Vector2 value2)
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
    public static Vector2 Min(Vector2 value1, Vector2 value2)
    {
        return new(
            value1.X < value2.X ? value1.X : value2.X,
            value1.Y < value2.Y ? value1.Y : value2.Y
        );
    }

    /// <summary>
    /// Gets the square root of the x and y components
    /// </summary>
    /// <param name="value">vector where the square root should be got from</param>
    /// <returns>the square root of the x and y components</returns>
    public static Vector2 Sqrt(Vector2 value)
    {
        return new(MathF.Sqrt(value.X), MathF.Sqrt(value.Y));
    }
    
    /// <summary>
    /// Rotates the vector
    /// </summary>
    /// <param name="angle">the rotation angle in radians</param>
    /// <returns>The rotated vector</returns>
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

    /// <summary>
    /// Turns the vector into a string
    /// </summary>
    /// <returns>string version of the vector</returns>
    public override string ToString()
    {
        return $"[{X}; {Y}]";
    }
}