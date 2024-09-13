using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;  // Singleton instance

    // Global variable
    public float globalVariable = 0f;

    void Awake()
    {
        // Ensure that there is only one instance of this class
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Keep this object alive across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
