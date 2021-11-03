using UnityEngine;

/// <summary>Put this script on the room prefab</summary>
public class RoomManager : MonoBehaviour
{
    public Door[] Doors { get; private set; }

    private void Start()
    {
        Doors = GetComponents<Door>();
    }
}
