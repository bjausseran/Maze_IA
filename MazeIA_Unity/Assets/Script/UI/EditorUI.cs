using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditorUI : MonoBehaviour
{
    [SerializeField] List<Button> buttonList = new List<Button>();
    [SerializeField] MazeEditor editor;
    [SerializeField] MazeMap map;
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private Transform buttonListContent;
    

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
    public void SetMap(MazeMap map)
    {
        this.map = map;
    }

    public MazeMap GetMap()
    {
        return map;
    }

    public void DisplayLoadWindow()
    {
        var fileList = SaveSystem.GetFileList();

        GameObject window = new GameObject(name + "window", typeof(RectTransform));
        window.transform.SetParent(transform);
        RectTransform rect = window.GetComponent<RectTransform>();
        
        rect.anchorMin = new Vector2(0.30f, 0f);
        rect.anchorMax = new Vector2(0.7f, 1f);
        rect.localPosition = Vector3.zero;

        
        for (int i = 0; i < fileList.Count; i++)
        {
            GameObject button = Instantiate(buttonPrefab);
            var str = fileList[i];
            button.GetComponent<Button>().onClick.AddListener(delegate { map.Load(str); });
            button.transform.SetParent(buttonListContent);
            var buttonText = button.GetComponentInChildren<Text>();
            buttonText.text = str;
        }
    }
    private void ChangeTile(int tileNb)
    {
        Debug.Log("EditorUI, ChangeTile : tile nb = " + tileNb);
        editor.SetCurrentTile(tileNb);
    }

}
