using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weakPoint : MonoBehaviour
{
    public bool isDeath = false;
    // Start is called before the first frame update
    void Start()
    {

       isDeath = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Laser")
        {
            isDeath = true;
        }


    }
}
