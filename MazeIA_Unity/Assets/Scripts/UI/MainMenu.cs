using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] List<Button> buttons = new List<Button>();
    [SerializeField] Button backButton;
    [SerializeField] GameObject loginMenu;

    // Start is called before the first frame update
    void Start()
    {
        buttons[0].onClick.AddListener(delegate { SceneManager.LoadScene("BetScene", LoadSceneMode.Single); });
        buttons[1].onClick.AddListener(delegate { SceneManager.LoadScene("EditorScene", LoadSceneMode.Single); });
        buttons[2].onClick.AddListener(delegate { SceneManager.LoadScene("ResolverScene", LoadSceneMode.Single); });
        backButton.onClick.AddListener(delegate {
            loginMenu.SetActive(true);
            gameObject.SetActive(false);
        });
    }

}
