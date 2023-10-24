using System.Drawing;
using System.Runtime.InteropServices;
using LdLib.Shapes;
using LdLib.Vector;
using Microsoft.VisualBasic.CompilerServices;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using Rectangle = LdLib.Shapes.Rectangle;

namespace LdLib;

/// <summary>
/// Canvas that opens a window and that can be drawn to
/// The drawing works as a state machine
/// </summary>
public static class Canvas
{
    /// <summary>
    /// Settings of the window
    /// </summary>
    public static CanvasSettings Settings;

    internal static IWindow Window = null!;
    internal static bool Initialized;

    private static Vector2D<int> prevWindowSize;
    private static Vector2D<int> prevWindowPosition;

    internal static GL Gl = null!;
    private static Action? loadCallback;

    // Drawing Variables
    /// <summary>
    /// Width of the lines in pixels
    /// </summary>
    public static float Weight = 10;
    /// <summary>
    /// Color of the shapes
    /// </summary>
    public static Color Color = Color.White;
    /// <summary>
    /// Pivot of the rectangles
    /// (0, 0) is the top left and (1, 1) the bottom right
    /// </summary>
    public static Vector2 Pivot = Vector2.Zero;

    /// <summary>
    /// Initializes the canvas
    /// CODE AFTER THIS CALL WILL NOT BE EXECUTED
    /// </summary>
    /// <param name="loadCallback">Will be executed once after the canvas has been initialized</param>
    public static void Initialize(Action? loadCallback = null)
    {
        Initialize(CanvasSettings.Default, loadCallback);
    }

    /// <summary>
    /// Initializes the canvas
    /// CODE AFTER THIS CALL WILL NOT BE EXECUTED
    /// </summary>
    /// <param name="settings">Settings of the window</param>
    /// <param name="loadCallback">Will be executed once after the canvas has been initialized</param>
    public static void Initialize(CanvasSettings settings, Action? loadCallback = null)
    {
        Settings = settings;

        // create window
        Window = Silk.NET.Windowing.Window.Create(WindowOptions.Default);
        Settings.UpdateCanvasWindow();

        // innit callbacks
        Window.Render += Render;
        Window.Update += Update;
        Window.Load += Load;
        Window.Closing += OnClose;
        Canvas.loadCallback = loadCallback;

        // set variables
        Initialized = true;
        prevWindowSize = new(Settings.Size.X, Settings.Size.Y);
        prevWindowPosition = new(Settings.Position.X, Settings.Position.Y);

        Window.Run();
    }

    private static void Render(double deltaTime)
    {
        // update local settings
        if (prevWindowSize.X != Window.Size.X || prevWindowSize.Y != Window.Size.Y)
        {
            Settings.Size = Window.Size;
            prevWindowSize = Window.Size;
        }

        if (prevWindowPosition != Window.Position)
        {
            Settings.Position = Window.Position;
            prevWindowPosition = Window.Position;
        }

        // update window settings
        Settings.UpdateCanvasWindow();

        // draw background
        if (Settings.DefaultBackground != null) Gl.Clear(ClearBufferMask.ColorBufferBit);


        // render shapes
        Shape.RenderAll();
    }
    private static void Update(double deltaTime)
    {
        // update time
        Time.UpdateDelta = (float)deltaTime;

        // execute all updates
        foreach (CanvasObject canvasObject in CanvasObject.All) canvasObject.UpdateInternal();
        
        Input.ResetInput();
    }

    private static void Load()
    {
        Input.Initialize(Window.CreateInput());

        Gl = Window.CreateOpenGL();

        // init message callbacks
        Gl.Enable(EnableCap.DebugOutput);
        Gl.DebugMessageCallback(MessageCallback, 0);

        // set clear color
        if (Settings.DefaultBackground != null)
            Gl.ClearColor((Color)Settings.DefaultBackground);

        // compile/create/generate stuff
        Shape.CompileShaders();

        Circle.GenerateMesh(100);

        loadCallback?.Invoke();
    }

    private static void MessageCallback(GLEnum source, GLEnum type, int id, GLEnum severity, int length, nint message,
        nint userParam)
    {
        string typeString = type == GLEnum.DebugTypeError ? "** GL ERROR **" : $"type = {type},";

        string? msg = Marshal.PtrToStringAnsi(message);
        Console.WriteLine($"GL CALLBACK: {typeString} severity = {severity}, message = {msg}");
    }

    private static void OnClose()
    {
        Shape.DeleteProgram();
    }

