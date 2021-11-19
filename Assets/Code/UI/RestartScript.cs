using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartScript : MonoBehaviour
{
    public void Restart()
    {
        /*pauseMenu.SetActive(false);
        isPaused = false;
        Time.timeScale = 1f;*/

        // cleanup
        PublicVars.Game.InitializeGame();

        SceneManager.LoadScene("Menu");
    }
}
