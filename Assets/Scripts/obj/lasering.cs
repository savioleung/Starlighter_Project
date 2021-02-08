using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lasering : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyLaser;
    float laserSpeed=10.0f,laserReload;


    // Update is called once per frame
    void Update()
    {
        laserReload += 1 * Time.deltaTime;
        if (laserReload >= 0.3f)
        {
            GameObject laser = Instantiate(enemyLaser, transform.position, Quaternion.identity);

            // 弾に速度を与える
            laser.GetComponent<Rigidbody2D>().velocity = -Vector3.up * laserSpeed;

            Destroy(laser, 2);
            laserReload = 0;
        }
    }
}
