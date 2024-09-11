using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteTerrain : MonoBehaviour
{
    public List<GameObject> groundList;
    // Start is called before the first frame update
    void Start()
    {
        float xLength = groundList[0].GetComponent<Renderer>().bounds.size.x;
        Debug.Log(xLength);

        //Set position for every groud in list
        for (int i = 0; i < groundList.Count; i++)
        {
            groundList[i].GetComponent<Rigidbody>().position = new Vector3(i * xLength, 0, 0);
            groundList[i].GetComponent<Rigidbody>().velocity = Vector3.left * -10f;

        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject g in groundList)
        {
            if (g.GetComponent<MoveGround>().OutOfSight())
            {
                //Move in-front
            }

            //else do nothing
        }
    }

}
