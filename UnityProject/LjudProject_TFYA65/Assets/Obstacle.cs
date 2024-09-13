using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //GetComponent<Rigidbody>().velocity = Vector3.right * -GlobalSpeed.speed;

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(-GlobalSpeed.speed, 0, 0) * Time.deltaTime;
    }
}
