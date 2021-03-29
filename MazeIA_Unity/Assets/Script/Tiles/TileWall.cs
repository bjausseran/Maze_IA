using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileWall : MazeTile
{
    // Start is called before the first frame update
    void Awake()
    {
        type = TileTypes.Wall;
        sprite = Resources.Load<Sprite>("Sprites/tile01");
        color = new Color(0.3f, 0.3f, 0.3f);
        SetWalkable(false);

    }
}
