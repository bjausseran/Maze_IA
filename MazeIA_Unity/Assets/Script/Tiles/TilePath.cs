using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePath : MazeTile
{
    // Start is called before the first frame update
    void Awake()
    {
        type = TileTypes.Path;
        sprite = Resources.Load<Sprite>("Sprites/tile01");
        color = new Color(0.5f, 0.4f, 0.3f);
    }

}
