using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotRunner : MazeBot
{
    private MazeTile previousTile;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        previousTile = currentTile;
    }

    protected override void PickNextTile()
    {
        var listTiles = IsWalkable(pathfinding.GetListAdjacent(currentTile), previousTile);
        var listLength = listTiles.Count;
        if (listLength == 0) listTiles = IsWalkable(pathfinding.GetListAdjacent(currentTile), null);
        Debug.Log("BotRunner, Update : listTiles.Count = " + listLength);
        var pick = Random.Range(0, listLength);
        previousTile = currentTile;
        currentTile = listTiles[pick];
    }


}

