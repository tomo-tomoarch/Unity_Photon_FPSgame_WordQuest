using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] Transform[] spawnPoints;

    GameObject enemy;
    public int enemyCount = 0;



    void OnJoinedRoom()
    {
        StartSpawnProcess(3f);
    }

    public void StartSpawnProcess(float respawnTime)
    {
        StartCoroutine("SpawnPlayer", respawnTime);
    }

    IEnumerator SpawnPlayer(float respawnTime)
    {
        yield return new WaitForSeconds(respawnTime);

        int index = Random.Range(0, spawnPoints.Length);
        enemy = PhotonNetwork.Instantiate("Enemy",
            spawnPoints[index].position,
            spawnPoints[index].rotation,
            0);
       
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
