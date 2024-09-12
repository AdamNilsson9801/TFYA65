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

    }

    public bool OutOfSight() //Look if ground is behind camera
    {
        return (transform.position.x < -100f);
    }
}
