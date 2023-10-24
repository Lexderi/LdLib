using System.Drawing;
using LdLib.Vector;

namespace LdLib.Shapes;

/// <summary>
/// A line that gets drawn to the canvas
/// THIS OBJECT HAS TO BE DESTROYED MANUALLY
/// </summary>
public class Line : Shape
{
    /// <summary>
    /// Start of the line in pixels
    /// </summary>
    public Vector2 Start;
    /// <summary>
    /// End of the line in pixels
    /// </summary>
    public Vector2 End;
    /// <summary>
    /// Width of the line in pixels
    /// </summary>
    public float Weight;
    
    /// <summary>
    /// A line that gets drawn to the canvas
    /// </summary>
    /// <param name="start">Start of the line in pixels</param>
    /// <param name="end">End of the line in pixels</param>
    /// <param name="weight">Width of the line in pixels</param>
    /// <param name="color">Color of the line</param>
    public Line(Vector2 start, Vector2 end, float weight, Color color)
    {
        Start = start;
        End = end;
        Weight = weight;
        Color = color;
    }

    protected override (Vector2[] points, Vector2 position, Vector2 scale, float rotation)
        GetNormalizedPoints()
    {
        Vector2[] points =
        {
            new(0, -0.5f),
            new(0, 0.5f),
            new(1, -0.5f),
            new(1, 0.5f)
        };

        Vector2 normalizedStart = NormalizePosition(Start);
        Vector2 delta = End - Start;
        float rotation = -delta.Rotation;
        Vector2 normalizedSize = NormalizeScale(new(delta.Magnitude, Weight));

        return (points, normalizedStart, normalizedSize, rotation);
    }
}