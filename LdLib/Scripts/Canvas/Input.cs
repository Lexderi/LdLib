using System.Diagnostics;
using LdLib.Vector;
using Silk.NET.Input;

namespace LdLib;

public static class Input
{
    private static bool mouseFound;

    private static IInputContext? input;

    private static IMouse mouse = null!;

    /// <summary>
    /// If any button on the mouse is pressed
    /// </summary>
    public static bool MousePressed { get; private set; }

    private static int mouseButtonsPressed;
    private static int mouseButtonsDown;
    private static int mouseButtonsUp;
    
    /// <summary>
    /// Position of the mouse
    /// </summary>
    /// <exception cref="Exception">Thrown when the canvas has not been initialized yet</exception>
    public static Vector2 MousePosition
    {
        get
        {
            if (!mouseFound) throw new("The canvas was either not initialized or there were no connected mice found");
            return mouse.Position;
        }
    }
    
    /// <summary>
    /// Delta of the mouse scroll in the current frame
    /// </summary>
    /// <exception cref="Exception">Thrown when the canvas has not been initialized yet</exception>
    public static float MouseScrollDelta
    {
        get
        {
            if (!mouseFound) throw new("The canvas was either not initialized or there were no connected mice found");
            return mouse.ScrollWheels[0].Y;
        }
    }
    /// <summary>
    /// Checks if the mouse button is currently being pressed
    /// </summary>
    /// <param name="mouseButton">
    /// The mouse button to check. 
    /// 0 = left click, 1 = right click, 2 = middle click
    /// </param>
    /// <returns>If the button is currently being pressed</returns>
    public static bool GetMouseButtonPressed(int mouseButton) => CheckEncoding(mouseButtonsPressed, mouseButton);
    
    /// <summary>
    /// Checks if the mouse button is being pressed down this frame
    /// </summary>
    /// <param name="mouseButton">
    /// The mouse button to check. 
    /// 0 = left click, 1 = right click, 2 = middle click
    /// </param>
    /// <returns>If the button is being pressed down this frame</returns>
    public static bool GetMouseButtonDown(int mouseButton) => CheckEncoding(mouseButtonsDown, mouseButton);
    /// <summary>
    /// Checks if the mouse button is being released this frame
    /// </summary>
    /// <param name="mouseButton">
    /// The mouse button to check. 
    /// 0 = left click, 1 = right click, 2 = middle click
    /// </param>
    /// <returns>If the button is being released this frame</returns>
    public static bool GetMouseButtonUp(int mouseButton) => CheckEncoding(mouseButtonsUp, mouseButton);
    
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
        if (mouseButton == MouseButton.Unknown) return;
        
        MousePressed = true;
        
        // encode mouse button in pressed mouse buttons
        AddEncoding(ref mouseButtonsDown, (int)mouseButton);
        AddEncoding(ref mouseButtonsPressed, (int)mouseButton);
    }

    private static void OnMouseUp(IMouse mouse, MouseButton mouseButton)
    {   
        if (mouseButton == MouseButton.Unknown) return;
        
        MousePressed = false;
        
        // encode mouse button in pressed mouse buttons
        AddEncoding(ref mouseButtonsUp, (int)mouseButton);
        RemoveEncoding(ref mouseButtonsPressed, (int)mouseButton);
    }
    
    internal static void ResetInput()
    {
        mouseButtonsDown = 0;
        mouseButtonsUp = 0;
    } 
    private static void AddEncoding(ref int code, int n)
    {
        // sets bit at n to 1
        code |= 1 << n;
    }

    private static void RemoveEncoding(ref int code, int n)
    {
        // sets bit at n to 0
        code &= ~(1 << n);
    }

    private static bool CheckEncoding(int code, int n) => (code & (1 << n)) != 0;

      
}