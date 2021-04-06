using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MazeModes
{
    Editor,
    Resolver,
    Bet,
}
public class MazeMode : MonoBehaviour
{
    [SerializeField] protected MazeMap map;
    virtual public void SetMap(MazeMap map)
    {
        this.map = map;
    }
}
