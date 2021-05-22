using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;




public class ResolverUI : ModeUI
{

    [SerializeField] MazeResolver resolver;
    [SerializeField] Button pathfindingButton;
    [SerializeField] Text resolveText;
    [SerializeField] Text costText;
    [SerializeField] Text numberText;
    [SerializeField] MazeManager mazeManager;
    [SerializeField] Button rndButton;
    [SerializeField] Text randomMapName;


    // Start is called before the first frame update
    void Start()
    {
        rndButton.onClick.AddListener(delegate { mazeManager.RequestRandomMap(randomMapName.text); });
        SetBackbutton();
        resolver = GetComponentInParent<MazeResolver>();
        pathfindingButton.onClick.AddListener(delegate { SetValues(); });
    }
    public void ResetValues()
    {
        resolveText.text = "Unknow";
        costText.text = 0.ToString();
        numberText.text = 0.ToString();
    }
    public void SetValues()
    {
        resolveText.text = resolver.FindPath().ToString();
        costText.text = resolver.GetPathfinding().GetTotalCost().ToString();
        numberText.text = resolver.GetPathfinding().GetNumberOfSteps().ToString();
    }
}
