using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    Player player;
    [SerializeField]
    private GameObject deathUI,winUI;
    // Start is called before the first frame update
    void Start()
    {
        winUI.SetActive(false);
        deathUI.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.isDeath)
        {
            deathUI.SetActive(true);
            player.gameObject.SetActive(false);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(0);
            }
        }
        else if (player.isWin)
        {
            winUI.SetActive(true);
            player.gameObject.SetActive(false);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(0);
            }
        }
    }
}
