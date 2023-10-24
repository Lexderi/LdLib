using System.Drawing;
using LdLib;
using LdLib.Shapes;

public class DebugProgram : CanvasObject
{
    private static DebugProgram instance = null!;

    private Circle circle;

    public static void Main(string[] _)
    {
        instance = new();

        Canvas.Initialize(instance.Load);
    }

    private void Load()
    {
        Canvas.Color = Color.HotPink;
    }

    protected override void Update()
    {
        Canvas.DrawCircle(Input.MousePosition, 50);
    }
}