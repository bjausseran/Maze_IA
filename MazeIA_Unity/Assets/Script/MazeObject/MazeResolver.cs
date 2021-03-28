using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeResolver : MonoBehaviour
{

    [Header("Component")]
    [SerializeField] MazeMap map;
    [SerializeField] TypeToTileConverter converter;
    [SerializeField] Pathfinding pathfinding;
    [SerializeField] List<MazeTile> tileList = new List<MazeTile>();

    private void Start()
    {

        converter = TypeToTileConverter.GetInstance();
        converter.SetArray(tileList.ToArray());
        map = new MazeMap(24, 15, 0.5f, tileList[0]);
        map.Load();
        pathfinding = new Pathfinding(map.GetGrid());
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log("MazeResolver, Update : mouse world pos = " + mouseWorldPosition);
            map.GetGrid().GetXY(mouseWorldPosition, out int x, out int y);
            Debug.Log("MazeResolver, Update : mouse in & out = " + x + ", " + y);
            List<MazeTile> path = pathfinding.FindPath(map.GetGrid().FindStart()[0], map.GetGrid().FindStart()[1], map.GetGrid().FindEnd()[0], map.GetGrid().FindEnd()[1]);
            if (path != null)
            {
                for (int i = 0; i < path.Count - 1;  i++)
                {
                    Debug.Log("MazeResolver, Update : path i = " + path[i].GetXPos() + ", " + path[i].GetYPos());
                    Debug.DrawLine(new Vector3(path[i].GetXPos(), path[i].GetYPos()) * 10f + Vector3.one * 5f, new Vector3(path[i + 1].GetXPos(), path[i + 1].GetYPos()) * 10f + Vector3.one * 5f);
                    path[i].SetColor(Color.green);
                    map.GetGrid().SetValue(path[i].GetXPos(), path[i].GetYPos(), tileList[4]);
                }
            }
        }

        if (Input.GetMouseButton(1))
        {
            map.SetTileValue(Camera.main.ScreenToWorldPoint(Input.mousePosition), tileList[0]);
        }
    }
}