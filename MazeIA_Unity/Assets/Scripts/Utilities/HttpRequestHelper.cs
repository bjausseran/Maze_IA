using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class HttpRequestHelper : MonoBehaviour
{
    const string API_URL = "http://mazelaravel.test/api/";


    public IEnumerator GetMazeList()
    {
        UnityWebRequest www = UnityWebRequest.Get(API_URL + "mazelist");
        www.SetRequestHeader("USERKEY", MazeUser.GetInstance().GetApiKey());
        yield return www.SendWebRequest();
        string output = null;
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("HttpRequestHelper, GetMazeList: error : " + www.error);
            yield return null;
        }
        else
        {
            output = www.downloadHandler.text;
            yield return output;
            // Show results as text
            Debug.Log("HttpRequestHelper, GetMazeList : success : " + output);

            // Or retrieve results as binary data
            byte[] results = www.downloadHandler.data;
        }
    }
    public IEnumerator GetMazeJson(int id)
    {
        UnityWebRequest www = UnityWebRequest.Get(API_URL + "maze/json/" + id);
        www.SetRequestHeader("USERKEY", MazeUser.GetInstance().GetApiKey());
        yield return www.SendWebRequest();
        string output = null;
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("HttpRequestHelper, GetMazeJson: error : " + www.error);
            yield return false;
        }
        else
        {
            output = www.downloadHandler.text;
            yield return output;
            // Show results as text

            // Or retrieve results as binary data
            byte[] results = www.downloadHandler.data;
            Debug.Log("HttpRequestHelper, GetMazeJson : success : " + results.Length);
        }
    }

    public IEnumerator TryLogin(string name, string password)
    {
        WWWForm data = new WWWForm();
        data.AddField("name", name);
        data.AddField("password", password);

        UnityWebRequest www = UnityWebRequest.Post(API_URL + "user/login", data);

        using (www)
        {

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("HttpRequestHelper, TryLogin : www.error = " + www.error);
                yield return www.error;
            }
            else
            {
                Debug.Log("HttpRequestHelper, TryLogin : www.response = " + www.downloadHandler.text);
                yield return www.downloadHandler.text;
            }

        }
    }
    public IEnumerator RegisterUser(string name, string email, string password)
    {
        WWWForm data = new WWWForm();
        data.AddField("name", name);
        data.AddField("email", email);
        data.AddField("password", password);

        UnityWebRequest www = UnityWebRequest.Post(API_URL + "user", data);

        using (www)
        {

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("HttpRequestHelper, RegisterUser : www.error = " + www.error);
                yield return false;
            }
            else
            {
                Debug.Log("HttpRequestHelper, RegisterUser : www.success = " + www.downloadHandler.text);
                yield return www.downloadHandler.text;
            }

        }
    }

    public IEnumerator UploadMaze(string name, string mapData, int authorId)
    {
        WWWForm data = new WWWForm();
        data.AddField("name", name);
        data.AddField("user_id", authorId);
        data.AddField("composition", mapData);

        UnityWebRequest www = UnityWebRequest.Post(API_URL + "maze", data);

        www.SetRequestHeader("USERKEY", MazeUser.GetInstance().GetApiKey());

        using (www)
        {

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("HttpRequestHelper, UpdateMap : www.error = " + www.error);
                yield return false;
            }
            else
            {
                Debug.Log("HttpRequestHelper, UpdateMap : www.success = " + www.downloadHandler.text);
                yield return true;
            }

        }
    }
    public IEnumerator UpdateMaze(int id, string all)
    {
        using (UnityWebRequest www = UnityWebRequest.Put(API_URL + "maze/" + id, all))
        {
            www.SetRequestHeader("Accept", "application/json");
            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("USERKEY", MazeUser.GetInstance().GetApiKey());
            www.uploadHandler.contentType = "application/json";
            Debug.Log("HttpRequestHelper, UpdateMap : rawdata = " + all);
            www.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(all));

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("HttpRequestHelper, UpdateMap : www.error = " + www.error);
                yield return false;
            }
            else
            {
                Debug.Log("HttpRequestHelper, UpdateMap : www.success = " +  www.downloadHandler.text);
                yield return true;
            }
        }
    }
}