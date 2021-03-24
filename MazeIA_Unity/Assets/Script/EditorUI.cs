using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditorUI : MonoBehaviour
{
    [SerializeField] List<Button> buttonList = new List<Button>();
    [SerializeField] List<MazeTile> tileList = new List<MazeTile>();
    [SerializeField] MazeEditor editor;

    

    // Start is called before the first frame update
    void Start()
    {
        editor = GetComponentInParent<MazeEditor>();

        buttonList[0].onClick.AddListener(delegate { ChangeTile(0); });
        buttonList[1].onClick.AddListener(delegate { ChangeTile(1); });
        buttonList[2].onClick.AddListener(delegate { ChangeTile(2); });
        buttonList[3].onClick.AddListener(delegate { ChangeTile(3); });
    }

    private void ChangeTile(int tileNb)
    {
        Debug.Log("EditorUI, ChangeTile : tile nb = " + tileNb);
        editor.SetCurrentTile(tileList[tileNb]);
    }

}
