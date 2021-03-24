using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileTrap : MazeTile
{
    // Start is called before the first frame update
    void Start()
    {
        sprite = Resources.Load<Sprite>("Sprites/tile01");
        color = new Color(0.6f, 0, 0);
    }
}
