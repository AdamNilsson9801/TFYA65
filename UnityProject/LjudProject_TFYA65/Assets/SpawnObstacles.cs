using UnityEngine;

public class SpawnObstacles : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Spawn();
    }

    public void Spawn()
    {
        //Get SpawnRow1
        //GameObject spawnRow1 = transform.GetChild(1).gameObject;

        //Get SpawnRow2
        GameObject spawnRow2 = transform.GetChild(2).gameObject;


        //Spawn obstacle(s) in row 1
        //spawnRow1.GetComponent<SpawnObstaclesInRow>().SpawnInRow();
        
        //Spawn obstacle(s) in row 2
        spawnRow2.GetComponent<SpawnObstaclesInRow>().SpawnInRow();
    }
}
