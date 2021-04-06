using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeUser
{
    static MazeUser instance;

    private int id;
    private string name;
    private string apiKey;

    static public MazeUser GetInstance()
    {
        if (instance == null)
        {
            instance = new MazeUser();
            return instance;
        }
        return instance;
    }
    override public string ToString()
    {
        return "User : id = " + id + ", name = " + name + ", key = " + apiKey;
    }

    public void SetUser(int id, string name, string apiKey)
    {
        this.id = id;
        this.name = name;
        this.apiKey = apiKey;
    }

    public void SetId(int id)
    {
        this.id = id;
    }
    public int GetId()
    {
        return id;
    }
    public void SetName(string name)
    {
        this.name = name;
    }
    public string GetName()
    {
        return name;
    }
    public void SetApiKey(string apiKey)
    {
        this.apiKey = apiKey;
    }
    public string GetApiKey()
    {
        return apiKey;
    }

    public class SaveObject
    {
        public int id;
        public string name;
        public string apiKey;

        public void SetUserInfos()
        {
            Debug.Log("MazeUser.SaveObject : id = " + id + ", name = " + name + ", key = " + apiKey);
             MazeUser.GetInstance().SetUser(id, name, apiKey);
        }
    }

    }
