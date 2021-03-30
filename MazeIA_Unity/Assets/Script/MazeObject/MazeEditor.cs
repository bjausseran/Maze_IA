using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeEditor : MonoBehaviour
{

    [Header("Component")]
    [SerializeField] EditorUI ui;
    [SerializeField] MazeMap map;
    [SerializeField] TypeToTileConverter converter;
    [SerializeField] List<MazeTile> tileList = new List<MazeTile>();
    [Header("Brush Infos")]
    [SerializeField] private MazeTile currentTile;

   private void Start()
    {
        
        converter = TypeToTileConverter.GetInstance();
        converter.SetArray(tileList.ToArray());
        map = new MazeMap(24, 15, 0.5f, tileList[0], MazeMode.Editor);
        ui.SetMap(map);
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
            //map.Save();
            ui.DisplayFileWindow(EditorUI.FileMode.Save);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            //map.Load();
            ui.DisplayFileWindow(EditorUI.FileMode.Load);
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
