using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private void Start()
    {

    }
    public void StartGame()
    {
        SceneManager.LoadScene("Game");

    }

    public void GameOver()
    {
        SceneManager.LoadScene("Menu");
    }
}
