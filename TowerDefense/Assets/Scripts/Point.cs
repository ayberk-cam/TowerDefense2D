using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Point
{
    private int X { get; set; }
    private int Y { get; set; }

    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }
    public int GetY()
    {
        return Y;
    }
    public int GetX()
    {
        return this.X;
    }
}
