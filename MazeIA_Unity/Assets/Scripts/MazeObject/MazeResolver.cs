using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MazeResolver : MazeMode
{

    [Header("Component")]
    [SerializeField] ResolverUI ui;
    [SerializeField] TypeToTileConverter converter;
    [SerializeField] Pathfinding pathfinding;
    [SerializeField] List<MazeTile> tileList = new List<MazeTile>();

    private void Start()
    {

        converter = TypeToTileConverter.GetInstance();
        ui.DisplayFileWindow();
        converter.SetArray(tileList.ToArray());
        /*map = new MazeMap(24, 15, 0.5f, tileList[0], MazeModes.Resolver);
        map.LoadMostRecent();
        pathfinding = new Pathfinding(map.GetGrid());*/
    }
    public Pathfinding GetPathfinding()
    {
        return pathfinding;
    }

    public override void SetMap(MazeMap map)
    {
        base.SetMap(map);
        pathfinding = new Pathfinding(map.GetGrid());
    }

    public Resolvability FindPath()
    {
        if (map == null) return Resolvability.Unknow;
        var start = map.GetGrid().FindStart();
        var end = map.GetGrid().FindEnd();
        var http = gameObject.AddComponent<HttpRequestHelper>();

        List<MazeTile> path = pathfinding.FindPath(start[0], start[1], end[0], end[1]);
        if (path != null)
        {
            for (int i = 1; i < path.Count - 1; i++)
            {
                Debug.Log("MazeResolver, Update : path i = " + path[i].GetXPos() + ", " + path[i].GetYPos());
                map.GetGrid().SetValue(path[i].GetXPos(), path[i].GetYPos(), tileList[7]);
            }
            StartCoroutine(http.SendTest(FindObjectOfType<MazeManager>().GetSelectedOrNew(), 1));
            return Resolvability.True;
        }
        else
        {
            StartCoroutine(http.SendTest(FindObjectOfType<MazeManager>().GetSelectedOrNew(), 0));
            return Resolvability.False;
        }
        

    }

}