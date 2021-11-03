using UnityEngine;

public class Game : MonoBehaviour
{
    private void Awake()
    {
        return; // temporary
        Room start = new Room("test");
        Room test2 = new Room("test2");
        Room test3 = new Room("test3", new Room[] { start });
        Room boss = new Room("boss", new Room[] { start, test2, test3 });
    }
}
