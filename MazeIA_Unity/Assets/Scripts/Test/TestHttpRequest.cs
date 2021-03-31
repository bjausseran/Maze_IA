using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class TestHttpRequest : MonoBehaviour
{
    const string API_URL = "mazelaravel.test/api/";


    public IEnumerator GetMazeList()
    {
        UnityWebRequest www = UnityWebRequest.Get("http://mazelaravel.test/api/mazelist");
        yield return www.SendWebRequest();
        string output = null;
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("TestHttpRequest, GetMazeList: output error : " + www.error);
            yield return null;
        }
        else
        {
            output = www.downloadHandler.text;
            yield return output;
            // Show results as text
            Debug.Log("TestHttpRequest, GetMazeList : output success : " + output);

            // Or retrieve results as binary data
            byte[] results = www.downloadHandler.data;
        }
    }
}