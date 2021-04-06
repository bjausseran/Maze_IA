using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeBet : MazeMode
{

    [Header("Component")]
    [SerializeField] BetUI ui;
    [SerializeField] Color[] color = { Color.red, Color.blue, Color.green, Color.black, Color.cyan, Color.gray, Color.magenta, Color.white };
    [SerializeField] TypeToTileConverter converter;
    [SerializeField] Pathfinding pathfinding;
    [SerializeField] List<MazeTile> tileList = new List<MazeTile>();
    [SerializeField] List<MazeBot> bots = new List<MazeBot>();
    bool botCanMove = false;

    private void Start()
    {
        converter = TypeToTileConverter.GetInstance();
        converter.SetArray(tileList.ToArray());
        ui.DisplayFileWindow();
        Debug.Log("MazeBet, Start : " + MazeUser.GetInstance().ToString());

    }
    public void ReinitializeBots()
    {
        foreach (MazeBot bot in bots)
        {
            bot.Initialize();
        }
    }

    public void SetBotFromUI()
    {
        if (bots.Count <= 0) return;
        SetBotCanMove(!botCanMove);
    }
    public void SetBotCanMove(bool value)
    {
        botCanMove = value;
        foreach (MazeBot bot in bots)
        {
            bot.SetCanMove(value);
        }
    }

    public void CreateBot()
    {
        Debug.Log("MazeBet, CreateBot : start");
        pathfinding = new Pathfinding(map.GetGrid());


        SetBotCanMove(false);

        GameObject bankObject = new GameObject("bot_", typeof(BotBank));
        bankObject.GetComponent<BotBank>().SetGrid(map.GetGrid());
        bankObject.GetComponent<BotBank>().SetColor(Color.yellow);

        bots.Add(bankObject.GetComponent<BotBank>());

        for (int i = 0; i < color.Length; i++)
        {
            GameObject gameObject = new GameObject("bot_", typeof(BotRunner));
            gameObject.GetComponent<BotRunner>().SetGrid(map.GetGrid());
            gameObject.GetComponent<BotRunner>().SetColor(color[i]);
            bots.Add(gameObject.GetComponent<BotRunner>());
        }
    }

    public void DeleteBot()
    {
        for (int i = 0; i < bots.Count; i++)
        {
            Destroy(bots[i].gameObject);
        }
        bots.Clear();
    }

        private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            ui.DisplayFileWindow();
        }
    }
}