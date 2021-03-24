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
    private int width;
    private int height;
    private int tileNb;
    private float cellSize;
    private Vector3 originPosition;
    private MazeTile[,] gridArray;
    private SpriteRenderer[,] spriteArray;

    public MazeGrid(int width, int height, float cellSize)
    {
        //Load a Sprite (Assets/Resources/Sprites/sprite01.png)
        tile = Resources.Load<Sprite>("Sprites/tile01");

        map = GameObject.Find("Map");
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = new Vector3(- width * cellSize * 0.5f, - height * cellSize * 0.5f);

        gridArray = new MazeTile[width, height];
        spriteArray = new SpriteRenderer[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                //CreateTile(map.transform, "base", GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * .5f, Color.white, 1f);
                spriteArray[x, y] = CreateTile(map.transform, "maze", GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * .5f, Camera.main.backgroundColor, 0.98f);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
            }
        }
        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
    }

    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize + originPosition;
    }

    public void SetValue(int x, int y, MazeTile tile)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x, y] = tile;
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
            return new TileWall();
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

    public SpriteRenderer CreateTile(Transform parent, string name, Vector3 localPosition, Color color, float size)
    {
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