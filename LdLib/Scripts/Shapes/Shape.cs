using System.Drawing;
using LdLib.Vector;
using Silk.NET.Maths;
using Silk.NET.OpenGL;

namespace LdLib.Shapes;

public abstract class Shape : IDisposable
{
    private const string vertexShaderSource =
        @"#version 330 core
            layout (location = 0) in vec4 vPos;
            
            uniform vec4 current_color;
            out vec4 out_color;

            void main()
            {   
                out_color = current_color;
                gl_Position = vec4(vPos.x, vPos.y, vPos.z, 1.0);
            }
            ";

    private const string fragmentShaderSource =
        @"#version 330 core
            out vec4 FragColor;         
            
            in vec4 out_color;

            void main()
            {
                FragColor = out_color;
            }
            ";

    internal static List<Shape> All = new();

    private static readonly List<Shape> DestroyQueue = new();

    public Color Color;

    internal bool DestroyAfterDraw;

    public bool Destroyed;

    private float prevRotation;
    private Matrix2X2<float> prevRotationMatrix = Matrix2X2<float>.Identity;

    // caches for calculating transform
    private Vector2 prevScale = Vector2.One;
    private Matrix2X2<float> prevScaleMatrix = Matrix2X2<float>.Identity;

    private Matrix2X2<float> prevTransform = Matrix2X2<float>.Identity;

    internal Shape()
    {
        All.Add(this);
    }

    protected static uint ShaderProgram { get; private set; }

    public void Dispose()
    {
        Destroy();
    }

    internal static void CompileShaders()
    {
        // compile vertex shaded
        uint vertexShader = Canvas.Gl.CreateShader(ShaderType.VertexShader);
        Canvas.Gl.ShaderSource(vertexShader, vertexShaderSource);
        Canvas.Gl.CompileShader(vertexShader);

        // check for errors
        string infoLog = Canvas.Gl.GetShaderInfoLog(vertexShader);
        if (!string.IsNullOrWhiteSpace(infoLog)) Console.WriteLine($"Error compiling vertex shader {infoLog}");

        // compile fragment shader
        uint fragmentShader = Canvas.Gl.CreateShader(ShaderType.FragmentShader);
        Canvas.Gl.ShaderSource(fragmentShader, fragmentShaderSource);
        Canvas.Gl.CompileShader(fragmentShader);

        // check for errors
        infoLog = Canvas.Gl.GetShaderInfoLog(fragmentShader);
        if (!string.IsNullOrWhiteSpace(infoLog)) Console.WriteLine($"Error compiling fragment shader {infoLog}");

        // link shaders
        ShaderProgram = Canvas.Gl.CreateProgram();

        Canvas.Gl.AttachShader(ShaderProgram, vertexShader);
        Canvas.Gl.AttachShader(ShaderProgram, fragmentShader);
        Canvas.Gl.LinkProgram(ShaderProgram);

        // check for errors
        infoLog = Canvas.Gl.GetProgramInfoLog(ShaderProgram);
        if (!string.IsNullOrWhiteSpace(infoLog)) Console.WriteLine($"Error compiling shader program {infoLog}");
        // delete individual shaders
        Canvas.Gl.DetachShader(ShaderProgram, vertexShader);
        Canvas.Gl.DetachShader(ShaderProgram, fragmentShader);
        Canvas.Gl.DeleteShader(vertexShader);
        Canvas.Gl.DeleteShader(fragmentShader);
    }

    internal static void DeleteProgram()
    {
        Canvas.Gl.DeleteProgram(ShaderProgram);
    }

    internal static void RenderAll()
    {
        //Canvas.Gl.PolygonMode(GLEnum.FrontAndBack, GLEnum.Line); // Wireframe mode

        Canvas.Gl.UseProgram(ShaderProgram);
        foreach (Shape shape in All) shape.Render();

        // clear destroy queue
        foreach (Shape shape in DestroyQueue) All.Remove(shape);
    }

