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

    public MazeMap(int width, int height, float cellSize, MazeTile tile, MazeMode mode)
    {
        this.converter = TypeToTileConverter.GetInstance();
        this.width = width;
        this.height = height;
        grid = new MazeGrid(width, height, 0.5f, tile, mode);

        GameObject bankObject = new GameObject("bot_", typeof(BotBank));
        bankObject.GetComponent<BotBank>().SetGrid(grid);
        bankObject.GetComponent<BotBank>().SetColor(Color.yellow);
        Color[] color = { Color.red, Color.blue, Color.green, Color.black, Color.cyan, Color.gray, Color.magenta };
        if (mode == MazeMode.Bet)
        {
            for (int i = 0; i < color.Length; i++)
            {
                GameObject gameObject = new GameObject("bot_", typeof(BotRunner));
                gameObject.GetComponent<BotRunner>().SetGrid(grid);
                gameObject.GetComponent<BotRunner>().SetColor(color[i]);
            }
        }
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
    public bool LoadFromAPI(string json)
    {
        Debug.Log("MazeMap, Load : json = " + json);
        if (JsonHelper.FromJson<MazeTile.SaveObject>(json) == null) return false;
        MazeTile.SaveObject[] saveObjectArray = JsonHelper.FromJson<MazeTile.SaveObject>(json);
        Debug.Log("MazeMap, Load : saveobject lenght + " + saveObjectArray.Length);
        foreach (MazeTile.SaveObject mazeTileSaved in saveObjectArray)
        {
            grid.SetValue(mazeTileSaved.x, mazeTileSaved.y, converter.EnumToTile(mazeTileSaved.type));
        }
        return true;
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

