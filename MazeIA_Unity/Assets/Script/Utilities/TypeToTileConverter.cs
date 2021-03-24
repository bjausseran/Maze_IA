using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeToTileConverter 
{

    #region Singleton
    public static TypeToTileConverter instance;

    public static TypeToTileConverter GetInstance()
    {
        if (instance != null) return instance;
        else return instance = new TypeToTileConverter();
    }

    #endregion

    public MazeTile[] tilesArray;

    public MazeTile EnumToTile(MazeTile.TileTypes type)
    {
        return tilesArray[(int)type];
    }

    public void SetArray(MazeTile[] tiles)
    {
        tilesArray = tiles;
    }
}
