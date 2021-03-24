using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeEditor : MonoBehaviour
{

    [Header("Component")]
    [SerializeField] MazeGrid grid;
    [Header("Brush Infos")]
    [SerializeField] private MazeTile currentTile;

    private void Start()
    {
        grid = new MazeGrid(24, 15, 0.5f);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            grid.SetValue(Camera.main.ScreenToWorldPoint(Input.mousePosition), currentTile);
        }

        if (Input.GetMouseButtonDown(1))
        {
            grid.SetValue(Camera.main.ScreenToWorldPoint(Input.mousePosition), new TileEmpty());
        }
    }
    public void SetCurrentTile(MazeTile tile)
    {
        currentTile = tile;
    }

    public MazeTile GetCurrentTile()
    {
        return currentTile;
    }
}
