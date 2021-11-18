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
        PublicVars.paper_count = 0;
        PublicVars.kill_count = 0;
        PublicVars.got_key = false;
        PublicVars.health = 100;

        SceneManager.LoadScene("Menu");
    }
}
