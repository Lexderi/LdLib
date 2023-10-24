﻿using System.Drawing;
using LdLib.Vector;

namespace LdLib.Shapes;

public class Line : Shape
{
    public Vector2 End;
    public Vector2 Start;

    public float Weight;

    public Line(Vector2 start, Vector2 end, float weight, Color color)
    {
        Start = start;
        End = end;
        Weight = weight;
        Color = color;
    }

    protected internal override (Vector2[] points, Vector2 position, Vector2 scale, float rotation)
        GetNormalizedPoints()
    {
        Vector2[] points =
        {
            new(0, -0.5f),
            new(0, 0.5f),
            new(1, -0.5f),
            new(1, 0.5f)
        };

        Vector2 normalizedStart = NormalizePosition(Start);
        Vector2 delta = End - Start;
        float rotation = -delta.Rotation;
        Vector2 normalizedSize = NormalizeScale(new(delta.Magnitude, Weight));

        return (points, normalizedStart, normalizedSize, rotation);
    }
}