    internal unsafe void Render()
    {
        (Vector2[] points, Vector2 position, Vector2 scale, float rotation) = GetNormalizedPoints();

        points = Transform(points, position, scale, rotation);

        // create indices
        uint[] indices = new uint[(points.Length - 2) * 3];

        for (uint i = 0; i < points.Length - 2; i++)
        for (uint j = 0; j < 3; j++)
            if (j == 0) indices[i * 3 + j] = 0;
            else indices[i * 3 + j] = i + j;

        // create vertices
        float[] vertices = new float[points.Length * 3];

        for (int i = 0; i < points.Length; i++)
        {
            vertices[i * 3] = points[i].X;
            vertices[i * 3 + 1] = points[i].Y;
            vertices[i * 3 + 2] = 0;
        }

        uint vertexArrayObject = Canvas.Gl.GenVertexArray();

        Canvas.Gl.BindVertexArray(vertexArrayObject);

        // send vertices to gpu
        uint vertexBufferObject = Canvas.Gl.GenBuffer();
        Canvas.Gl.BindBuffer(BufferTargetARB.ArrayBuffer, vertexBufferObject);
        Canvas.Gl.BufferData(BufferTargetARB.ArrayBuffer, (ReadOnlySpan<float>)vertices.AsSpan(), GLEnum.DynamicDraw);

        //// send color to gpu
        //uint color = Canvas.Gl.GenBuffer();
        //Canvas.Gl.BindBuffer(BufferTargetARB.ArrayBuffer, color);
        //Canvas.Gl.BufferData(BufferTargetARB.ArrayBuffer, (ReadOnlySpan<float>) colorArray.AsSpan(), GLEnum.DynamicDraw);
        //Canvas.Gl.VertexAttribPointer(1, 4, GLEnum.Float, false, 4 * sizeof(float), null);

        //Canvas.Gl.EnableVertexAttribArray(1);


        // tell open gl how to give the data to the shaders
        Canvas.Gl.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), null);
        Canvas.Gl.EnableVertexAttribArray(0);

        // Initializing a element buffer that holds the index data.
        uint ebo = Canvas.Gl.GenBuffer(); // Creating the buffer.
        Canvas.Gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, ebo); // Binding the buffer.
        fixed (void* i = &indices[0])
        {
            Canvas.Gl.BufferData(BufferTargetARB.ElementArrayBuffer, (nuint)(indices.Length * sizeof(uint)), i,
                BufferUsageARB.DynamicDraw); // Setting buffer data.
        }

        int colorLocation = Canvas.Gl.GetUniformLocation(ShaderProgram, "current_color");
        Canvas.Gl.Uniform4(colorLocation, Color.R / 255f, Color.G / 255f, Color.B / 255f, Color.A / 255f);

        Canvas.Gl.BindVertexArray(vertexArrayObject);
        Canvas.Gl.DrawElements(GLEnum.Triangles, (uint)indices.Length, GLEnum.UnsignedInt, null);

        Canvas.Gl.DeleteBuffer(vertexBufferObject);
        Canvas.Gl.DeleteBuffer(vertexArrayObject);
        Canvas.Gl.DeleteBuffer(ebo);

        if (DestroyAfterDraw) Destroy();
    }

    protected internal abstract (Vector2[] points, Vector2 position, Vector2 scale, float rotation)
        GetNormalizedPoints();

    public void Destroy()
    {
        DestroyQueue.Add(this);
        Destroyed = true;
    }

    private Vector2[] Transform(Vector2[] points, Vector2 position, Vector2 scale, float rotation)
    {
        // calculate scale matrix
        Matrix2X2<float> scaleMatrix;

        bool scaleChanged = prevScale != scale;
        bool rotationChanged = Math.Abs(prevRotation - rotation) > 0.001f;

        // check cache
        if (!scaleChanged)
        {
            scaleMatrix = prevScaleMatrix;
        }
        else
        {
            // generate scale matrix
            scaleMatrix = new(scale.X, 0, 0, scale.Y);

            // cache values
            prevScale = scale;
            prevScaleMatrix = scaleMatrix;
        }

        // calculate rotation matrix
        Matrix2X2<float> rotationMatrix;

        // check cache
        if (!rotationChanged)
        {
            rotationMatrix = prevRotationMatrix;
        }
        else
        {
            // generate rotation matrix
            float cos = MathF.Cos(rotation);
            float sin = MathF.Sin(rotation);

            rotationMatrix = new(cos, -sin, sin, cos);

            // cache values
            prevRotation = rotation;
            prevRotationMatrix = rotationMatrix;
        }

        // calculate transform
        Matrix2X2<float> transform;
        // check cache
        if (!scaleChanged && !rotationChanged)
        {
            transform = prevTransform;
        }
        else
        {
            // calculate transform
            transform = rotationMatrix * scaleMatrix;

            // cache values
            prevTransform = transform;
        }

        for (int i = 0; i < points.Length; i++) points[i] = transform * points[i] + position;


        return points;
    }

    protected internal static Vector2 NormalizePosition(Vector2 position)
    {
        position.Y = Canvas.Settings.Size.Y - position.Y;
        Vector2 normalizedPosition = position * 2 / (Vector2)Canvas.Settings.Size - Vector2.One;
        return normalizedPosition;
    }

    protected internal static Vector2 NormalizeScale(Vector2 size)
    {
        Vector2 normalizedSize = size / (Vector2)Canvas.Settings.Size * 2;
        return normalizedSize;
    }
}