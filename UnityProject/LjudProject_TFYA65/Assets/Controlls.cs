using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Controlls : MonoBehaviour
{
    public SoundControll soundControllScript;
    public TextMeshProUGUI text;
    public GameObject terrain;
    public GameObject lanePositions;
    public float carSpeed = 100f;
    public GameObject GameManager;
    public float deadSpaceScale = 2f;

    private Vector3 targetPos;
    private bool isMoving = false;
    private int lanePos = 1;
    private float currentPitch = 0;



    // Start is called before the first frame update
    void Start()
    {
        transform.position = lanePositions.transform.GetChild(1).position;
    }

    // Update is called once per frame
    void Update()
    {

        float currentTime = Time.time;

        if (soundControllScript)
        {
            if (soundControllScript.GetLoudness() > soundControllScript.loudnessThreshold)
            {

                float pitch = soundControllScript.GetPitch();


                if (pitch > 69f && pitch < 1000f)
                {
                    currentPitch = pitch;
                }
            }
            else
            {
                if (currentPitch > 1f)
                {
                    text.text = Mathf.Round(currentPitch).ToString() + " Hz";
                    ChangeLane(currentPitch);
                    currentPitch = 0f;
                }

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

        //Move car with keys (<-- A, D -->)
        MoveCarWithKeys();
    }

    private void MoveCarWithKeys()
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
        float dist = Mathf.Abs(GlobalSpeed.rightPitch - GlobalSpeed.leftPitch);

        float lowBar = GlobalSpeed.leftPitch + (dist / 2f) - deadSpaceScale;

        float highBar = GlobalSpeed.rightPitch - (dist / 2f) + deadSpaceScale;

        if (freq < lowBar) //Turn left
        {

            int currentLane = lanePos;

            if (isMovePossible(currentLane, currentLane - 1))
            {
                isMoving = true;
                targetPos = lanePositions.transform.GetChild(currentLane - 1).position;
                lanePos = currentLane - 1;
            }

        }
        else if (freq > highBar)//Turn right
        {
            int currentLane = lanePos;

            if (isMovePossible(currentLane, currentLane + 1))
            {
                isMoving = true;
                targetPos = lanePositions.transform.GetChild(currentLane + 1).position;
                lanePos = currentLane + 1;
            }

        }
    }
}
