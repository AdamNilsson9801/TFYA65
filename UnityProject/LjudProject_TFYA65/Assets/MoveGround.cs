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
        startPos = transform.position;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = transform.right * -speedFactor;

        if (OutOfSight())
        {
            //Change pos to => infront
            Debug.Log("OUT_OF_SIGHT");
            //rb.position = startPos;

        }
    }

    public bool OutOfSight() //Look if ground is behind camera
    {
        Vector3 currentPos = transform.position;
        if(currentPos.x < -100f)
        {
            return true; //If behind, return true
        }

        return false; //If ground is in front, return false
    }
}
