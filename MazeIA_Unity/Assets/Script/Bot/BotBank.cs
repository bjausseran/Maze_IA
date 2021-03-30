using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotBank : MonoBehaviour
{
    int count = 50;

    private Pathfinding pathfinding;
    private MazeGrid grid;
    [SerializeField] MazeMap map;
    private Sprite sprite;
    private int it = 0;
    private SpriteRenderer spriteRenderer;
    private MazeTile currentTile;
    private MazeTile previousTile;
    List<MazeTile> path; 

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
        var end = grid.FindEnd();
        path = pathfinding.FindPath(start[0], start[1], end[0], end[1]);


    }

    // Update is called once per frame
    void FixedUpdate()
    {
        count--;
        if (count == 0)
        {
            currentTile = path[it];
            transform.position = currentTile.transform.position;
            if (currentTile.GetTileType() == MazeTile.TileTypes.Trap) count = -1;
            else if (currentTile.GetTileType() == MazeTile.TileTypes.Mud) count = 100;
            else count = 50;
            if (it!=path.Count-1) it++;

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

