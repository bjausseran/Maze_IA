using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSolution : MazeTile
{
    // Start is called before the first frame update
    void Awake()
    {
        type = TileTypes.Solution;
        sprite = Resources.Load<Sprite>("Sprites/tile01");
        color = new Color(0f, 1f, 0f);
    }
}
