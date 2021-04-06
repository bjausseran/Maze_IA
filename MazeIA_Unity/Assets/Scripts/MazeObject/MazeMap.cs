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
    private MazeTile baseTile;
    private TypeToTileConverter converter;
    private MazeModes mode;

    public MazeMap(int width, int height, float cellSize, MazeTile tile, MazeModes mode)
    {
        this.converter = TypeToTileConverter.GetInstance();
        this.width = width;
        this.height = height;
        this.mode = mode;
        this.baseTile = tile;
        Debug.Log("MazeMap, MazeMap : width = " + width + ", height = " + height);
        this.grid = new MazeGrid(width, height, 0.5f, tile, mode);

        
    }
    public MazeModes GetMode()
    {
        return mode;
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

    public void Save(string name)
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


        SaveSystem.SaveString("/" + name, output, false);
    }

    public string GetDataToUpload()
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
        Debug.Log("MazeMap, Save : Saved data = " + output);

        return output;
    }

    public void LoadFromFile(string fileName)
    {
        Debug.Log("MazeMap, Load : fileName = " + fileName);
        MazeTile.SaveObject[] saveObjectArray = JsonHelper.FromJson<MazeTile.SaveObject>(SaveSystem.LoadMap(fileName));
        Debug.Log("MazeMap, Load : saveobject lenght + " + saveObjectArray.Length);

        foreach (MazeTile.SaveObject mazeTileSaved in saveObjectArray)
        {
            grid.SetValue(mazeTileSaved.x, mazeTileSaved.y, converter.EnumToTile(mazeTileSaved.type));
        }
    }

    private int[] FindWidthAndHeight(MazeTile.SaveObject[] tileSaved)
    {
        var width = 0;
        var height = 0;
        for (int i = 0; i < tileSaved.Length; i++)
        {
            if (tileSaved[i].x + 1 > width) width = tileSaved[i].x + 1;
            if (tileSaved[i].y + 1 > height) height = tileSaved[i].y + 1;
        }
        Debug.Log("MazeMap, FindWidthAndHeight : width, height = " + width + ", " + height);
        return new int[2]{ width, height };
    }

    public bool LoadFromAPI(string json)
    {
        Debug.Log("MazeMap, LoadFromAPI : json = " + json);
        if (JsonHelper.FromJson<MazeTile.SaveObject>(json) == null) return false;
        MazeTile.SaveObject[] saveObjectArray = JsonHelper.FromJson<MazeTile.SaveObject>(json);
        Debug.Log("MazeMap, LoadFromAPI : saveobject lenght + " + saveObjectArray.Length);

        grid.DestroyGrid();
        var size = FindWidthAndHeight(saveObjectArray);
        SetWidth(size[0]);
        SetHeight(size[1]);
        if (baseTile == null) baseTile = converter.EnumToTile(MazeTile.TileTypes.Path);
        grid = new MazeGrid(size[0], size[1], 0.5f, baseTile, mode);
        foreach (MazeTile.SaveObject mazeTileSaved in saveObjectArray)
        {
            grid.LoadValue(mazeTileSaved.x, mazeTileSaved.y, converter.EnumToTile(mazeTileSaved.type));
        }
        return true;
    }
    public IEnumerator GenerateGrid(int width, int height)
    {
        grid.DestroyGrid();
        grid = new MazeGrid(width, height, 0.5f, baseTile, mode);
        SetWidth(width);
        SetHeight(height);
        yield return true;
    }

    public void LoadMostRecent()
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
    [System.Serializable]
    public class SavedObject
    {
        public int id;
        public string name;

    }


}

