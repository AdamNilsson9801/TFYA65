using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObstaclesInRow : MonoBehaviour
{
    public List<GameObject> obstacleList;

    private List<GameObject> obstacles;

    public void Start()
    {
        obstacles = new List<GameObject>();
        Debug.Log(obstacleList.Count);
    }
    public void SpawnInRow()
    {
        int spawnPoint1 = Random.Range(0, 3);
        int spawnPoint2 = Random.Range(0, 3);

        if (spawnPoint1 == spawnPoint2)
        {
            //Spawn only the first obstacle
            Vector3 spawnPos = transform.GetChild(spawnPoint1).position;

            //Pick a random obstacles from the List
            int randomInt = Random.Range(0, obstacleList.Count);

            //obstacles.Add(Instantiate(obstacleList[randomInt], spawnPos, Quaternion.Euler(0, -90, 0)));

        }
        else
        {
            //Spawn two obstacles
            Vector3 spawnPos1 = transform.GetChild(spawnPoint1).position;
            Vector3 spawnPos2 = transform.GetChild(spawnPoint2).position;

            //Pick a random obstacles from the List
            int randomInt1 = Random.Range(0, obstacleList.Count);
            int randomInt2 = Random.Range(0, obstacleList.Count);

            //obstacles.Add(Instantiate(obstacleList[randomInt1], spawnPos1, Quaternion.Euler(0, -90, 0)));
            //obstacles.Add(Instantiate(obstacleList[randomInt2], spawnPos2, Quaternion.Euler(0, -90, 0)));

        }
    }

    public void DeleteObstacles()
    {
        obstacles.Clear();
    }
}
