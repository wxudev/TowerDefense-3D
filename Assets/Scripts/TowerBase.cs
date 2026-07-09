using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBase : MonoBehaviour
{
    public Transform targetPos;
    public GameObject tower;//记录下所创建的炮塔

    // Start is called before the first frame update
    void Start()
    {
        targetPos = transform.Find("pos");
    }

    // Update is called once per frame
    void Update()
    {

    }
}

