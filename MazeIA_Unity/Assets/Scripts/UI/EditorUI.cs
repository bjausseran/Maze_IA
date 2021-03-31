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
    [SerializeField] List<Button> fileButton = new List<Button>();
    [SerializeField] MazeEditor editor;
    [SerializeField] MazeMap map;
    [SerializeField] InputField nameInput;
    [SerializeField] List<GameObject> fileMenus = new List<GameObject>();
    [SerializeField] string currentFileString = null;
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
        fileButton[0].onClick.AddListener(delegate { LoadMap(); });
        fileButton[1].onClick.AddListener(delegate { SaveMap(); });
        nameInput.onValueChanged.AddListener(delegate { SetCurrentFileString(nameInput.text); });
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
            currentFileString = null;
            buttonListContent.parent.parent.gameObject.SetActive(false);
            fileWindowUp = false;
            return;
        }
        fileWindowUp = true;
        var fileList = new List<string>();
        if (!fromHttp) fileList = SaveSystem.GetFileList();
        else
        {
            var http = gameObject.AddComponent<TestHttpRequest>(); 
            CoroutineWithData cd = new CoroutineWithData(this, http.GetMazeList());
            StartCoroutine(WaitForData(cd, mode));
            Debug.Log("EditorUI, DisplayFileWindow : http response : " + cd.result);  //  'success' or 'fail'
        }

        foreach (Transform child in buttonListContent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        for (int i = 0; i < fileList.Count; i++)
        {
            GameObject button = Instantiate(buttonPrefab);
            var str = fileList[i].Replace(".json", "");
            var buttonText = button.GetComponentInChildren<Text>();
            buttonText.text = str;

            if (mode == FileMode.Load)
            {
                fileMenus[0].SetActive(true);
            }
            else if (mode == FileMode.Save)
            {
                fileMenus[1].SetActive(true);
            }

            button.GetComponent<Button>().onClick.AddListener(delegate { SetCurrentFileString(str); });
            button.transform.SetParent(buttonListContent);
            
        }
        buttonListContent.parent.parent.gameObject.SetActive(true);
    }

    private IEnumerator WaitForData(CoroutineWithData corout, FileMode mode)
    {
        while (!(corout.result is string) || corout.result == null)
        {
            Debug.Log("EditorUI, WaitForData : data is null");
            yield return false;
        }
        Debug.Log("EditorUI, WaitForData : data = " + corout.result); 
        var responseObj = JsonHelper.FromJson<MazeMap.SaveObjectMini>((string) corout.result);
        var response = new string[responseObj.Length];
        for(int i =0; i < response.Length; i++)
        {
            response = responseObj[i].nameList;
        }
        Debug.Log("EditorUI, WaitForData : data length = " + response.Length);
        CreateButtons(response, mode);
        yield return true;
    }

    private void CreateButtons(string[] names, FileMode mode)
    {

        foreach (Transform child in buttonListContent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        for (int i = 0; i < names.Length; i++)
        {
            GameObject button = Instantiate(buttonPrefab);
            var str = names[i].Replace(".json", "");
            var buttonText = button.GetComponentInChildren<Text>();
            buttonText.text = str;

            if (mode == FileMode.Load)
            {
                fileMenus[0].SetActive(true);
            }
            else if (mode == FileMode.Save)
            {
                fileMenus[1].SetActive(true);
            }

            button.GetComponent<Button>().onClick.AddListener(delegate { SetCurrentFileString(str); });
            button.transform.SetParent(buttonListContent);

        }
        buttonListContent.parent.parent.gameObject.SetActive(true);
    }
    private void ChangeTile(int tileNb)
    {
        Debug.Log("EditorUI, ChangeTile : tile nb = " + tileNb);
        editor.SetCurrentTile(tileNb);
    }
    public void SetCurrentFileString(string fileName)
    {
        this.currentFileString = fileName;
        nameInput.text = fileName;
    }

    public void LoadMap()
    {
        var alertObj = Instantiate(alertPrefab, transform);
        var messageAlert = alertObj.GetComponent<MessageAnimation>();
        if (currentFileString == "")
        {
            messageAlert.SetUpMessage("Error :", "Add a name", MessageAnimation.Colors.Alerte);
            return;
        }
        messageAlert.SetUpMessage("Maze loaded :", currentFileString, MessageAnimation.Colors.Success);
        map.Load(currentFileString);
    }
    public void SaveMap()
    {
        var alertObj = Instantiate(alertPrefab, transform);
        var messageAlert = alertObj.GetComponent<MessageAnimation>();
        if (currentFileString == "")
        {
            messageAlert.SetUpMessage("Error :", "Add a name", MessageAnimation.Colors.Alerte);
            return;
        }
        messageAlert.SetUpMessage("Maze saved :", currentFileString, MessageAnimation.Colors.Success);
        map.Save(currentFileString);
    }



}
