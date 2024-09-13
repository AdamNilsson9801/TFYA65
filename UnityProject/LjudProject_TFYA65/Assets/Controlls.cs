using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Controlls : MonoBehaviour
{
    public GameObject terrain;
    public GameObject lanePositions;
    public float carSpeed = 10f;
    public GameObject GameManager;

    private Vector3 targetPos;
    private bool isMoving = false;


    // Start is called before the first frame update
    void Start()
    {
        transform.position = lanePositions.transform.GetChild(1).position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            targetPos = lanePositions.transform.GetChild(0).position;
            isMoving = true;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            targetPos = lanePositions.transform.GetChild(1).position;
            isMoving = true;

        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            targetPos = lanePositions.transform.GetChild(2).position;
            isMoving = true;

        }

        if (isMoving)
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, carSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPos) < 0.1f)
            {
                isMoving = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("GroundSectionTrigger"))
        {
            int GroundIndex = terrain.GetComponent<InfiniteTerrain>().GetIndexOfGround(other.transform.parent.gameObject);
           
            terrain.GetComponent<InfiniteTerrain>().MoveGroundSection(GroundIndex);

        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("ObstacleCollision"))
        {
            Debug.Log("COLLISION!!");
            GameManager.GetComponent<GameController>().GameOver();
        }
    }
}
