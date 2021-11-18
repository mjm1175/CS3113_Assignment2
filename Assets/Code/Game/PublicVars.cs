using System.Collections.Generic;

public static class PublicVars
{
    public static Game Game;

    // Player vars
    public static int paper_count = 0;
    public static bool got_key = false;
    public static int health = 100;
    public static List<Item> Items = new List<Item>();

    public static int LastEnteredDoorIndex;

    /// <value>The minimum magnitude of speed to be considered moving</value>
    public const float MINIMUM_MOVEMENT_SPEED = 0.2f;

    /// <value>The step size for ray cast detection in degrees</value>
    public const float ALERT_DETECTION_STEP = 7f;

    /// <value>The minimum distance before the enemy stops moving towards the player</value>
    public const float MINIMUM_CHASE_DISTANCE = 1;
}
