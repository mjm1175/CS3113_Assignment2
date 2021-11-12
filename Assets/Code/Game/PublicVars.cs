using System.Collections.Generic;

public static class PublicVars
{
    public static Game Game;

    // Player vars
    public static int paper_count = 0;
    public static int kill_count = 1;
    public static bool got_key = false;
    public static List<Item> Items = new List<Item>();

    /// <value>The minimum magnitude of speed to be considered moving</value>
    public static float MINIMUM_MOVEMENT_SPEED = 0.2f;
}
