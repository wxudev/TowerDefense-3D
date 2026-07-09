using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private Camera mainCamera;
    private Vector3 startPosition = new Vector3(65.75f, 186.9f, 111.4f);
    private float speed = 8; // 速度
    private bool isGameStart = false; // 当前游戏是否开始

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = this.GetComponent<Camera>();
        transform.position = startPosition;
        mainCamera.fieldOfView = 60;
    }

    // 游戏开始与结束
    public void GameStart()
    {
        isGameStart = true;
    }

    public void GameStop()
    {
        isGameStart = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGameStart)
        {
            return;
        }
        Move();
        ScaleView();
    }

    // 视野移动
    private void Move()
    {
        // 鼠标左键按下
        if (Input.GetMouseButton(0))
        {
            float mouseX = Input.GetAxis("Mouse X"); // 水平方向
            float mouseY = Input.GetAxis("Mouse Y"); // 竖直方向
            if (mouseX != 0 || mouseY != 0)
            {
                Vector3 target = transform.position + new Vector3(mouseX, 0, mouseY) * speed;
                transform.position = Vector3.Lerp(transform.position, target, speed * Time.deltaTime); // 插值运算
            }
        }
    }
    //视野缩放
    private void ScaleView()
    {
        float mouseWhell = Input.GetAxis("Mouse ScrollWheel"); 
        if (mouseWhell != 0)
        {
            mainCamera.fieldOfView += mouseWhell * speed; 
            if(mainCamera.fieldOfView <= 38)
            {
                mainCamera.fieldOfView = 38;
            }
            else if(mainCamera.fieldOfView >= 90)
            {
                mainCamera.fieldOfView = 90;
            }

        }
    }
}

