using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeTile : MonoBehaviour
{
    [Header("Design")]
    protected Sprite sprite;
    protected Color color;

    [Header("Infos")]
    protected bool walkable = true;
    protected float speedModifier = 1;
    protected bool gameEnder = true;

    public Color GetColor()
    {
        return color;
    }
    public void SetColor(Color color)
    {
        this.color = color;
    }
    public Sprite GetSprite()
    {
        return sprite;
    }
    public void SetSprite(Sprite sprite)
    {
        this.sprite = sprite;
    }

    public bool GetWalkable()
    {
        return walkable;
    }
    public void SetWalkable(bool walkable)
    {
        this.walkable = walkable;
    }

    public float GetSpeedModifier()
    {
        return speedModifier;
    }
    public void SetSpeedModifier(float speedMd)
    {
        this.speedModifier = speedMd;
    }
    public bool GetGameEnder()
    {
        return gameEnder;
    }
    public void SetGameEnder(bool value)
    {
        this.gameEnder = value;
    }
}
