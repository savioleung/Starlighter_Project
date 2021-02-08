using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laser : MonoBehaviour
{
    //
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //壁当たったら消える
        if (collision.tag == "Ground" ||  collision.tag == "Map Object" || collision.tag == "Shield") //(collision.gameObject.tag != "EnemyLaser" && collision.gameObject.tag!="LaserCannon")//|| collision.gameObject.name == "Shield"
        {
            this.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            this.GetComponent<Rigidbody2D>().isKinematic = true;
            //Destroy(gameObject);
        }
    }

}
