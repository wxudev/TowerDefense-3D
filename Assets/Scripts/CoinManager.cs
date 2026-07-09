using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public GameObject item;//掉落的粒子效果
    // Start is called before the first frame update

    public GameObject Create()
    {
        GameObject itemGo = GameObject.Instantiate(item);
        return itemGo;
    }



    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
