using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickupPaper : MonoBehaviour
{
    public Text pickupText;
    public Text storyText;

    private void Start()
    {
        pickupText.enabled = false;
        storyText.enabled = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            pickupText.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            pickupText.enabled = false;
        }
    }

    private void Update()
    {
        if (pickupText.enabled && Input.GetKeyDown("j"))
        {
            PublicVars.TransitionManager.PickupPaperSound.Play();
            pickupText.enabled = false;
            storyText.enabled = true;
            PublicVars.PaperCount++;
            Destroy(gameObject);
        }

    }
}
