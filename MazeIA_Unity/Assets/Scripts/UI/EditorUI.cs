using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;




public class EditorUI : ModeUI
{

    [SerializeField] List<Button> tileButtonList = new List<Button>();
    [SerializeField] Button generateButton;

    [SerializeField] MazeEditor editor;

    [SerializeField] List<Slider> sizeSlider = new List<Slider>();
    [SerializeField] List<Text> sizeText = new List<Text>();
    

    // Start is called before the first frame update
    void Start()
    {
        SetBackbutton();
        //mazeManager = FindObjectOfType<MazeManager>();
        sizeText[0].text = sizeSlider[0].value.ToString();
        sizeText[1].text = sizeSlider[1].value.ToString();

        editor = GetComponentInParent<MazeEditor>();

        tileButtonList[0].onClick.AddListener(delegate { ChangeTile(1); });
        tileButtonList[1].onClick.AddListener(delegate { ChangeTile(2); });
        tileButtonList[2].onClick.AddListener(delegate { ChangeTile(3); });
        tileButtonList[3].onClick.AddListener(delegate { ChangeTile(4); });
        tileButtonList[4].onClick.AddListener(delegate { ChangeTile(5); });
        tileButtonList[5].onClick.AddListener(delegate { ChangeTile(6); });


        sizeSlider[0].onValueChanged.AddListener(delegate { SetWidth(sizeSlider[0].value); });
        sizeSlider[1].onValueChanged.AddListener(delegate { SetHeight(sizeSlider[1].value); });

        generateButton.onClick.AddListener(delegate {
            StartCoroutine( map.GenerateGrid(GetWidth(), GetHeight()));
        });

    }

    public void SetWidth(float width)
    {
        width = Mathf.CeilToInt(width);
        sizeText[0].text = width.ToString();
    }

    public int GetWidth()
    {
        return int.Parse(sizeText[0].text);
    }

    public void SetHeight(float height)
    {
        height = Mathf.CeilToInt(height);
        sizeText[1].text = height.ToString();
    }

    public int GetHeight()
    {
        return int.Parse(sizeText[1].text);
    }

    private void ChangeTile(int tileNb)
    {
        Debug.Log("EditorUI, ChangeTile : tile nb = " + tileNb);
        editor.SetCurrentTile(tileNb);
    }


}
