using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLifeSpan : MonoBehaviour
{
    public int timeTilDeath = 2;
    void Start()
    {
        Destroy(gameObject, timeTilDeath);
    }
}
