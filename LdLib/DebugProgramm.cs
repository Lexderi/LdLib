using LdLib;
using LdLib.Shapes;
using LdLib.Vector;
using Color = System.Drawing.Color;

public class DebugProgram : CanvasObject
{
    private static DebugProgram instance = null!;

    private Polygon polygon;

    public static void Main(string[] _)
    {
        instance = new();

        Canvas.Initialize(instance.Load);
    }

    private void Load()
    {
        polygon = new(new List<Vector2>() { new(250), new(500, 250), new(250, 500) }, Color.MintCream);
    }

    protected override void Update()
    {
        if (Input.MouseDown)
        {
            polygon.Points[0] = Input.MousePosition;
        }
    }
}
