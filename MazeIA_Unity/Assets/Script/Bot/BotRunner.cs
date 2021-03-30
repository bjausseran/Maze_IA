using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotRunner : MonoBehaviour
{
    int count = 20;

    private Pathfinding pathfinding;
    private MazeGrid grid;
    private Sprite sprite;
    private SpriteRenderer spriteRenderer;
    private MazeTile currentTile;
    private MazeTile previousTile;

    private void Awake()
    {
        sprite = Resources.Load<Sprite>("Sprites/tile01");

        GameObject gameObject = new GameObject("visual", typeof(SpriteRenderer));
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        gameObject.transform.SetParent(transform);
        gameObject.transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one * 7f;

        gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
    }
    // Start is called before the first frame update
    void Start()
    {
        var start = grid.FindStart();
        var startTile = grid.GetValue(start[0], start[1]);
        transform.position = startTile.transform.position;
        currentTile = startTile;
        previousTile = startTile;
        pathfinding = new Pathfinding(grid);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        count--;
        if (count == 0)
        {
            var listTiles = IsWalkable(pathfinding.GetListAdjacent(currentTile), previousTile);
            var listLength = listTiles.Count;
            if(listLength==0) listTiles = IsWalkable(pathfinding.GetListAdjacent(currentTile), null);
            Debug.Log("BotRunner, Update : listTiles.Count = " + listLength);
                var pick = Random.Range(0, listLength);
                transform.position = listTiles[pick].transform.position;
            previousTile = currentTile;
                currentTile = listTiles[pick];
            if (currentTile.GetTileType() == MazeTile.TileTypes.Trap 
                || currentTile.GetTileType() == MazeTile.TileTypes.End) count = -1;
            else if (currentTile.GetTileType() == MazeTile.TileTypes.Mud) count = 40;
            else count = 20;
            
        }
    }

    public void SetGrid(MazeGrid grid)
    {
        this.grid = grid;
    }
    public void SetColor(Color color)
    {
        spriteRenderer.color = color;
    }

    private List<MazeTile> IsWalkable(List<MazeTile> list, MazeTile previousTile)
    {
        var outList = new List<MazeTile>();
        foreach (MazeTile tile in list)
        {
            if (tile.GetWalkable() && tile != previousTile) outList.Add(tile);
        }
        return outList;
    }
}

