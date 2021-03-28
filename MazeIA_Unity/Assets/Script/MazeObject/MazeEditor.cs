using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeEditor : MonoBehaviour
{

    [Header("Component")]
    [SerializeField] MazeMap map;
    [SerializeField] TypeToTileConverter converter;
    [SerializeField] List<MazeTile> tileList = new List<MazeTile>();
    [Header("Brush Infos")]
    [SerializeField] private MazeTile currentTile;

   private void Start()
    {

        converter = TypeToTileConverter.GetInstance();
        converter.SetArray(tileList.ToArray());
        map = new MazeMap(24, 15, 0.5f, tileList[0]);
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            map.SetTileValue(Camera.main.ScreenToWorldPoint(Input.mousePosition), currentTile);
        }

        if (Input.GetMouseButton(1))
        {
            map.SetTileValue(Camera.main.ScreenToWorldPoint(Input.mousePosition), tileList[0]);
        }

        if (Input.GetKeyDown(KeyCode.S)){
            map.Save();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            map.Load();
        }
    }
    public void SetCurrentTile(int tileNb)
    {
        currentTile = tileList[tileNb];
    }

    public MazeTile GetCurrentTile()
    {
        return currentTile;
    }
}
