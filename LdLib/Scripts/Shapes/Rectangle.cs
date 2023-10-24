using System.Drawing;
using LdLib.Vector;

namespace LdLib.Shapes;

/// <summary>
/// A rectangle that gets drawn to the canvas
/// THIS OBJECT HAS TO BE DESTROYED MANUALLY
/// </summary>
public class Rectangle : Shape
{
    /// <summary>
    /// Pivot of the rectangle
    /// (0, 0) is the top left and (1, 1) the bottom right
    /// </summary>
    public Vector2 Pivot;
    /// <summary>
    /// Position of the rectangle in pixels
    /// </summary>
    public Vector2 Position;
    /// <summary>
    /// Rotation of the rectangle in radians
    /// </summary>
    public float Rotation;
    /// <summary>
    /// Size of the rectangle in pixels
    /// </summary>
    public Vector2 Size;

    /// <summary>
    /// A rectangle that gets drawn to the canvas
    /// </summary>
    /// <param name="position">Position of the rectangle in pixels</param>
    /// <param name="size">Size of the rectangle in pixels</param>
    /// <param name="color">Color of the rectangle</param>
    /// <param name="rotation">Rotation of the rectangle in radians</param>
    public Rectangle(Vector2 position, Vector2 size, Color color, float rotation = 0) : this(position, size,
        Vector2.Zero, color, rotation)
    {
    }

    /// <summary>
    /// A rectangle that gets drawn to the canvas
    /// </summary>
    /// <param name="position">Position of the rectangle in pixels</param>
    /// <param name="size">Size of the rectangle in pixels</param>
    /// <param name="pivot">Pivot of the rectangle. 
    /// (0, 0) is the top left and (1, 1) the bottom right</param>
    /// <param name="color">Color of the rectangle</param>
    /// <param name="rotation">Rotation of the rectangle in radians</param>
    public Rectangle(Vector2 position, Vector2 size, Vector2 pivot, Color color, float rotation = 0)
    {
        Position = position;
        Size = size;
        Pivot = pivot;
        Rotation = rotation;
        Color = color;
    }

    protected override (Vector2[] points, Vector2 position, Vector2 scale, float rotation)
        GetNormalizedPoints()
    {
        Vector2 normalizedPosition = NormalizePosition(Position);
        Vector2 normalizedSize = NormalizeScale(Size);

        Vector2[] points =
        {
            -Pivot,
            Vector2.Right - Pivot,
            Vector2.Up - Pivot,
            Vector2.One - Pivot
        };

        return (points, normalizedPosition, normalizedSize, Rotation);
    }
}