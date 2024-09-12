using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.UIElements;

public class InfiniteTerrain : MonoBehaviour
{
    public GameObject ground;
    [Range(1, 5)]
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

            //Set velocity
            groundInstance.GetComponent<Rigidbody>().velocity = Vector3.right * -groundSpeed;

            //Add to list
            groundList.Add(groundInstance);
        }

    }
    // Update is called once per frame
    void Update()
    {
        //for(int i = 0; i < groundSectionAmount; i++)
        //{
        //    if (groundList[i].GetComponent<MoveGround>().OutOfSight())
        //    {
        //        //Get last ground section
        //        GameObject lastGroundSection = groundList[(i - 1 + groundSectionAmount) % groundSectionAmount];

        //        //Set new position
        //        groundList[i].GetComponent<Rigidbody>().position = lastGroundSection.transform.position + new Vector3(xLength, 0, 0);


        //    }

        //    //else do nothing
        //}
    }

    public void UpdatePosition(GameObject obj)
    {
        int index = groundList.IndexOf(obj);

        if(index != -1)
        {
            //Get last ground section
            GameObject lastGroundSection = groundList[(index - 1 + groundSectionAmount) % groundSectionAmount];

            //Set new position
            groundList[index].GetComponent<Rigidbody>().position = lastGroundSection.transform.position + new Vector3(xLength, 0, 0);
        }
    }
}
