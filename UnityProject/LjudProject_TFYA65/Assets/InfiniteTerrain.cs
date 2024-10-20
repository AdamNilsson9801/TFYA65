using System.Collections.Generic;
using UnityEngine;


public class InfiniteTerrain : MonoBehaviour
{
    public GameObject ground;
    [Range(1, 10)]
    public int groundSectionAmount = 1;
    public float groundSpeed = 10;
    private List<GameObject> groundList = new List<GameObject>();
    public float xLength;
    // Start is called before the first frame update
    void Start()
    {
        xLength = ground.GetComponent<Renderer>().bounds.size.x;
        //Debug.Log(xLength);

        //Create groundList
        for (int i = 0; i < groundSectionAmount; ++i)
        {
            //Create instance of the ground section and set start position
            GameObject groundInstance = Instantiate(ground, new Vector3(i * xLength, 0, 0), Quaternion.Euler(-90, 0, 0));

            //Add to list
            groundList.Add(groundInstance);
        }

    }


    public int GetIndexOfGround(GameObject obj)
    {
        return groundList.IndexOf(obj);
    }

    public void MoveGroundSection(int index)
    {
        //Get last ground section
        GameObject lastGroundSection = groundList[(index - 1 + groundSectionAmount) % groundSectionAmount];

        //Set new position
        groundList[index].transform.position = lastGroundSection.transform.position + new Vector3(xLength, 0, 0);

        //Reset obstacle(s)
        groundList[index].transform.GetChild(1).gameObject.GetComponent<SpawnObstaclesInRow>().DeleteObstacles();
        groundList[index].transform.GetChild(1).gameObject.GetComponent<SpawnObstaclesInRow>().SpawnInRow();
    }

}
