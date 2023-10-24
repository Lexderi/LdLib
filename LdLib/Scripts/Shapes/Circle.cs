using System.Drawing;
using LdLib.Vector;

namespace LdLib.Shapes;

public class Circle : Shape
{
    private static Vector2[] mesh = null!;
    public float Diameter;
    public Vector2 Position;

    public Circle(Vector2 position, float diameter, Color color)
    {
        Position = position;
        Diameter = diameter;
        Color = color;
    }

    internal static void GenerateMesh(int precision)
    {
        mesh = new Vector2[precision];

        // go around circle and save points
        for (int i = 0; i < precision; i++)
        {
            float arcLength = 2 * MathF.PI * i / precision;

            float x = MathF.Sin(arcLength);
            float y = MathF.Cos(arcLength);

            mesh[i] = new(x, y);
        }
    }

    protected internal override (Vector2[] points, Vector2 position, Vector2 scale, float rotation)
        GetNormalizedPoints()
    {
        Vector2 normalizedPosition = NormalizePosition(Position);
        Vector2 normalizedScale = NormalizeScale(new(Diameter));

        Vector2[] meshCopy = new Vector2[mesh.Length];
        Array.Copy(mesh, meshCopy, mesh.Length);

        return (meshCopy, normalizedPosition, normalizedScale, 0);
    }
}