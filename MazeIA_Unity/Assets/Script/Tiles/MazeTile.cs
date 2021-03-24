using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeTile : MonoBehaviour
{
    public enum TileTypes
    {
        Empty,
        Path,
        Wall,
        Mud,
        Trap,
    }

    [Header("Design")]
    protected Sprite sprite;
    protected Color color;

    [Header("Infos")]
    protected TileTypes type = TileTypes.Empty;
    protected int x;
    protected int y;
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
    public int GetXPos()
    {
        return x;
    }
    public void SetXPos(int x)
    {
        this.x = x;
    }
    public int GetYPos()
    {
        return y;
    }
    public void SetYPos(int y)
    {
        this.y = y;
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


    [System.Serializable]
    public class SaveObject
    {
        public TileTypes type;
        public int x;
        public int y;
    }

    public SaveObject Save()
    {
        return new SaveObject
        {
            type = type,
            x = x,
            y = y
        };
    }

}
