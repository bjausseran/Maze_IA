using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MazeMap
{
    private MazeGrid grid;
    private int width;
    private int height;
    private TypeToTileConverter converter;

    public MazeMap(int width, int height, float cellSize, MazeTile tile)
    {
        this.converter = TypeToTileConverter.GetInstance();
        this.width = width;
        this.height = height;
        grid = new MazeGrid(width, height, 0.5f, tile);
    }
    public MazeGrid GetGrid()
    {
        return grid;
    }
    public int GetWidth()
    {
        return width;
    }
    public void SetWidth(int width)
    {
        this.width = width;
    }
    public int GetHeight()
    {
        return height;
    }
    public void SetHeight(int height)
    {
        this.height = height;
    }
    internal void SetTileValue(Vector3 worldPos, MazeTile tile)
    {
        grid.SetValue(worldPos, tile);
    }
    internal MazeTile GetTileValue(Vector3 worldPos)
    {
        return grid.GetValue(worldPos);
    }

    public void Save()
    {
        List<MazeTile.SaveObject> saveObjectList = new List<MazeTile.SaveObject>();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                saveObjectList.Add(grid.GetValue(x, y).Save());
            }

        }
        string output = JsonHelper.ToJson<MazeTile.SaveObject>(saveObjectList.ToArray());
        Debug.Log("MazeMap, Save : Saved json = " +  output);


        SaveSystem.SaveString("/map", output, false);
    }

    public void Load()
    {
        MazeTile.SaveObject[] saveObjectArray = JsonHelper.FromJson<MazeTile.SaveObject>(SaveSystem.LoadMostRecentFile());
        Debug.Log("MazeMap, Load : saveobject lenght + " + saveObjectArray.Length);
        foreach (MazeTile.SaveObject mazeTileSaved in saveObjectArray)
        {
            grid.SetValue(mazeTileSaved.x, mazeTileSaved.y, converter.EnumToTile(mazeTileSaved.type));
        }
    }


    public class SaveObject
    {
        public MazeTile.SaveObject[] saveObjectArray;

        public string GetFormatedString()
        {
            string output = "[" + JsonUtility.ToJson(saveObjectArray[0]);
            for (int i = 1; i < saveObjectArray.Length; i++)
            {
                output = output + "," + JsonUtility.ToJson(saveObjectArray[i]);
            }
            output = output + "]";
            return output;
        }

    }


}

