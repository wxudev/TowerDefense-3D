using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Tower : MonoBehaviour
{
    public float attack;//攻击力
    public float attackSpeed;//攻击速度
    public float attackRange;//攻击半径
    public Transform target;//攻击对象
    public float rotateSpeed;//旋转速度
    public float lastShootTime;//上一次设计的时间
    public float recharge;//射击间隔
    public int expend;//建造炮塔消耗的金币
    public List<Transform> muzzle;//枪口
    public Transform turrent;//炮身
    public GameObject bullet;//子弹
    private EnemyManager enemyManager;
    private UIManager uiManager;
    // Start is called before the first frame update
    void Start()
    {
        enemyManager = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();
        uiManager = GameObject.Find("Canvas").GetComponentInParent<UIManager>();
    }
    // Update is called once per frame

    public void DestroyTower()
    {
        GameData.Instance.coins += expend * 0.7f;
        uiManager.UpdateBattleData();
        Destroy(this.gameObject);
    }
    void Update()
    {
        //如果没有炮塔底座，不需要进行旋转瞄准
        if (turrent == null)
        {
            return;
        }
        GetAttackTarget();
        RotateTarget();
        Shooting();
    }

    //开火射击
    //1个引用
    void Shooting()
    {
        if (target != null)
        {
            //计算炮口和炮身的位置角度
            Vector3 a = target.position - turrent.position + turrent.forward;
            Vector3 b = target.position - turrent.position;
            //夹角小于10度，开始攻击
            if (Vector3.Angle(a, b) <= 10)
            {
                //射击CD
                if (Time.time - lastShootTime <= recharge / attackSpeed)
                {
                    return;
                }
                if (bullet != null)
                {
                    lastShootTime = Time.time;
                    //遍历炮口来设置子弹
                    for (int i = 0; i < muzzle.Count; i++)
                    {
                        GameObject bulletGo = GameObject.Instantiate(bullet);
                        bulletGo.tag = "Bullet";
                        bulletGo.transform.position = muzzle[i].position;
                        bulletGo.transform.localScale *= 0.1f;
                        bulletGo.transform.eulerAngles = muzzle[i].eulerAngles;
                        //添加攻击音效
                        AudioSource audio = bulletGo.AddComponent<AudioSource>();
                        audio.clip = Resources.Load<AudioClip>("audio/attack");
                        audio.Play();
                    }
                }
            }
        }
    }

    //计算攻击目标
    void GetAttackTarget()
    {
        if (target != null)
        {
            //判断炮塔和攻击目标是否在攻击范围内
            if (Vector3.Distance(target.transform.position, transform.position) <= attackRange)
            {
                return;
            }
        }
        //攻击目标为空 获取到敌人
        for (int i = 0; i < enemyManager.enemys.Count; i++)
        {
            GameObject enemy = enemyManager.enemys[i];
            if (enemy != null)
            {
                if (Vector3.Distance(enemy.transform.position, transform.position) <= attackRange)
                {
                    target = enemy.transform;
                    return;
                }
            }
        }
    }

    //确定好攻击目标后需要将炮塔进行旋转，瞄准攻击目标
    void RotateTarget()
    {
        if (target != null)
        {
            Quaternion quaternion = Quaternion.FromToRotation(Vector3.forward, target.transform.position - turrent.position);
            Quaternion qua = Quaternion.Lerp(turrent.rotation, quaternion, Time.deltaTime * rotateSpeed * attackSpeed);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, qua.eulerAngles.y, transform.eulerAngles.z);
            turrent.eulerAngles = new Vector3(qua.eulerAngles.x, turrent.eulerAngles.y, turrent.eulerAngles.z);
        }
    }
}