using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int id;
    public string enemyName;
    public int type;
    public int level;//等级
    public float hp;//生命值
    public float attack;//攻击力
    public float defensive;//防御力

    public float moveSpeed;//移动速度
    public float gold;//死亡时掉落的金币
    private float initMoveSpeed;//初始速度
    private bool isMoving;//是否移动中
    private Animator animator;
    private Rigidbody rigidbodyComp;
    private int pointIndex;//寻路起始点
    private Transform nextPoint;//下一个寻路点
    private Transform[] pointList;//获取寻路点集合
    private UIManager uiManager;
    private EnemyManager enemyManager;

    void Awake()
    {
        animator = this.GetComponent<Animator>();
        rigidbodyComp = this.GetComponent<Rigidbody>();
        uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
    }

    public void SetData(Transform[] point, EnemyManager enemyManager) //设置敌人初始化数据
    {
        if (GameData.Instance.levelID <= 0)
        {
            return;
        }
        //设置敌人属性 数值
        level = GameData.Instance.levelID;
        hp = GameData.Instance.levelID * 100;
        attack = GameData.Instance.levelID * 100;
        defensive = GameData.Instance.levelID * 10;
        moveSpeed = GameData.Instance.levelID * 3;
        gold = GameData.Instance.levelID * 100;
        initMoveSpeed = moveSpeed;
        this.pointList = point;
        this.enemyManager = enemyManager;
        isMoving = true;
    }

    public void Move()
    {
        if (!isMoving)
        {
            return;
        }

        //寻路
        if (nextPoint == null)
        {
            pointIndex = 0;
            nextPoint = pointList[pointIndex];
        }

        if (Vector3.Distance(transform.position, nextPoint.position) >= 1.0f) //判断敌人和路径点的差
        {
            transform.LookAt(nextPoint.position, Vector3.up);
            rigidbodyComp.velocity = transform.forward * moveSpeed;
        }
        else
        {
            //当到达下一个寻路点的时候，需要切换新的寻路点
            pointIndex++;
            //当超出寻路点，到达终点
            if (pointIndex >= pointList.Length)
            {
                rigidbodyComp.velocity = Vector3.zero;
                isMoving = false;
                if (GameData.Instance.homeHP > 0)
                {
                    GameData.Instance.homeHP -= attack; //水晶扣血
                    uiManager.UpdateBattleData(); //更新UI界面
                }
                if (GameData.Instance.homeHP <= 0)
                {
                    uiManager.ShowGameResult(false); //游戏结束
                    enemyManager.Stop();
                }
                GameObject.Destroy(this.gameObject);
                return;
            }
            nextPoint = pointList[pointIndex];
        }
    }

    private void Update()
    {
        Move();
    }

    //点击判定 碰撞器 触发器
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))//碰到的是子弹
        {
            if (hp <= 0)
            {
                return;
            }
            hp -= 50;//受到伤害
            if (hp <= 0)
            {
                isMoving = false;
                animator.Play("death_1");
                if (enemyManager != null)
                {
                    enemyManager.enemys.Remove(this.gameObject);
                }
                this.GetComponent<Rigidbody>().isKinematic = true;//敌人停止移动
                this.GetComponent<Collider>().enabled = false;//关闭碰撞
                GameData.Instance.killCount += 1;
                GameData.Instance.coins += this.gold;
                uiManager.UpdateBattleData();
                //游戏胜利判定
                if (GameData.Instance.killCount == GameData.Instance.allEnemyCount)
                {
                    uiManager.ShowGameResult(true);
                }
                //当怪物死亡后，需要在这个位置产生金币
                CoinManager coinManager = GameObject.Find("CoinManager").GetComponent<CoinManager>();
                GameObject coinObject = coinManager.Create();
                coinObject.transform.position=transform.position+new Vector3(0, 0.5f, 0);
                coinObject.gameObject.SetActive(true);
                GameObject.Destroy(coinObject, 1.0f);
                GameObject.Destroy(this.gameObject, 2.0f);
            }
        }
    }

    //攻击判定 触发器 三个阶段
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Effect"))
        {
            if (initMoveSpeed == moveSpeed)
            {
                moveSpeed /= 2;//减速一半
            }
        }
    }

    //离开减速区域
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Effect"))
        {
            moveSpeed = initMoveSpeed;//恢复速度
        }
    }

    //停止攻击
    public void Stop()
    {
        isMoving = false;
        rigidbodyComp.Sleep();
        animator.Play("idle01");
    }


}

