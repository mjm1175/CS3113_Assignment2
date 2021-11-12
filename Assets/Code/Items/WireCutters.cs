using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireCutters : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (PublicVars.kill_count != 0){
            gameObject.active = false;
        }
    }
}
