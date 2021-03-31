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
        map = new MazeMap(24, 15, 0.5f, tileList[0], MazeMode.Resolver);
        map.LoadMostRecent();
        pathfinding = new Pathfinding(map.GetGrid());
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //Debug.Log("MazeResolver, Update : mouse world pos = " + mouseWorldPosition);
            //map.GetGrid().GetXY(mouseWorldPosition, out int x, out int y);
            //Debug.Log("MazeResolver, Update : mouse in & out = " + x + ", " + y);
            var start = map.GetGrid().FindStart();
            var end = map.GetGrid().FindEnd();

            List<MazeTile> path = pathfinding.FindPath(start[0], start[1], end[0], end[1]);
            if (path != null)
            {
                for (int i = 1; i < path.Count - 1;  i++)
                {
                    Debug.Log("MazeResolver, Update : path i = " + path[i].GetXPos() + ", " + path[i].GetYPos());
                    map.GetGrid().SetValue(path[i].GetXPos(), path[i].GetYPos(), tileList[7]);
                }
            }
        }

        if (Input.GetMouseButton(1))
        {
            map.SetTileValue(Camera.main.ScreenToWorldPoint(Input.mousePosition), tileList[0]);
        }
    }
}