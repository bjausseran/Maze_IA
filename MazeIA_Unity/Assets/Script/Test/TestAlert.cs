using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestAlert : MonoBehaviour
{
    public List<Button> button = new List<Button>();
    [SerializeField] private GameObject alertPrefab;

    private void Start()
    {
        button[0].onClick.AddListener(delegate {

            var alertObj = Instantiate(alertPrefab, transform);
            var messageAlert = alertObj.GetComponent<MessageAnimation>();
            messageAlert.SetUpMessage("Success :", "This is a success", MessageAnimation.Colors.Success);
        });

        button[1].onClick.AddListener(delegate {

            var alertObj = Instantiate(alertPrefab, transform);
            var messageAlert = alertObj.GetComponent<MessageAnimation>();
            messageAlert.SetUpMessage("Info :", "This is an info", MessageAnimation.Colors.Info);
        });

        button[2].onClick.AddListener(delegate {

            var alertObj = Instantiate(alertPrefab, transform);
            var messageAlert = alertObj.GetComponent<MessageAnimation>();
            messageAlert.SetUpMessage("Warning :", "This is a warning", MessageAnimation.Colors.Warning);
        });

        button[3].onClick.AddListener(delegate {

            var alertObj = Instantiate(alertPrefab, transform);
            var messageAlert = alertObj.GetComponent<MessageAnimation>();
            messageAlert.SetUpMessage("Alerte :", "This is an alert", MessageAnimation.Colors.Alerte);
        });
    }
}
