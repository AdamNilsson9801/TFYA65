using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObstaclesInRow : MonoBehaviour
{
    public GameObject obstacle;
    private List<GameObject> obstacles;
    public void Start()
    {
        obstacles = new List<GameObject>();
    }
    public void SpawnInRow()
    {
        int spawnPoint1 = Random.Range(0, 3);
        int spawnPoint2 = Random.Range(0, 3);

        if (spawnPoint1 == spawnPoint2)
        {
            //Spawn only the first obstacle
            Vector3 spawnPos = transform.GetChild(spawnPoint1).position;

            obstacles.Add(Instantiate(obstacle, spawnPos, Quaternion.identity));

        }
        else
        {
            //Spawn two obstacles
            Vector3 spawnPos1 = transform.GetChild(spawnPoint1).position;
            Vector3 spawnPos2 = transform.GetChild(spawnPoint2).position;

            obstacles.Add(Instantiate(obstacle, spawnPos1, Quaternion.identity));
            obstacles.Add(Instantiate(obstacle, spawnPos2, Quaternion.identity));
        }
    }

    public void DeleteObstacles()
    {
        obstacles.Clear();
    }
}
