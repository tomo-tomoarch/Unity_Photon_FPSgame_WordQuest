﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropKi : MonoBehaviour
{

    private void OnDestroy()
    {
        Vector3 currentposition = this.transform.position;
        Quaternion currentquanternion = this.transform.rotation;

        int index = Random.Range(0, 10);

        if (index == 0)
        {
            PhotonNetwork.Instantiate("ItemKi",
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