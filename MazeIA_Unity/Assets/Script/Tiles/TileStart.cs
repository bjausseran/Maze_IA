using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileStart : MazeTile
{
    // Start is called before the first frame update
    void Start()
    {
        type = TileTypes.Start;
        sprite = Resources.Load<Sprite>("Sprites/tile01");
        color = new Color(1f, 1f, 1f);

    }
}
