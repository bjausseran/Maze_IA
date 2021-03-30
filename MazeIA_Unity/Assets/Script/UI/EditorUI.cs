using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditorUI : MonoBehaviour
{
    [SerializeField] List<Button> buttonList = new List<Button>();
    [SerializeField] MazeEditor editor;

    

    // Start is called before the first frame update
    void Start()
    {
        editor = GetComponentInParent<MazeEditor>();

        buttonList[0].onClick.AddListener(delegate { ChangeTile(1); });
        buttonList[1].onClick.AddListener(delegate { ChangeTile(2); });
        buttonList[2].onClick.AddListener(delegate { ChangeTile(3); });
        buttonList[3].onClick.AddListener(delegate { ChangeTile(4); });
        buttonList[4].onClick.AddListener(delegate { ChangeTile(5); });
        buttonList[5].onClick.AddListener(delegate { ChangeTile(6); });
    }
    public void DisplayLoadWindow()
    {
        var fileList = SaveSystem.GetFileList();
        GameObject window = new GameObject(name + "window", typeof(RectTransform));
        window.transform.SetParent(transform); 
        foreach(string file in fileList)
        {
            GameObject fileObject = new GameObject(name + "file", typeof(Text));
            fileObject.GetComponent<Text>().text = file;
            fileObject.transform.SetParent(window.transform);
        }
    }
    private void ChangeTile(int tileNb)
    {
        Debug.Log("EditorUI, ChangeTile : tile nb = " + tileNb);
        editor.SetCurrentTile(tileNb);
    }

}
