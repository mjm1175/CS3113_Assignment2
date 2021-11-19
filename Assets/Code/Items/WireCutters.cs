using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WireCutters : MonoBehaviour, ICollectable
{
    public Text pickupText;
    public Text storyText;

    // Start is called before the first frame update
    void Start()
    {
        pickupText.enabled = false;
        storyText.enabled = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            pickupText.enabled = true;
            if (Input.GetKeyDown("j"))
            {
                pickupText.enabled = false;
                storyText.enabled = true;
                PublicVars.Items.Add(new Item("WireCutters", true, ItemType.WireCutter));
                Destroy(gameObject);
                return;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            pickupText.enabled = false;
        }
    }
}
