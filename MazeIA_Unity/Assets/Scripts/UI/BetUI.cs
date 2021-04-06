using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;




public class BetUI : ModeUI
{

    [SerializeField] MazeBet bet;
    [SerializeField] Button startButton;
    [SerializeField] Button restartButton;




    // Start is called before the first frame update
    void Start()
    {
        SetBackbutton();
        bet = GetComponentInParent<MazeBet>();
        startButton.onClick.AddListener(delegate { bet.SetBotFromUI(); });
        restartButton.onClick.AddListener(delegate { bet.ReinitializeBots(); });

    }







}
