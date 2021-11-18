using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WireCutters : MonoBehaviour
{
    public Text pickupText;
    public Text storyText;      // for this one it should be something about pacifist

    // Start is called before the first frame update
    void Start()
    {
        pickupText.enabled = false;   
        storyText.enabled = false; 

        if (PublicVars.kill_count != 0)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.CompareTag("Player")){
            pickupText.enabled = true;
            if (Input.GetKeyDown("j")){
                pickupText.enabled = false;
                storyText.enabled = true; 
                Destroy(gameObject);   
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")){
            pickupText.enabled = false;
        }
    }
}
