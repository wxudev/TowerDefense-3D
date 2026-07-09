using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private Transform mainPage;//主界面
    private Transform battlePage;//战斗界面
    private Transform failPage;//失败界面
    private Button startBtn;//开始游戏按钮
    private Button settingBtn;//设置按钮
    private Button quitBtn;//退出游戏按钮
    private Button[] towerButtons = new Button[5];//炮塔按钮列表
    private Button lastClick;//上一次点击的按钮
    private Button pauseBtn;//暂停按钮
    public Sprite pauseSprite;//暂停按钮的图片
    public Sprite resumeSprite;//取消暂停按钮的图片
    private Transform gameTips;//金币不足的提示
    private Text gameTipsText;//金币不足的文本提示框
    private Text homeHP;//水晶血量
    private Text killText;//击杀数量
    private Text coinsText;//金币数量
    private Text enemyCountText;//剩余敌人数量
    private Text timeText;//时间文本
    private TowerManager towerManager;
    private Button restartBtn;//重新开始按钮
    // Start is called before the first frame update
    void Awake()
    {
        restartBtn=transform.Find("FailPage/ReturnBtn").GetComponent<Button>();
        restartBtn.onClick.AddListener(() =>
        {
            failPage.gameObject.SetActive(false);
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        });
        towerManager = GameObject.Find("TowerManager").GetComponent<TowerManager>();
        towerManager.enabled = false;
        Transform towerButtonParents = transform.Find("BattlePage/TowerContainers");
        for (int i = 0; i < 5; i++)
        {
            towerButtons[i] = towerButtonParents.GetChild(i).GetComponent<Button>();
        }
        for (int i = 0; i < towerButtons.Length; i++)
        {
            int index = i;
            Button btn = towerButtons[i];
            btn.onClick.AddListener(() => {
                towerManager.selectedIndex = index;
                if (lastClick != null)
                {
                    lastClick.GetComponent<Image>().color = new Color(0, 0, 0, 0.78f);
                }
                btn.gameObject.GetComponent<Image>().color = Color.yellow;
                lastClick = btn;
            });
        }
        //设置选中的默认值
        towerManager.selectedIndex = 0;
        towerButtons[0].gameObject.GetComponent<Image>().color = Color.yellow;
        lastClick = towerButtons[0];
        //设置炮塔的金币
        for (int i = 0; i < towerManager.towerPrefabs.Count; i++)
        {
            int expend = towerManager.towerPrefabs[i].GetComponent<Tower>().expend;
            towerButtons[i].transform.Find("Coin").GetComponent<Text>().text = expend.ToString();
        }
    

    mainPage = transform.Find("MainPage");
        battlePage = transform.Find("BattlePage");
        failPage = transform.Find("FailPage");
        startBtn = transform.Find("MainPage/Btn_Start").GetComponent<Button>();
        startBtn.onClick.AddListener(OnClickStartBtn);
        quitBtn = transform.Find("MainPage/Btn_Quit").GetComponent<Button>();
        quitBtn.onClick.AddListener(OnClickQuitBtn);

        //初始化战斗界面
        for (int i = 0; i < 5; i++)
        {
            towerButtons[i] = transform.Find("BattlePage/TowerContainers/Item" + i).GetComponent<Button>();
        }
        pauseBtn = transform.Find("BattlePage/Btn_Pause").GetComponent<Button>();
        gameTips = transform.Find("BattlePage/GameTips");
        gameTipsText = transform.Find("BattlePage/GameTips/Text").GetComponent<Text>();
        homeHP = transform.Find("BattlePage/HomeHP/Text").GetComponent<Text>();
        killText = transform.Find("BattlePage/Kill/Text").GetComponent<Text>();
        coinsText = transform.Find("BattlePage/Coins/Text").GetComponent<Text>();
        enemyCountText = transform.Find("BattlePage/EnemyCount/Text").GetComponent<Text>();
        timeText = transform.Find("BattlePage/Time").GetComponent<Text>();
        pauseBtn.onClick.AddListener(OnClickPauseBtn);
    }

    //暂停按钮
    void OnClickPauseBtn()
    {
        if (Time.timeScale == 1)
        {

            pauseBtn.transform.GetComponent<Image>().sprite = pauseSprite;
            Time.timeScale = 0;
        }
        else
        {

            pauseBtn.transform.GetComponent<Image>().sprite = resumeSprite;
            Time.timeScale = 1;
        }
    }

    void OnClickStartBtn()
    {
        mainPage.gameObject.SetActive(false);
        battlePage.gameObject.SetActive(true);
        GameData.Instance.SetLeveData(1);
        UpdateBattleData();
        timeText.text = "00:00";
        StartCoroutine(CountDown()); // 开启倒计时
        Camera.main.GetComponent<CameraManager>().GameStart();
    }
    //初始化战斗界面数据
    public void UpdateBattleData()
    {
        homeHP.text = GameData.Instance.homeHP.ToString();
        coinsText.text = GameData.Instance.coins.ToString();
        killText.text = GameData.Instance.killCount.ToString();
        enemyCountText.text = GameData.Instance.enemyCount.ToString();
    }
    //怪物生成倒计时
    IEnumerator CountDown()
    {
        gameTips.gameObject.SetActive(true);
        for (int i = 0; i < 5; i++)
        {
            gameTipsText.text = $"怪物即将到来 {5 - i}";
            yield return new WaitForSeconds(1);
        }
        gameTipsText.text = "";
        gameTips.gameObject.SetActive(false);
        GameObject.Find("EnemyManager").GetComponent<EnemyManager>().enabled=true;
        towerManager.enabled = true;
        // 水晶血量大于0，开始计时游戏进行时长
        while (GameData.Instance.homeHP > 0)
        {
            yield return new WaitForSeconds(1);
            GameData.Instance.time += 1;
            float second = GameData.Instance.time % 60;
            timeText.text = $"{(int)(GameData.Instance.time / 60):00}:{second.ToString("00")}";
        }
    }


    //退出游戏
    void OnClickQuitBtn()
    {
        Debug.Log("退出游戏");
        Application.Quit();
    }

    void Update()
    {
        //测试代码 用于检测开发阶段代码是否有问题，正式阶段会移除
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CoinTip();
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            ShowGameResult(false);
        }
    }

    //游戏结束
    public void ShowGameResult(bool isWin)
    {
        if (!isWin)
        {
            failPage.gameObject.SetActive(true);
            Camera.main.GetComponent<CameraManager>().GameStop();
        }
        else
        {
            Debug.Log("游戏胜利！");
        }
    }

    //金币不足的提示
    public void CoinTip()
    {
        GameObject tips = GameObject.Instantiate(gameTips.gameObject);
        tips.transform.SetParent(gameTips.transform.parent);
        Text t = tips.transform.Find("Text").GetComponent<Text>();
        t.text = "金币不足！";
        tips.transform.position = gameTips.transform.position;
        tips.gameObject.AddComponent<MoveTween>();
        tips.gameObject.SetActive(true);
    }

}


