using System.Drawing;
using LdLib.Vector;

namespace LdLib.Shapes;

public class Polygon : Shape
{
    public List<Vector2> Points;

    public Polygon(IEnumerable<Vector2> points, Color color)
    {
        Points = points.ToList();
        Color = color;
    }

    protected internal override (Vector2[] points, Vector2 position, Vector2 scale, float rotation)
        GetNormalizedPoints()
    {
        Vector2[] points = Points.ToArray();

        for (int i = 0; i < points.Length; i++) points[i] = NormalizePosition(points[i]);

        return (points, Vector2.Zero, Vector2.One, 0);
    }
}