using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartScript : MonoBehaviour
{
    public void Restart()
    {
        // cleanup
        PublicVars.Game.InitializeGame();

        SceneManager.LoadScene("Menu");
    }
}
