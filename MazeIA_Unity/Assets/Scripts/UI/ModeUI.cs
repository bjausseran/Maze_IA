using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ModeUI : MonoBehaviour
{
    [SerializeField] protected MazeManager mazeManager;
    [SerializeField] protected Button backButton;
    [SerializeField] protected MazeMap map;
    [SerializeField] protected Text mapName;

    // Start is called before the first frame update
    protected void SetBackbutton()
    {
        backButton.onClick.AddListener(delegate { SceneManager.LoadScene("MenuScene", LoadSceneMode.Single); });
    }

    public void DisplayFileWindow()
    {
        mazeManager.DisplayFileWindow(FileMode.Load);
    }
    public void DisplayFileWindow(FileMode mode)
    {
        mazeManager.DisplayFileWindow(mode);
    }
    public void SetMap(MazeMap map)
    {
        this.map = map;
        mazeManager.SetMap(map);
    }

    public MazeMap GetMap()
    {
        return map;
    }
    public void SetUIMapNap(string mapName)
    {
        this.mapName.text = mapName;
    }
}
