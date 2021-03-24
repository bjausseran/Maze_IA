using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMud : MazeTile
{
    // Start is called before the first frame update
    void Start()
    {
        sprite = Resources.Load<Sprite>("Sprites/tile01");
        color = new Color(0.3f, 0.2f, 0.1f);
    }

}
