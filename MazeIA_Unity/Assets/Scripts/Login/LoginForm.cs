using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginForm : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] Button loginBtn;
    [SerializeField] InputField nameInput;
    [SerializeField] InputField passwordInput;
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject signUpMenu;
    [SerializeField] Button signUpButton;
    [SerializeField] Image loadImage;
    [Header("Prefabs")]
    [SerializeField] private GameObject alertPrefab;

    // Start is called before the first frame update
    void Start()
    {
        loginBtn.onClick.AddListener(delegate { Trylogin(nameInput.text, passwordInput.text); });
        signUpButton.onClick.AddListener(delegate { 
            signUpMenu.SetActive(true);
            gameObject.SetActive(false);
        });
        if(MazeUser.GetInstance().GetId() != 0)
        {
            mainMenu.SetActive(true);
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Trylogin(string name, string password)
    {
        var http = gameObject.AddComponent<HttpRequestHelper>();
        CoroutineWithData cd = new CoroutineWithData(this, http.TryLogin(name, password));
        StartCoroutine(WaitForLogin(cd));

        Debug.Log("LoginForm, TryLogin : Credentials = " + name + ", " + password);
    }
    private IEnumerator WaitForLogin(CoroutineWithData corout)
    {
        while (!(corout.result is string) || corout.result == null)
        {
            Debug.Log("EditorUI, WaitForLogin : data is null");
            yield return false;
        }
        var alertObj = Instantiate(alertPrefab, transform);
        var messageAlert = alertObj.GetComponent<MessageAnimation>();
        var result = (string)corout.result;
        Debug.Log("EditorUI, WaitForLogin : data = " + result);
        if (result.Contains("[error501] : "))
        {
            messageAlert.SetUpMessage("Error", "Invalid credentials :\r\n" + result, MessageAnimation.Colors.Error);

            yield return false;
        }
        else
        {
            var responseLogin = JsonUtility.FromJson<MazeUser.SaveObject>(result);
            Debug.Log("EditorUI, WaitForLogin : login = " + responseLogin.id + ", " + responseLogin.name + ", " + responseLogin.apiKey);
            messageAlert.SetUpMessage("Welcome back :", "Glad to see you " + responseLogin.name, MessageAnimation.Colors.Success, transform.parent);
            SetUser(responseLogin);
            Debug.Log("LoginForm, WaitForLogin : " + MazeUser.GetInstance().ToString());
            NextMenu();
            yield return true;
        }
    }
    private void SetUser(MazeUser.SaveObject user)
    {
        user.SetUserInfos();
    }
    private void NextMenu()
    {
        mainMenu.SetActive(true);
        gameObject.SetActive(false);
    }
}
