using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum FileMode
{
    Save,
    Load,
}
public class MazeManager : MonoBehaviour
{

    [Header("Components")]
    [SerializeField] MazeMap map;
    [SerializeField] MazeMap.SavedObject[] mapList;
    [SerializeField] MazeMap.SavedObject selectedMap = null;
    TilePath tileBase;
    [Header("Infos")]
    [SerializeField] bool fromHttp = true;
    [SerializeField] string mapName = "";
    [SerializeField] MazeModes mode;
    [Header("UI")]
    [SerializeField] Button selectedButton = null;
    [SerializeField] Image loadImage;
    [SerializeField] List<GameObject> fileMenus = new List<GameObject>();
    [SerializeField] List<Button> fileButton = new List<Button>();
    [SerializeField] InputField nameInput;
    [SerializeField] private Transform buttonListContent;
    [Header("Prefabs")]
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private GameObject alertPrefab;

    private void Awake()
    {
        buttonListContent.parent.parent.gameObject.SetActive(false);
        nameInput.onValueChanged.AddListener(delegate { SetMapName(nameInput.text); });
        fileButton[0].onClick.AddListener(delegate { RequestMap(); });
        fileButton[1].onClick.AddListener(delegate { SaveMap(); });
    }
    public void SetMap(MazeMap map)
    {
        this.map = map;
    }

    public MazeMap GetMap()
    {
        return map;
    }
    private void ClearWindow()
    {
        foreach (Transform child in buttonListContent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        selectedMap = null;
        buttonListContent.parent.parent.gameObject.SetActive(false);
        fileButton.Clear();
        selectedButton = null;
        loadImage.enabled = true;
    }
    private bool ShowHideWindow(FileMode mode)
    {
        ClearWindow();
        if (mode == FileMode.Load)
        {
            if (fileMenus[0].activeSelf)
            {
                fileMenus[0].SetActive(false);
                return false;
            }
            else
            {
                fileMenus[1].SetActive(false);
                fileMenus[0].SetActive(true);
                return true;
            }
        }
        else
        {
            if (fileMenus[1].activeSelf)
            {
                fileMenus[1].SetActive(false);
                return false;
            }
            else
            {
                fileMenus[0].SetActive(false);
                fileMenus[1].SetActive(true);
                return true;
            }
        }
    }
    public void DisplayFileWindow(FileMode mode)
    {
        if(!ShowHideWindow(mode)) return;

        buttonListContent.parent.parent.gameObject.SetActive(true);

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
        var responseList = JsonHelper.FromJson<MazeMap.SavedObject>((string)corout.result);
        mapList = responseList;
        Debug.Log("EditorUI, WaitForData : data length = " + responseList.Length);
        CreateButtons(mapList, mode);
        yield return true;
    }
    private IEnumerator WaitForJson(CoroutineWithData corout, FileMode mode)
    {
        if (mode == FileMode.Load)
        {
            while (corout.result == null || (!(corout.result is string) && !(corout.result is bool)))
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
                Debug.Log("EditorUI, WaitForData : data result = " + (string)corout.result);
                var responseList = JsonHelper.FromJson<MazeTile.SaveObject>((string)corout.result);
                if (map == null) map = new MazeMap(1, 1, 0.5f, null, this.mode);
                if (map.LoadFromAPI((string)corout.result))
                {
                    FindObjectOfType<ModeUI>().SetUIMapNap(selectedMap.name);
                    if (map.GetMode() == MazeModes.Bet)
                    {
                        var mazeBet = FindObjectOfType<MazeBet>();
                        mazeBet.DeleteBot();
                        mazeBet.SetMap(map);
                        mazeBet.CreateBot();
                    }
                    if (map.GetMode() == MazeModes.Resolver)
                    {
                        var mazeResolver = FindObjectOfType<MazeResolver>();
                        mazeResolver.SetMap(map);
                        var resolverUi = FindObjectOfType<ResolverUI>();
                        resolverUi.ResetValues();
                    }
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

            FindObjectOfType<ModeUI>().SetUIMapNap(mapName);
            DisplayFileWindow(mode);
            DisplayFileWindow(mode);
            yield return true;
        }
    }

    public void SetMapName(string mapName)
    {
        this.mapName = mapName;
    }
    private void CreateButtons(MazeMap.SavedObject[] map, FileMode mode)
    {
        loadImage.enabled = false;
        for (int i = 0; i < map.Length; i++)
        {
            GameObject button = Instantiate(buttonPrefab);
            //var str = map[i].Replace(".json", "");
            var str = map[i];
            var buttonText = button.GetComponentInChildren<Text>();
            buttonText.text = str.name;


            button.GetComponent<Button>().onClick.AddListener(delegate { SetSelectedMap(str, button.GetComponent<Button>(), buttonText); });
            button.transform.SetParent(buttonListContent);

        }
    }
    public void SetSelectedMap(MazeMap.SavedObject map, Button button, Text text)
    {
        //If you choose to select no map, to save a new one for exemple
        if (map == selectedMap)
        {
            this.selectedMap = null;
            this.nameInput.text = "";
            this.selectedButton = null;
            button.colors = UISetter.GetDefaultColorBlock();
            text.color = new Color(0.2f, 0.15f, 0.1f);

        }
        else
        {
            //You select a other map, the new one is no more
            if (selectedMap != null && selectedMap.id != 0)
            {
                selectedButton.colors = ColorBlock.defaultColorBlock;
                selectedButton.GetComponentInChildren<Text>().color = new Color(0.2f, 0.15f, 0.1f);
            }

            this.selectedButton = button;
            this.selectedMap = map;
            this.nameInput.text = map.name;

            button.colors = UISetter.GetSelectedColorBlock();
            text.color = new Color(0.9f, 0.85f, 0.7f);
        }
    }

    public void RequestMap()
    {
        if (selectedMap == null || selectedMap.id == 0)
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
        foreach (MazeMap.SavedObject obj in mapList)
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

        FindObjectOfType<ModeUI>().SetUIMapNap(selectedMap.name);
    }
    public void SaveMap()
    {
        var http = gameObject.AddComponent<HttpRequestHelper>();
        var id = 0;
        if (selectedMap != null) id = selectedMap.id;
        var mazeData = map.GetDataToUpload();
        var authorId = MazeUser.GetInstance().GetId();
        var rawdata = "{\"name\":\"" + mapName.Replace("\"", "\\\"") + "\",\"user_id\":\"" + authorId + "\",\"composition\":\"" + mazeData.Replace("\"", "\\\"") + "\"}";

        Debug.Log("EditorUI, SaveMap : rawdata : " + rawdata);  //  'success' or 'fail'
        Debug.Log("EditorUI, SaveMap : parse name : " + mapName);  //  'success' or 'fail'
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
            StartCoroutine(WaitForJson(new CoroutineWithData(this, http.UploadMaze(mapName, mazeData, authorId)), FileMode.Save));
        }
        else
        {
            StartCoroutine(WaitForJson(new CoroutineWithData(this, http.UpdateMaze(id, rawdata)), FileMode.Save));
        }

    }

}
