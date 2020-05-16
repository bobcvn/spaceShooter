using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool _isGameOver;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && (_isGameOver))
        {
            SceneManager.LoadScene(1); // Current game scene
        }
        if (Input.GetKeyDown(KeyCode.Escape) && (_isGameOver))
        {
            Application.Quit();
        }
    }

    public void GameOver()
    {
        _isGameOver = true;
    }
}
