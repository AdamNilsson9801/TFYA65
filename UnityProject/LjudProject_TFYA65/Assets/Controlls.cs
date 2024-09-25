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
    private int lanePos = 1;


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
            int currentLane = lanePos;
            Debug.Log("currentLane: " + currentLane);
            if (isMovePossible(currentLane, currentLane - 1))
            {
                targetPos = lanePositions.transform.GetChild(currentLane - 1).position;
                isMoving = true;
                lanePos = currentLane - 1;
            }
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            int currentLane = lanePos;
            if (isMovePossible(currentLane, currentLane + 1))
            {
                targetPos = lanePositions.transform.GetChild(currentLane + 1).position;
                isMoving = true;
                lanePos = currentLane + 1;
            }

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

    private bool isMovePossible(int current, int target)
    {
        if (current == 0 || current == 2)
        {
            if (target == 1)
            {
                return true;
            }
            else return false;

        }
        else if (current == 1)
        {
            if (target == 0 || target == 2)
            {
                return true;
            }
            else return false;

        }
        return false;
    }

    public void ChangeLane(float freq)
    {
        if(freq < 200) //Turn left
        {
            int currentLane = lanePos;

            if (isMovePossible(currentLane, currentLane - 1))
            {
                targetPos = lanePositions.transform.GetChild(currentLane - 1).position;
                isMoving = true;
                lanePos = currentLane - 1;
            }

        }
        else //Turn right
        {
            int currentLane = lanePos;

            if (isMovePossible(currentLane, currentLane + 1))
            {
                targetPos = lanePositions.transform.GetChild(currentLane + 1).position;
                isMoving = true;
                lanePos = currentLane + 1;
            }

        }
    }
}
