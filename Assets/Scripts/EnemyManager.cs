using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public List<GameObject> enemyPrefabs = new List<GameObject>(); // 存储敌人的预制体
    public List<GameObject> enemys = new List<GameObject>();      // 存储当前存活的敌人
    public bool isStop;                                           // 是否生成敌人的状态
    private UIManager uiManager;
    public Transform[] path;

    // Start is called before the first frame update
    void Start()
    {
        uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        StartCoroutine(Create());

        for(int i = 0; i < enemyPrefabs.Count; i++)
        {
            Enemy config = enemyPrefabs[i].GetComponent<Enemy>();
            config.id = i;
        }
    }

    // 敌人生成
    IEnumerator Create()
    {
        while (!isStop)
        {
            if (GameData.Instance.enemyCount > 0)
            {
                GameData.Instance.enemyCount -= 1;
                uiManager.UpdateBattleData();

                // 随机创建怪物
                int index = UnityEngine.Random.Range(0, enemyPrefabs.Count - 1);
                GameObject enemy = GameObject.Instantiate(enemyPrefabs[index].gameObject);

                // 设置怪物的起始位置和初始状态
                enemy.transform.position = path[0].position;
                enemy.transform.eulerAngles = path[0].eulerAngles;
                //设置怪物属性
                Enemy enemyScript = enemy.GetComponent<Enemy>();
                enemyScript.SetData(path, this);
                // 将生成的怪物添加到列表中进行存储
                enemys.Add(enemy);
            }

            // 当前要生成的怪物数量小于0，怪物生成完成
            if (GameData.Instance.enemyCount <= 0)
            {
                break;  
            }

            yield return new WaitForSeconds(3); // 每隔3秒生成一个怪物
        }
    }

    public void Stop()
    {
        isStop = true;

        for(int i = 0; i < enemys.Count; i++)
        {
            if (enemys[i] != null)
            {
                
            }
            enemys.Clear();
        }   

    }
}

