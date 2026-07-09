using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerManager : MonoBehaviour
{
    public List<GameObject> towerPrefabs;//存储所有的炮塔
    public int selectedIndex;//当前选择炮塔的索引值
    private UIManager uiManager;

    // Start is called before the first frame update
    void Start()
    {
        uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        CreateTower();
        DestroyTower();
    }

    //建造炮塔
    private void CreateTower()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;
            if (Physics.Raycast(ray, out raycastHit, 100, LayerMask.GetMask("TowerBase")))
            {
                if (raycastHit.collider.gameObject.CompareTag("TowerBase"))
                {
                    TowerBase towerBase = raycastHit.collider.gameObject.GetComponent<TowerBase>();
                    if (towerBase != null && towerBase.tower == null)
                    {
                        GameObject towerObj = GameObject.Instantiate(towerPrefabs[selectedIndex]);
                        Tower tower = towerObj.GetComponent<Tower>();
                        if (GameData.Instance.coins >= tower.expend)
                        {
                            towerObj.transform.position = towerBase.targetPos.position;
                            towerObj.transform.parent=towerBase.transform;
                            towerObj.transform.eulerAngles = raycastHit.collider.transform.eulerAngles;
                            towerBase.tower = towerObj;
                            GameData.Instance.coins -= tower.expend;
                            uiManager.UpdateBattleData();
                        }
                        else
                        {
                            uiManager.CoinTip();
                        }
                    }
                }
            }
        }
    }

    //销毁炮塔
    void DestroyTower()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
            {
                if (hitInfo.collider.gameObject.CompareTag("Tower"))
                {
                    Tower tower = hitInfo.collider.GetComponent<Tower>();
                    if (tower != null)
                    {
                        tower.transform.GetComponentInParent<TowerBase>().tower = null;
                        tower.DestroyTower();
                    }
                }

                else if(hitInfo.collider.gameObject.CompareTag("Effect"))
                {
                    Tower tower = hitInfo.collider.transform.parent.parent.GetComponent<Tower>();
                    if (tower != null)
                    {
                        tower.transform.GetComponentInParent<TowerBase>().tower = null;
                        tower.DestroyTower();
                    }
                }
            }
        }
    }

}

