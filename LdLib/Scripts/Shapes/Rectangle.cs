using System.Drawing;
using LdLib.Vector;
using Silk.NET.OpenGL;

namespace LdLib.Shapes
{
    public class Rectangle: Shape
    {
        public Vector2 Size;
        public Vector2 Pivot;
        public float Rotation;

        //Index data, uploaded to the EBO.
        private static readonly uint[] indices =
        {
            0, 1, 2,
            1, 2, 3
        };


        public Rectangle(Vector2 position, Vector2 size, Color color, float rotation = 0): this(position, size, Vector2.Zero, color, rotation) { }

        public Rectangle(Vector2 position, Vector2 size, Vector2 pivot, Color color, float rotation = 0 )
        {
            Position = position;
            Size = size;
            Pivot = pivot;
            Rotation = rotation;
            Color = color;
        }

        protected internal override (Vector2[] points, Vector2 position, Vector2 scale, float rotation) GetNormalizedPoints()
        {
            Vector2 normalizedPosition = Position * 2 / (Vector2)Canvas.Settings.Size - Vector2.One;
            Vector2 normalizedSize = Size / (Vector2)Canvas.Settings.Size * 2;

            Vector2[] points =
            {
                -Pivot,
                new Vector2(1, 0) - Pivot,
                new Vector2(0, 1) - Pivot,
                Vector2.One - Pivot,
            };

            return (points, normalizedPosition, normalizedSize, Rotation);
        }
    }
}
