using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



public class MazeGrid
{
    [Header("Grid Infos")]
    private Sprite tile;
    private GameObject map;
    private MazeMode mode;
    private int width;
    private int height;
    private int tileNb;
    private float cellSize;
    private Vector3 originPosition;
    private MazeTile defaultTile;
    private MazeTile[,] gridArray;
    private SpriteRenderer[,] spriteArray;

    public MazeGrid(int width, int height, float cellSize, MazeTile defaultTile, MazeMode mode)
    {
        //Load a Sprite (Assets/Resources/Sprites/sprite01.png)
        tile = Resources.Load<Sprite>("Sprites/tile01");

        map = GameObject.Find("Map");
        this.width = width;
        this.height = height;
        this.mode = mode;
        this.cellSize = cellSize;
        this.defaultTile = defaultTile;
        this.originPosition = new Vector3(- width * cellSize * 0.5f, - height * cellSize * 0.5f);

        gridArray = new MazeTile[width, height];
        spriteArray = new SpriteRenderer[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {

                if (mode == MazeMode.Editor)
                {
                    //A bigger tilemap size to show a white grid, just visual
                    CreateTile(map.transform, "base", GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * .5f, Color.white, 1f);

                    //Create the sprite array, use to display tile design
                    spriteArray[x, y] = CreateTile(map.transform, "maze", GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * .5f, Camera.main.backgroundColor, 0.95f);
                    SetValue(x, y, defaultTile);
                }
                else 
                {
                    spriteArray[x, y] = CreateTile(map.transform, "maze", GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * .5f, Camera.main.backgroundColor, 1f);
                    SetValue(x, y, defaultTile);
                }
                //Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                //Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
            }
        }
        //Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
        //Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
    }

    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize + originPosition;
    }

    public void SetValue(int x, int y, MazeTile tile)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            if (mode == MazeMode.Editor)
            {
                if (tile.GetTileType() == MazeTile.TileTypes.Start && !(FindStart()[0] == x && FindStart()[1] == y))
                {
                    var startPos = FindStart();
                    Debug.Log("MazeGrid, SetValue(x:y) : start = " + startPos[0] + ", " + startPos[1]);
                    SetValue(startPos[0], startPos[1], defaultTile);
                }
                if (tile.GetTileType() == MazeTile.TileTypes.End && !(FindEnd()[0] == x && FindEnd()[1] == y))
                {
                    var endPos = FindEnd();
                    SetValue(endPos[0], endPos[1], defaultTile);
                }
            }
            if (gridArray[x, y]) GameObject.Destroy(gridArray[x, y].gameObject.GetComponent<MazeTile>());
            gridArray[x, y] = (MazeTile) spriteArray[x, y].gameObject.AddComponent(tile.GetType());
            gridArray[x, y].SetXPos(x);
            gridArray[x, y].SetYPos(y);
            spriteArray[x, y].color = tile.GetColor();
            Debug.Log("MazeGrid, SetValue(x:y) : Color = " + tile.GetColor());

        }
    }

    public void SetValue(Vector3 worldPosition, MazeTile tile)
    {
        int x, y;
        Debug.Log("MazeGrid, SetValue(world) : Tile type = " + tile);
        GetXY(worldPosition, out x, out y);
        SetValue(x, y, tile);
    }

    public MazeTile GetValue(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        }
        else
        {
            return defaultTile;
        }
    }

    public MazeTile GetValue(Vector3 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetValue(x, y);
    }
    public void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
    }
    public int[] FindStart()
    {
        int[] startPos = { int.MinValue, int.MinValue };

        for (int x = 0; x < width; x++)
        {

            for (int y = 0; y < height; y++)
            {
                if (gridArray[x, y].GetTileType() == MazeTile.TileTypes.Start)
                {
                    startPos[0] = x;
                    startPos[1] = y;
                    return startPos;
                }
            }
        }
        return startPos;
    }
    public int[] FindEnd()
    {
        int[] endPos = { -1, -1 };

        for (int x = 0; x < width; x++)
        {

            for (int y = 0; y < height; y++)
            {
                if (gridArray[x, y].GetTileType() == MazeTile.TileTypes.End)
                {
                    endPos[0] = x;
                    endPos[1] = y;
                    return endPos;
                }
            }
        }
        return endPos;
    }
    public int GetWidth()
    {
        return width;
    }
    public int GetHeight()
    {
        return height;
    }
    public void SetWidth(int width)
    {
        this.width = width;
    }
    public void SetHeight(int height)
    {
        this.height = height;
    }

    public SpriteRenderer CreateTile(Transform parent, string name, Vector3 localPosition, Color color, float size)
    {
        Debug.Log("MazeGrid, CreateTile : Color = " + color);
        GameObject gameObject = new GameObject(name + "_tile_" + tileNb, typeof(SpriteRenderer));
        tileNb++;
        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localPosition;
        transform.localScale = new Vector3(cellSize * 20f * size, cellSize * 20f * size);
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = tile;
        spriteRenderer.color = color;
        return spriteRenderer;
    }
    public GameObject GetMapObject()
    {
        return map;
    }

}