using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Resolvability
{
    True,
    False,
    Unknow
}
public class Pathfinding 
{
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    private bool diagonalMovement = false;

    private MazeGrid grid;
    private List<MazeTile> openList;
    private List<MazeTile> closedList;

    private int totalCost = 0;
    private int nbStep = 0;
    private Resolvability resolvable = Resolvability.Unknow;

    public Pathfinding(MazeGrid grid)
    {
        this.grid = grid;
    }

    public List<MazeTile> FindPath(int startX, int startY, int endX, int endY)
    {
        totalCost = 0;
        nbStep = 0;
        resolvable = Resolvability.Unknow;
        MazeTile startStep = grid.GetValue(startX, startY);
        MazeTile endStep = grid.GetValue(endX, endY);

        openList = new List<MazeTile> { startStep };
        closedList = new List<MazeTile>();
        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                MazeTile MazeTile = grid.GetValue(x, y);
                MazeTile.SetGCost(int.MaxValue);
                MazeTile.CalculateFCost();
                MazeTile.previousStep = null;
            }
        }
            startStep.SetGCost(0);
            startStep.hCost = CalculateDistanceCost(startStep, endStep);
            startStep.CalculateFCost();


                while (openList.Count > 0)
            {
                MazeTile currentStep = GetLowestFCostStep(openList);
                if (currentStep == endStep)
                {
                //the end
                resolvable = Resolvability.True;
                return CalculatePath(endStep);
                }
                openList.Remove(currentStep);
                closedList.Add(currentStep);

                var it = 0;
                foreach(MazeTile adjacentStep in GetListAdjacent(currentStep))
                {
                    it++;
                    if (closedList.Contains(adjacentStep)) continue;
                    if (!adjacentStep.GetWalkable())
                    {
                        closedList.Add(adjacentStep);
                        continue;
                    }
                    if (adjacentStep.GetGameEnder())
                    {
                        closedList.Add(adjacentStep);
                        continue;
                    }
                    

                    //GetSpeedModifier is for the mud
                    int tentativeGCost = currentStep.GetGCost()+ CalculateDistanceCost(currentStep, adjacentStep)*adjacentStep.GetSpeedModifier();
                    if (tentativeGCost < adjacentStep.GetGCost())
                    {
                        adjacentStep.previousStep = currentStep;
                        adjacentStep.SetGCost(tentativeGCost);
                        adjacentStep.hCost = CalculateDistanceCost(adjacentStep, endStep);
                        adjacentStep.CalculateFCost();
                        if (!openList.Contains(adjacentStep))
                        {
                            openList.Add(adjacentStep);
                        }
                    }
                }
            }

        // There is no way
        resolvable = Resolvability.False;
        return null;
    }

    public Resolvability GetResolvability()
    {
        return resolvable;
    }
    public int GetTotalCost()
    {
        return totalCost;
    }
    public int GetNumberOfSteps()
    {
        return nbStep;
    }

    private MazeTile GetStep(int x, int y)
    {
        return grid.GetValue(x, y);
    }

    private List<MazeTile> CalculatePath(MazeTile endStep)
    {
        List<MazeTile> path = new List<MazeTile>();
        path.Add(endStep);
        MazeTile currentStep = endStep;
        while(currentStep.previousStep != null)
        {
            nbStep++;
            totalCost += currentStep.previousStep.GetSpeedModifier();
            path.Add(currentStep.previousStep);
            currentStep = currentStep.previousStep;
        }
        path.Reverse();
        return path;
    }

    public List<MazeTile> GetListAdjacent(MazeTile currentStep)
    {
        List<MazeTile> adjacentList = new List<MazeTile>();

        if (currentStep.GetXPos() - 1 >= 0)
        {
            //Left
            adjacentList.Add(GetStep(currentStep.GetXPos() - 1, currentStep.GetYPos()));

            //If diagonal movement
            if (diagonalMovement)
            {
                //Left down
                if (currentStep.GetYPos() - 1 >= 0) adjacentList.Add(GetStep(currentStep.GetXPos() - 1, currentStep.GetYPos() - 1));
                //Left up
                if (currentStep.GetYPos() + 1 < grid.GetHeight()) adjacentList.Add(GetStep(currentStep.GetXPos() - 1, currentStep.GetYPos() + 1));
            }
        }
        if (currentStep.GetXPos() + 1 < grid.GetWidth())
        {
            //Right
            adjacentList.Add(GetStep(currentStep.GetXPos() + 1, currentStep.GetYPos()));
            //If diagonal movement
            if (diagonalMovement)
            {
                //Right down
                if (currentStep.GetYPos() - 1 >= 0) adjacentList.Add(GetStep(currentStep.GetXPos() + 1, currentStep.GetYPos() - 1));
                //Right up
                if (currentStep.GetYPos() + 1 < grid.GetHeight()) adjacentList.Add(GetStep(currentStep.GetXPos() + 1, currentStep.GetYPos() + 1));
            }
        }
        //Up
        if (currentStep.GetYPos() + 1 < grid.GetHeight()) adjacentList.Add(GetStep(currentStep.GetXPos(), currentStep.GetYPos() + 1));
        //Down
        if (currentStep.GetYPos() - 1 >= 0) adjacentList.Add(GetStep(currentStep.GetXPos(), currentStep.GetYPos() - 1));

        return adjacentList;
    }
    private int CalculateDistanceCost(MazeTile a, MazeTile b)
    {
        int xDistance = Mathf.Abs(a.GetXPos() - b.GetXPos());
        int yDistance = Mathf.Abs(a.GetYPos() - b.GetYPos());
        int remaining = Mathf.Abs(xDistance - yDistance);
        //with diagonal movement
        if (diagonalMovement) return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
        //without diagonal movement
        else return MOVE_STRAIGHT_COST * (xDistance + yDistance);
    }

    private MazeTile GetLowestFCostStep(List<MazeTile> pathStepList)
    {
        MazeTile lowestFCostStep = pathStepList[0];
        for (int i = 1; i < pathStepList.Count; i++)
        {
            if (pathStepList[i].fCost < lowestFCostStep.fCost)
            {
                lowestFCostStep = pathStepList[i];
            }
        }
        return lowestFCostStep;
    }
}
