using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //プレイヤーのマウスを狙った方向にカメラを向くプログラム
    public Transform Parent; //追跡したいオプジェ
    public Transform Obj; //追跡するオプジェ
    public float Radius = 5f;//追跡の最大範囲
    Vector3 MousePos1, ScreenMouse, MouseOffset;
    public void Update()
    {
        MousePos1 = Input.mousePosition;
        ScreenMouse = Camera.main.ScreenToWorldPoint(new Vector3(MousePos1.x, MousePos1.y, Obj.position.z - Camera.main.transform.position.z));
        MouseOffset = ScreenMouse - Parent.position;

        var norm = MouseOffset.normalized;
        //追跡するプログラム
        Obj.position = new Vector3(norm.x * Radius + Parent.position.x, norm.y * Radius + Parent.position.y, Obj.position.z);

    }
}
