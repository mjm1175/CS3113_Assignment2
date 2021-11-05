using UnityEngine;

public class Key : MonoBehaviour
{
    public Door Door;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && Door.locked)
        {
            Door.locked = false;
            Destroy(gameObject);
        }
    }
}
