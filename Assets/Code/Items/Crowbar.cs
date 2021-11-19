using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crowbar : MonoBehaviour
{
    public Text pickupText=null;
    public Text storyText=null;

    // Start is called before the first frame update
    void Start()
    {
        if (pickupText != null){
            pickupText.enabled = false;   
            storyText.enabled = false;             
        }

        Room currentRoom = Room.CurrentRoom;
        if (!currentRoom.IsCompleted && PublicVars.PaperCount < 3){
            gameObject.SetActive(false);
        }
    }
    private void OnTriggerStay(Collider other) {
        if (other.CompareTag("Player")){
            pickupText.enabled = true;
            if (Input.GetKeyDown("j")){
                pickupText.enabled = false;
                storyText.enabled = true; 
                PublicVars.Items.Add(new Item("Crowbar", true, ItemType.Crowbar));
                Destroy(storyText.gameObject, 5.0f);
                Destroy(gameObject);   
                return;
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")){
            pickupText.enabled = false;
        }
    }
}
