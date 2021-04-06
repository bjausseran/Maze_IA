using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotBank : MazeBot
{
    [SerializeField] MazeMap map;
    private int it = 0;
    List<MazeTile> path; 

    // Start is called before the first frame update
    void Start()
    {
        SetCount(50);
        Initialize();
        path = pathfinding.FindPath(start[0], start[1], end[0], end[1]);
    }

    protected override void PickNextTile()
    {
        if (path == null) return;
        currentTile = path[it];
        if (it != path.Count - 1) it++;
    }
    public override void Initialize()
    {
        base.Initialize();
        it = 0;
    }

}

