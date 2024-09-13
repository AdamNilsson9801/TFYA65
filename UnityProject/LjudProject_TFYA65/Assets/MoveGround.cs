using System;
using UnityEngine;

public class MoveGround : MonoBehaviour
{
    public float speedFactor = 5f;
    private Rigidbody rb;
    private Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //rb.velocity = transform.right * -speedFactor;
        transform.position += new Vector3(-GlobalSpeed.speed, 0 ,0) * Time.deltaTime;

    }
}
