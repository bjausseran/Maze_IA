using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class MazeBot : MonoBehaviour
{
    private int COUNT = 20;
    protected int count = 1;
    protected bool canMove = false;
    protected Pathfinding pathfinding;
    protected MazeGrid grid;
    protected Sprite sprite;
    protected SpriteRenderer spriteRenderer;
    protected MazeTile currentTile;
    protected int[] start;
    protected int[] end;

    private void Awake()
    {
        sprite = Resources.Load<Sprite>("Sprites/tile01");

        GameObject gameObject = new GameObject("visual", typeof(SpriteRenderer));
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = 2;
        gameObject.transform.SetParent(transform);
        gameObject.transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one * 7f;

        gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (!canMove) return;
        count--;
        if (count == 0)
        {
            PickNextTile();
            SetSpeed();
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, currentTile.transform.position, Mathf.Clamp(1f / (float)count, 0.02f, 0.08f));
        }
    }
    public void SetCanMove(bool value)
    {
        canMove = value;
    }
    // Start is called before the first frame update
    virtual public void Initialize()
    {
        start = grid.FindStart();
        end = grid.FindEnd();
        count = 1;
        var startTile = grid.GetValue(start[0], start[1]);
        transform.position = startTile.transform.position;
        currentTile = startTile;
        pathfinding = new Pathfinding(grid);
    }
    protected void SetCount(int count)
    {
        COUNT = count;
    }
    protected int GetCount()
    {
        return COUNT;
    }

    public void SetGrid(MazeGrid grid)
    {
        this.grid = grid;
    }
    public void SetColor(Color color)
    {
        spriteRenderer.color = color;
    }

    protected List<MazeTile> IsWalkable(List<MazeTile> list, MazeTile previousTile)
    {
        var outList = new List<MazeTile>();
        foreach (MazeTile tile in list)
        {
            if (tile.GetWalkable() && tile != previousTile) outList.Add(tile);
        }
        return outList;
    }
    protected void SetSpeed()
    {
        if (currentTile.GetTileType() == MazeTile.TileTypes.Trap
            || currentTile.GetTileType() == MazeTile.TileTypes.End) count = -1;
        else if (currentTile.GetTileType() == MazeTile.TileTypes.Mud) count = GetCount() * 2;
        else count = GetCount();
    }

    protected abstract void PickNextTile();
}
