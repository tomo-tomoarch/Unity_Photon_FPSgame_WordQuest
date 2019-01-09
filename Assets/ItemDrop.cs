using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    public GameObject[] items;

    private void OnDestroy()
    {
        Vector3 currentposition = this.transform.position;
        Quaternion currentquanternion = this.transform.rotation;

        int index = Random.Range(0, items.Length);

        if (index == 0)
        {
            PhotonNetwork.Instantiate("ItemA",
            currentposition,
            currentquanternion,
            0);
        }
        else if (index == 1)
        {
            PhotonNetwork.Instantiate("ItemA",
            currentposition,
            currentquanternion,
            0);
        }
        else if (index == 2)
        {
            PhotonNetwork.Instantiate("ItemA",
            currentposition,
            currentquanternion,
            0);
        }
    }
        // Start is called before the first frame update
        void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
