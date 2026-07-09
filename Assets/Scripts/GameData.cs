using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData Instance;
    public int levelID;
    public float coins;
    public int enemyCount;
    public int allEnemyCount;
    public int killCount;
    public float time;
    public float homeHP;
    // Start is called before the first frame update
    void Awake()
    {
        Instance=this;
    }

    public void SetLeveData(int levelID)
    {
        this.levelID=levelID;
        coins= 1000*levelID;
        enemyCount=100* levelID;
        allEnemyCount=enemyCount;
        killCount=0;
        time=0;
        homeHP=20* levelID;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
