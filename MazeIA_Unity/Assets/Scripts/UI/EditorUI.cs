using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;




public class EditorUI : MonoBehaviour
{
    public enum FileMode
    {
        Save,
        Load,
    }
    [SerializeField] bool fromHttp = true;

    [SerializeField] List<Button> buttonList = new List<Button>();
    [SerializeField] Button selectedButton = null;

    [SerializeField] MazeMap.SavedObject[] mapList;
    [SerializeField] MazeMap.SavedObject selectedMap = null;
    [SerializeField] string mapName = "";

    ColorBlock buttonColorBlock = ColorBlock.defaultColorBlock;

    [SerializeField] List<Button> fileButton = new List<Button>();
    [SerializeField] MazeEditor editor;
    [SerializeField] MazeMap map;
    [SerializeField] InputField nameInput;
    [SerializeField] List<GameObject> fileMenus = new List<GameObject>();
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private GameObject alertPrefab;
    [SerializeField] private Transform buttonListContent;
    bool fileWindowUp = false;
    

    // Start is called before the first frame update
    void Start()
    {
        editor = GetComponentInParent<MazeEditor>();
        buttonListContent.parent.parent.gameObject.SetActive(false);

        buttonList[0].onClick.AddListener(delegate { ChangeTile(1); });
        buttonList[1].onClick.AddListener(delegate { ChangeTile(2); });
        buttonList[2].onClick.AddListener(delegate { ChangeTile(3); });
        buttonList[3].onClick.AddListener(delegate { ChangeTile(4); });
        buttonList[4].onClick.AddListener(delegate { ChangeTile(5); });
        buttonList[5].onClick.AddListener(delegate { ChangeTile(6); });
        fileButton[0].onClick.AddListener(delegate { RequestMap(); });
        fileButton[1].onClick.AddListener(delegate { SaveMap(); });
        nameInput.onValueChanged.AddListener(delegate { SetMapName(nameInput.text); });
    }
    public void SetMap(MazeMap map)
    {
        this.map = map;
    }

    public MazeMap GetMap()
    {
        return map;
    }

