using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathStep
{

    private Grid grid;
    public int x;
    public int y;

    public int gCost;
    public int hCost;
    public int fCost;

    public PathStep previousStep;
    public PathStep(Grid grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
    }

    public override string ToString()
    {
        return x + ", " + y;
    }
    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }
}
