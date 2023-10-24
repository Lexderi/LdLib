using System.Drawing;
using LdLib.Vector;

namespace LdLib.Shapes;

/// <summary>
/// A polygon that gets drawn to the canvas
/// THIS OBJECT HAS TO BE DESTROYED MANUALLY
/// </summary>
public class Polygon : Shape
{
    /// <summary>
    /// Corner points of the polygon
    /// </summary>
    public List<Vector2> Points;
    
    /// <summary>
    /// A polygon that gets drawn to the canvas
    /// </summary>
    /// <param name="points">Corner points of the polygon</param>
    /// <param name="color">Color of the polygon</param>
    public Polygon(IEnumerable<Vector2> points, Color color)
    {
        Points = points.ToList();
        Color = color;
    }

    protected override (Vector2[] points, Vector2 position, Vector2 scale, float rotation)
        GetNormalizedPoints()
    {
        Vector2[] points = Points.ToArray();

        for (int i = 0; i < points.Length; i++) points[i] = NormalizePosition(points[i]);

        return (points, Vector2.Zero, Vector2.One, 0);
    }
}