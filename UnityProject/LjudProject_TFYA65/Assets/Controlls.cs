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
<<<<<<< Updated upstream
    public float deadSpaceScale = 5f;
=======
    
>>>>>>> Stashed changes

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
<<<<<<< Updated upstream
=======

        float currentTime = Time.time;


        if (soundControllScript && (currentTime - checkTime) >= 0.2f)
        {
            float pitch = soundControllScript.GetPitch();


            if (pitch > 69f && pitch < 1000f)
            {
                //if (pitchArrayIndex < pitchArrayLength)
                //{
                //    pitchArray[pitchArrayIndex] = pitch;
                //    pitchArrayIndex++;
                //}
                //else
                //{
                 //   float newPitch = pitchArray.Max();

                    text.text = Mathf.Round(pitch).ToString() + " Hz";
                    ChangeLane(pitch);
                    checkTime = currentTime;
                //    pitchArrayIndex = 0;
               // }



            }
            //else
            //{
            //    pitchArrayIndex = 0;
            //}

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
        //MoveCarWithKeys()
    }

    private void MoveCarWithKeys()
    {
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
            float dist = Mathf.Abs(GlobalSpeed.rightPitch - GlobalSpeed.leftPitch);
            
            float lowBar = GlobalSpeed.leftPitch + (dist / 2f) - deadSpaceScale;
            
            float highBar = GlobalSpeed.rightPitch - (dist / 2f) + deadSpaceScale;

        if(freq < lowBar) //Turn left
=======
        float dist = Mathf.Abs(GlobalSpeed.rightPitch - GlobalSpeed.leftPitch);   // 30Hz     10Hz 
        
        float deadSpaceScale = dist/10f;

        float lowBar = GlobalSpeed.leftPitch + (dist / 2f) - deadSpaceScale;    // 10 + 20/2 - 2 = 18

        float highBar = GlobalSpeed.rightPitch - (dist / 2f) + deadSpaceScale;  // 30 - 20/2 +2 = 22
        
        if (freq < lowBar) //Turn left
>>>>>>> Stashed changes
        {

            int currentLane = lanePos;

            if (isMovePossible(currentLane, currentLane - 1))
            {
                targetPos = lanePositions.transform.GetChild(currentLane - 1).position;
                isMoving = true;
                lanePos = currentLane - 1;
            }

        }
        else if(freq > highBar)//Turn right
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
