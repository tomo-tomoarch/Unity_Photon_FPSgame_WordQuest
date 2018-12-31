using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Restart : Photon.MonoBehaviour
{   
    //public Text enemyText;

    GameController gameController;
    public GameObject button;
    NetworkManager networkManager;
    // Start is called before the first frame update
    public void OnClick()
    {
        networkManager = GameObject.Find("Networkmanager").GetComponent<NetworkManager>();
        //networkManager = GetComponent<NetworkManager>();
        networkManager.StartSpawnProcess(0f);
        GameObject.Find("Panel").GetComponent<UnityEngine.UI.Image>().enabled = false;
        button.SetActive(false);
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        gameController.enemyCount = 0;

        //if (photonView.isMine)
       // {
       //     gameController = GameObject.Find("GameController").GetComponent<GameController>();
       //     enemyText.text = gameController.enemyCount.ToString() + "enemy killed";
      //  }
    }

}
