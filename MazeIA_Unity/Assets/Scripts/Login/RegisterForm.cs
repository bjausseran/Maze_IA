using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegisterForm : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] Button registerBtn;
    [SerializeField] InputField nameInput;
    [SerializeField] InputField emailInput;
    [SerializeField] InputField passwordInput;
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject signInMenu;
    [SerializeField] Button signInButton;
    [SerializeField] Image loadImage;
    [Header("Prefabs")]
    [SerializeField] private GameObject alertPrefab;

    // Start is called before the first frame update
    void Start()
    {
        registerBtn.onClick.AddListener(delegate { TryRegister(nameInput.text, emailInput.text, passwordInput.text); });
        signInButton.onClick.AddListener(delegate { 
            signInMenu.SetActive(true); 
            gameObject.SetActive(false);
        });
    }

    // Update is called once per frame
    void TryRegister(string name, string email, string password)
    {
        var http = gameObject.AddComponent<HttpRequestHelper>();
        CoroutineWithData cd = new CoroutineWithData(this, http.RegisterUser(name, email, password));
        StartCoroutine(WaitForRegister(cd));

        Debug.Log("LoginForm, TryRegister : Credentials = " + name + ", " + email + ", " + password);
    }
    private IEnumerator WaitForRegister(CoroutineWithData corout)
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
        if (result.Contains("error"))
        {
            messageAlert.SetUpMessage("Error", "An error occured :\r\n" + result, MessageAnimation.Colors.Error);

            yield return false;
        }
        else
        {
            var responseLogin = JsonUtility.FromJson<MazeUser.SaveObject>(result);
            Debug.Log("EditorUI, WaitForLogin : login = " + responseLogin.id + ", " + responseLogin.name + ", " + responseLogin.apiKey);
            messageAlert.SetUpMessage("Welcome :", "Glad to see you " + responseLogin.name, MessageAnimation.Colors.Success, transform.parent);
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