    public void DisplayFileWindow(FileMode mode)
    {
        if (fileWindowUp)
        {
            foreach (Transform child in buttonListContent.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            foreach (GameObject menu in fileMenus)
            {
                menu.SetActive(false);
            }
            selectedMap = null;
            buttonListContent.parent.parent.gameObject.SetActive(false);
            fileWindowUp = false;
            fileButton.Clear();
            selectedButton = null;
            return;
        }
        fileWindowUp = true;
        var fileList = new List<string>();
        if (!fromHttp) fileList = SaveSystem.GetFileList();
        else
        {
            var http = gameObject.AddComponent<HttpRequestHelper>(); 
            CoroutineWithData cd = new CoroutineWithData(this, http.GetMazeList());
            StartCoroutine(WaitForNameList(cd, mode));
            Debug.Log("EditorUI, DisplayFileWindow : http response : " + cd.result);  //  'success' or 'fail'
        }
    }

    private IEnumerator WaitForNameList(CoroutineWithData corout, FileMode mode)
    {
        while (!(corout.result is string) || corout.result == null)
        {
            Debug.Log("EditorUI, WaitForData : data is null");
            yield return false;
        }
        Debug.Log("EditorUI, WaitForData : data = " + corout.result); 
        var responseList = JsonHelper.FromJson<MazeMap.SavedObject>((string) corout.result);
        mapList = responseList;
        Debug.Log("EditorUI, WaitForData : data length = " + responseList.Length);
        CreateButtons(mapList, mode);
        yield return true;
    }
    private IEnumerator WaitForJson(CoroutineWithData corout, FileMode mode)
    {
        if (mode == FileMode.Load)
        {
            while (corout.result == null || ( !(corout.result is string) && !(corout.result is bool)) )
            {
                Debug.Log("EditorUI, WaitForData : data is null");
                yield return false;
            }
            var alertObj = Instantiate(alertPrefab, transform);
            var messageAlert = alertObj.GetComponent<MessageAnimation>();
            messageAlert.SetUpMessage("Error while load :", "Map not load", MessageAnimation.Colors.Error);
            Debug.Log("EditorUI, WaitForData : data = " + corout.result);

            if (corout.result is string)
            { 
                var responseList = JsonHelper.FromJson<MazeTile.SaveObject>((string)corout.result);
                Debug.Log("EditorUI, WaitForData : data length = " + responseList.Length);
                if (map.LoadFromAPI((string)corout.result))
                {
                    messageAlert.SetUpMessage("Maze loaded :", selectedMap.name + " looks like a\r\ngood playground", MessageAnimation.Colors.Success);
                }
            }

        }
        else
        {
            while (!(corout.result is bool))
            {
                Debug.Log("EditorUI, WaitForData : data is null");
                yield return false;
            }
            var alertObj = Instantiate(alertPrefab, transform);
            var messageAlert = alertObj.GetComponent<MessageAnimation>();
            messageAlert.SetUpMessage("Error while saving :", mapName + " not saved", MessageAnimation.Colors.Error);
            if ((bool)corout.result) messageAlert.SetUpMessage("Maze saved :", "You can be\r\nproud of " + mapName, MessageAnimation.Colors.Success);

            yield return true;
        }
    }

    public void SetMapName(string mapName)
    {
        this.mapName = mapName;
    }
    private void CreateButtons(MazeMap.SavedObject[] map, FileMode mode)
    {

        foreach (Transform child in buttonListContent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        for (int i = 0; i < map.Length; i++)
        {
            GameObject button = Instantiate(buttonPrefab);
            //var str = map[i].Replace(".json", "");
            var str = map[i];
            var buttonText = button.GetComponentInChildren<Text>();
            buttonText.text = str.name;

            if (mode == FileMode.Load)
            {
                fileMenus[0].SetActive(true);
            }
            else if (mode == FileMode.Save)
            {
                fileMenus[1].SetActive(true);
            }

            button.GetComponent<Button>().onClick.AddListener(delegate { SetSelectedMap(str, button.GetComponent<Button>(), buttonText); });
            button.transform.SetParent(buttonListContent);

        }
        buttonListContent.parent.parent.gameObject.SetActive(true);
    }
    private void ChangeTile(int tileNb)
    {
        Debug.Log("EditorUI, ChangeTile : tile nb = " + tileNb);
        editor.SetCurrentTile(tileNb);
    }
    public void SetSelectedMap(MazeMap.SavedObject map, Button button, Text text)
    {
        //If you choose to select no map, to save a new one for exemple
        if (map == selectedMap)
        {
            this.selectedMap = null;
            this.nameInput.text = "";
            this.selectedButton = null;
            button.colors = ColorBlock.defaultColorBlock;
            text.color = new Color(0.2f, 0.15f, 0.1f);

        }
        else
        {
            //You select a other map, the new one is no more
            if (selectedMap.id != 0)
            {
                selectedButton.colors = ColorBlock.defaultColorBlock;
                selectedButton.GetComponentInChildren<Text>().color = new Color(0.2f, 0.15f, 0.1f);
            }

            this.selectedButton = button;
            this.selectedMap = map;
            this.nameInput.text = map.name;

            button.colors = buttonList[3].colors;
            text.color = new Color(0.9f, 0.85f, 0.7f);
        }
    }

    public void RequestMap()
    {
        if (selectedMap.id == 0)
        {
            var alertObj = Instantiate(alertPrefab, transform);
            var messageAlert = alertObj.GetComponent<MessageAnimation>();
            messageAlert.SetUpMessage("Select a map :", "Focus son,\r\nyou can do it", MessageAnimation.Colors.Warning);
            return;
        }
        var http = gameObject.AddComponent<HttpRequestHelper>();
        var id = GetSavedObjectId(selectedMap.name);
        CoroutineWithData cd = new CoroutineWithData(this, http.GetMazeJson(id));
        StartCoroutine(WaitForJson(cd, FileMode.Load));
        Debug.Log("EditorUI, RequestMap : http response : " + cd.result);  //  'success' or 'fail'
    }

    private int GetSavedObjectId(string mazeName)
    {
        foreach(MazeMap.SavedObject obj in mapList)
        {
            if (obj.name == selectedMap.name) return obj.id;
        }
        return 0;
    }
    //Unused, can be use to upload map from file
    public void LoadMap()
    {
        var alertObj = Instantiate(alertPrefab, transform);
        var messageAlert = alertObj.GetComponent<MessageAnimation>();
        if (selectedMap.name == "")
        {
            messageAlert.SetUpMessage("Focus son :", "Add a name", MessageAnimation.Colors.Warning);
            return;
        }
        messageAlert.SetUpMessage("Maze loaded :", selectedMap.name + " looks like a\r\ngood playground", MessageAnimation.Colors.Success);
        map.LoadFromFile(selectedMap.name);
    }
    public void SaveMap()
    {
        var http = gameObject.AddComponent<HttpRequestHelper>();
        var id = 0;
        if (selectedMap != null) id = selectedMap.id;
        var mazeData = map.GetDataToUpload();
        var authorId = 1;
        var rawdata = "{\"name\":\"" + mapName.Replace("\"", "\\\"") + "\",\"user_id\":\"" + authorId + "\",\"composition\":\"" + mazeData.Replace("\"", "\\\"") + "\"}";

        Debug.Log("EditorUI, SaveMap : rawdata : " + rawdata);  //  'success' or 'fail'
        Debug.Log("EditorUI, SaveMap : parse name : " + selectedMap.name);  //  'success' or 'fail'
        Debug.Log("EditorUI, SaveMap : parse id : " + id);  //  'success' or 'fail'
        Debug.Log("EditorUI, SaveMap : parse composition : " + mazeData);  //  'success' or 'fail'

        if (mapName == "")
        {
            var alertObj = Instantiate(alertPrefab, transform);
            var messageAlert = alertObj.GetComponent<MessageAnimation>();
            messageAlert.SetUpMessage("Add a name :", "How are you\r\ngonna call that ?", MessageAnimation.Colors.Warning);
            return;
        }

        if (id == 0)
        {
            StartCoroutine(WaitForJson(new CoroutineWithData(this, http.UploadMaze(mapName, authorId, mazeData)), FileMode.Save));
        }
        else
        {
            StartCoroutine(WaitForJson(new CoroutineWithData(this, http.UpdateMaze(id, rawdata)), FileMode.Save));
        }

    }



}
