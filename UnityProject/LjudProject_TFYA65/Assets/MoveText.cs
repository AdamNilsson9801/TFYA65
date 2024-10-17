using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveText : MonoBehaviour
{

    void Update()
    {
        transform.position += new Vector3(-GlobalSpeed.speed, 0, 0) * Time.deltaTime;
    }
}
