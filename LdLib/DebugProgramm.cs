using LdLib;
using LdLib.Shapes;
using LdLib.Vector;
using Color = System.Drawing.Color;

public class DebugProgram : CanvasObject
{
    private static DebugProgram instance = null!;

    private Line line;

    public static void Main(string[] _)
    {
        instance = new();

        Canvas.Initialize(instance.Load);
    }

    private void Load()
    {
        line = new(new(750), new(500), 10, Color.SkyBlue);
    }

    protected override void Update()
    {
        if (Input.MouseDown)
        {
            line.End = Input.MousePosition;
        }
    }
}
