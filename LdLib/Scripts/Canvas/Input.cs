using System.Diagnostics;
using LdLib.Vector;
using Silk.NET.Input;

namespace LdLib;

public static class Input
{
    private static bool mouseFound;

    private static IInputContext? input;

    private static IMouse mouse = null!;

    public static bool MouseDown { get; private set; }

    public static Vector2 MousePosition
    {
        get
        {
            if (!mouseFound) throw new("The canvas was either not initialized or there were no connected mice found");
            return mouse.Position;
        }
    }

    public static float MouseScrollDelta
    {
        get
        {
            if (!mouseFound) throw new("The canvas was either not initialized or there were no connected mice found");
            return mouse.ScrollWheels[0].Y;
        }
    }

    internal static void Initialize(IInputContext inputContext)
    {
        input = inputContext;

        // assign mouse events
        if (input.Mice.Count == 0)
        {
            Debug.WriteLine("WARNING: No connected mice found");
        }
        else
        {
            mouse = input.Mice[0];

            mouse.MouseDown += OnMouseDown;
            mouse.MouseUp += OnMouseUp;

            mouseFound = true;
        }
    }

    private static void OnMouseDown(IMouse mouse, MouseButton mouseButton)
    {
        MouseDown = true;
    }

    private static void OnMouseUp(IMouse mouse, MouseButton mouseButton)
    {
        MouseDown = false;
    }
}