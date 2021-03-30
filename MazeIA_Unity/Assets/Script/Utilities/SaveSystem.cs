using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveSystem : MonoBehaviour
{

    private const string SAVE_EXTENSION = "json";
    private static readonly string SAVE_FOLDER = Application.dataPath + "/Saves/Maps/";
    private static bool isInit = false;
    public static void Init()
    {
        if (!isInit)
        {
            isInit = true;
            // Is save folder exists ?
            if (!Directory.Exists(SAVE_FOLDER))
            {
                // If not, create it
                Directory.CreateDirectory(SAVE_FOLDER);
            }
        }
    }

    public static void Save(string fileName, string saveString, bool overwrite)
    {
        Init();
        string saveFileName = fileName;
           if (!overwrite)
        {
            // if !overwrite, pick unique number
            int saveNumber = 1;
            while (File.Exists(SAVE_FOLDER + saveFileName + "." + SAVE_EXTENSION))
            {
                saveNumber++;
                saveFileName = fileName + "." + saveNumber;
            }
        }
        File.WriteAllText(SAVE_FOLDER + saveFileName + "." + SAVE_EXTENSION, saveString);
    }

    public static string Load(string fileName)
    {
        Init();
        if (File.Exists(SAVE_FOLDER + fileName + "." + SAVE_EXTENSION))
        {
            string saveString = File.ReadAllText(SAVE_FOLDER + fileName + "." + SAVE_EXTENSION);
            return saveString;
        } 
        else
        {
            return null;
        }
    }
    public static List<string> GetFileList()
    {
        Init();
        DirectoryInfo directoryInfo = new DirectoryInfo(SAVE_FOLDER);
        //Get all saved files
        FileInfo[] saveFiles = directoryInfo.GetFiles("*." + SAVE_EXTENSION);
        //Check most recent
        FileInfo mostRecentFile = null;
        List<string> FileList = new List<string>();
        foreach (FileInfo fileinfo in saveFiles)
        {
            FileList.Add(fileinfo.Name);
        }
        return FileList;
        }
    public static string LoadMostRecentFile()
    {
        Init();
        DirectoryInfo directoryInfo = new DirectoryInfo(SAVE_FOLDER);
        //Get all saved files
        FileInfo[] saveFiles = directoryInfo.GetFiles("*." + SAVE_EXTENSION);
        //Check most recent
        FileInfo mostRecentFile = null;
        foreach(FileInfo fileinfo in saveFiles)
        {
            if (mostRecentFile == null)
            {
                mostRecentFile = fileinfo;
            } 
            else
            {
                if (fileinfo.LastWriteTime > mostRecentFile.LastWriteTime)
                {
                    mostRecentFile = fileinfo;
                }
            }
        }
        //If there is a file return it, or return null
        if (mostRecentFile != null)
        {
            string saveString = File.ReadAllText(mostRecentFile.FullName);
            return saveString;
        } else
        {
            return null;
        }
    }

    public static void SaveObject(object saveObject)
    {
        SaveObject("save", saveObject, false);
    }

    public static void SaveObject(string fileName, object saveObject, bool overwrite)
    {
        Init();
        string json = JsonUtility.ToJson(saveObject);
        Save(fileName, json, overwrite);
    }
    public static void SaveString(string fileName, string output, bool overwrite)
    {
        Init();
        Save(fileName, output, overwrite);
    }

    public static TSaveObject LoadMostRecentObject<TSaveObject>()
    {
        Init();
        string saveString = LoadMostRecentFile();
        if (saveString != null)
        {
            TSaveObject saveObject = JsonUtility.FromJson<TSaveObject>(saveString);
            return saveObject;
        }
        else
        {
            return default(TSaveObject);
        }
    }
    public static TSaveObject LoadObject<TSaveObject>(string fileName)
    {
        Init();
        string saveString = Load(fileName);
        if (saveString != null)
        {
            TSaveObject saveObject = JsonUtility.FromJson<TSaveObject>(saveString);
            return saveObject;
        }
        else
        {
            return default(TSaveObject);
        }
    }
}
