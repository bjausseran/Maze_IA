using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileEnd : MazeTile
{
    // Start is called before the first frame update
    void Awake()
    {
        type = TileTypes.End;
        sprite = Resources.Load<Sprite>("Sprites/tile01");
        color = new Color(0.1f, 0.1f, 0.1f);

    }
}
