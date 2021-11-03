using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public string levelToLoad;
    public bool locked = true;
    public int doorCode = 0;

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")){
            if (!locked){
                SceneManager.LoadScene(levelToLoad);
            } else if (PublicVars.hasKey[doorCode]){
                PublicVars.hasKey[doorCode] = false;
                SceneManager.LoadScene(levelToLoad);
            }
        }
    }
}
