using System.Drawing;
using System.Runtime.InteropServices;
using LdLib.Shapes;
using LdLib.Vector;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using Rectangle = LdLib.Shapes.Rectangle;

namespace LdLib
{
    public static class Canvas
    {
        public static CanvasSettings Settings;

        internal static IWindow Window = null!;
        internal static bool Initialized;

        private static Vector2D<int> prevWindowSize;
        private static Vector2D<int> prevWindowPosition;

        internal static GL Gl = null!;
        private static Action? loadCallback;

        // Drawing Variables
        public static float Weight = 10;
        public static Color Color = Color.White;
        public static Vector2 Pivot = Vector2.Zero;

        public static void Initialize(Action? loadCallback = null)
        {
            Initialize(CanvasSettings.Default, loadCallback);
        }
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
                Settings.Size = new(Window.Size);
                prevWindowSize = Window.Size;
            }
            if (prevWindowPosition != Window.Position)
            {
                Settings.Position = new(Window.Position);
                prevWindowPosition = Window.Position;
            }

            // update window settings
            Settings.UpdateCanvasWindow();

            // draw background
            if (Settings.DefaultBackground != null)
            {
                Gl.Clear(ClearBufferMask.ColorBufferBit);
            }


            // render shapes
            Shape.RenderAll();
        }

        private static void Update(double deltaTime)
        {
            // update time
            Time.DeltaTime = (float)deltaTime;

            // execute all updates
            foreach (CanvasObject canvasObject in CanvasObject.All)
            {
                canvasObject.UpdateInternal();
            }
        }

        private static void Load()
        {
            Input.Initialize(Window.CreateInput());
            
            Gl = Window.CreateOpenGL();
            
            // init message callbacks
            Gl.Enable(EnableCap.DebugOutput);
            Gl.DebugMessageCallback(MessageCallback, 0);

            // set clear color
            if(Settings.DefaultBackground != null)
                Gl.ClearColor((Color)Settings.DefaultBackground);

            // compile/create/generate stuff
            Shape.CompileShaders();

            Shapes.Circle.GenerateMesh(100);
            
            loadCallback?.Invoke();
        }

        private static void MessageCallback(GLEnum source, GLEnum type, int id, GLEnum severity, int length, nint message, nint userParam )
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

        public static void DrawPolygon(IEnumerable<Vector2> points)
        {
            _ = new Polygon(points, Color)
            {
                DestroyAfterDraw = true
            };
        }

        public static void DrawRectangle(Vector2 position, Vector2 size, float rotation = 0f)
        {
            _ = new Rectangle(position, size, Pivot, Color, rotation)
            {
                DestroyAfterDraw = true
            };
        }

        public static void DrawCircle(Vector2 position, float diameter)
        {
            _ = new Circle(position, diameter, Color)
            {
                DestroyAfterDraw = true
            };
        }

        public static void DrawLine(Vector2 start, Vector2 end)
        {
            _ = new Line(start, end, Weight, Color)
            {
                DestroyAfterDraw = true
            };
        }
        #endregion
    }

    public struct CanvasSettings
    {
        public Vector2Int Size;
        public Vector2Int Position;
        public string Title;
        public int FrameRate;
        public bool Visible;

        private Color? defaultBackground;
        public Color? DefaultBackground
        {
            get => defaultBackground;
            set
            {
                defaultBackground = value;

                if (value != null)
                {
                    Canvas.Gl?.ClearColor((Color)value);
                }
            }
        }

        public static readonly CanvasSettings Default = new(new(1000), new(50), "My Project", 60, true);
        public CanvasSettings(Vector2Int size, Vector2Int position, string title, int frameRate, bool visible, Color? defaultBackground)
        {
            Size = size;
            Position = position;
            Title = title;
            FrameRate = frameRate;
            Visible = visible;
            this.defaultBackground = defaultBackground;
        }
        public CanvasSettings(Vector2Int size, Vector2Int position, string title, int frameRate, bool visible): this(size, position, title, frameRate, visible, Color.Black) { }
        
        internal void UpdateCanvasWindow()
        {
            Vector2D<int> newSize = new(Size.X, Size.Y);
            if (newSize != Canvas.Window.Size)
            {
                Canvas.Window.Size = newSize;
            }

            Vector2D<int> newPos = new(Position.X, Position.Y);
            if (newPos != Canvas.Window.Position)
            {
                Canvas.Window.Position = newPos;
            }

            if (Title != Canvas.Window.Title)
            {
                Canvas.Window.Title = Title;
            }

            if (Math.Abs(FrameRate - Canvas.Window.FramesPerSecond) > 0.1f)
            {
                Canvas.Window.FramesPerSecond = FrameRate;
            }

            if (Visible != Canvas.Window.IsVisible)
            {
                Canvas.Window.IsVisible = Visible;
            }
        }
    }
}