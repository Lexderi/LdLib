using System.Drawing;
using LdLib.Vector;

namespace LdLib.Shapes;

public class Rectangle : Shape
{
    public Vector2 Pivot;
    public Vector2 Position;
    public float Rotation;
    public Vector2 Size;

    public Rectangle(Vector2 position, Vector2 size, Color color, float rotation = 0) : this(position, size,
        Vector2.Zero, color, rotation)
    {
    }

    public Rectangle(Vector2 position, Vector2 size, Vector2 pivot, Color color, float rotation = 0)
    {
        Position = position;
        Size = size;
        Pivot = pivot;
        Rotation = rotation;
        Color = color;
    }

    protected internal override (Vector2[] points, Vector2 position, Vector2 scale, float rotation)
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