    #region Drawing
    /// <summary>
    /// Draws a Polygon to the canvas
    /// </summary>
    /// <param name="points">Points of the corners</param>
    /// <exception cref="Exception">Thrown when the canvas has not been initialized yet</exception>
    public static void DrawPolygon(IEnumerable<Vector2> points)
    {
        if (!Initialized) throw new("Canvas has not been initialized yet");
        
        _ = new Polygon(points, Color)
        {
            DestroyAfterDraw = true
        };
    }
    /// <summary>
    /// Draws a rectangle to the canvas
    /// </summary>
    /// <param name="position">Position of the rectangle</param>
    /// <param name="size">Size of the rectangle</param>
    /// <param name="rotation">Rotation of the rectangle</param>
    /// <exception cref="Exception">Thrown when the canvas has not been initialized yet</exception>
    public static void DrawRectangle(Vector2 position, Vector2 size, float rotation = 0f)
    {
        if (!Initialized) throw new("Canvas has not been initialized yet");
        
        
        
        _ = new Rectangle(position, size, Pivot, Color, rotation)
        {
            DestroyAfterDraw = true
        };
    }
    /// <summary>
    /// Draws a circle to the canvas
    /// </summary>
    /// <param name="position">Position of the circle</param>
    /// <param name="diameter">Diameter of the circle</param>
    /// <exception cref="Exception">Thrown when the canvas has not been initialized yet</exception>
    public static void DrawCircle(Vector2 position, float diameter)
    {
        if (!Initialized) throw new("Canvas has not been initialized yet");
        
        _ = new Circle(position, diameter, Color)
        {
            DestroyAfterDraw = true
        };
    }
    /// <summary>
    /// Draws a line to the canvas
    /// </summary>
    /// <param name="start">Start of the line</param>
    /// <param name="end">End of the line</param>
    /// <exception cref="Exception">Thrown when the canvas has not been initialized yet</exception>
    public static void DrawLine(Vector2 start, Vector2 end)
    {
        if (!Initialized) throw new("Canvas has not been initialized yet");
        
        _ = new Line(start, end, Weight, Color)
        {
            DestroyAfterDraw = true
        };
    }

    #endregion
}

/// <summary>
/// Window settings of the canvas
/// </summary>
public struct CanvasSettings
{
    /// <summary>
    /// Size of the Window in pixels
    /// </summary>
    public Vector2Int Size;
    /// <summary>
    /// Position of the Window in pixels
    /// </summary>
    public Vector2Int Position;
    /// <summary>
    /// Title on the top left of the window
    /// </summary>
    public string Title;
    /// <summary>
    /// Frame rate at which the canvas updates and renders
    /// </summary>
    public int FrameRate;
    /// <summary>
    /// If the window is visible
    /// </summary>
    public bool Visible;

    private Color? defaultBackground;
    
    /// <summary>
    /// automatic background, if null the background won't be drawn
    /// </summary>
    public Color? DefaultBackground
    {
        get => defaultBackground;
        set
        {
            defaultBackground = value;

            if (value != null) Canvas.Gl?.ClearColor((Color)value);
        }
    }

    /// <summary>
    /// Default canvas settings
    /// </summary>
    public static readonly CanvasSettings Default = new(new(1000), new(50), "My Project", 60, true);
    
    /// <summary>
    /// Window settings of the canvas
    /// </summary>
    /// <param name="size">Size of the Window in pixels</param>
    /// <param name="position">Position of the Window in pixels</param>
    /// <param name="title">Title on the top left of the window</param>
    /// <param name="frameRate">Frame rate at which the canvas updates and renders</param>
    /// <param name="visible">If the window is visible</param>
    /// <param name="defaultBackground">automatic background, if null the background won't be drawn</param>
    public CanvasSettings(Vector2Int size, Vector2Int position, string title, int frameRate, bool visible,
        Color? defaultBackground)
    {
        Size = size;
        Position = position;
        Title = title;
        FrameRate = frameRate;
        Visible = visible;
        this.defaultBackground = defaultBackground;
    }
    
    /// <summary>
    /// Window settings of the canvas
    /// </summary>
    /// <param name="size">Size of the Window in pixels</param>
    /// <param name="position">Position of the Window in pixels</param>
    /// <param name="title">Title on the top left of the window</param>
    /// <param name="frameRate">Frame rate at which the canvas updates and renders</param>
    /// <param name="visible">If the window is visible</param>
    public CanvasSettings(Vector2Int size, Vector2Int position, string title, int frameRate, bool visible) : this(size,
        position, title, frameRate, visible, Color.Black)
    {
    }

    internal void UpdateCanvasWindow()
    {
        Vector2D<int> newSize = new(Size.X, Size.Y);
        if (newSize != Canvas.Window.Size) Canvas.Window.Size = newSize;

        Vector2D<int> newPos = new(Position.X, Position.Y);
        if (newPos != Canvas.Window.Position) Canvas.Window.Position = newPos;

        if (Title != Canvas.Window.Title) Canvas.Window.Title = Title;

        if (Math.Abs(FrameRate - Canvas.Window.FramesPerSecond) > 0.1f) Canvas.Window.FramesPerSecond = FrameRate;

        if (Visible != Canvas.Window.IsVisible) Canvas.Window.IsVisible = Visible;
    }
}