using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : Photon.MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("inventory").GetComponent<UnityEngine.UI.Image>().enabled = false;//panelオフ
    }

    // Update is called once per frame
    void Update()
    {

        // 左クリックされた瞬間にif文の中を実行
        if (Input.GetMouseButtonDown(1))
        {
            if (GameObject.Find("inventory").GetComponent<UnityEngine.UI.Image>().enabled == false)
            {
                GameObject.Find("inventory").GetComponent<UnityEngine.UI.Image>().enabled = true;//panelオン
            }
            else
            {
                GameObject.Find("inventory").GetComponent<UnityEngine.UI.Image>().enabled = false;//panelオン
            }
        }
    }

}
