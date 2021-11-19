using System.Collections.Generic;

public static class PublicVars
{
    public static Game Game;
    public static TransitionManager TransitionManager;

    public static int LastEnteredDoorIndex = -1;

    // ===== Bot consts/vars =====
    /// <value>The minimum magnitude of speed to be considered moving</value>
    public const float MINIMUM_MOVEMENT_SPEED = 0.2f;
    /// <value>The step size for ray cast detection in degrees</value>
    public const float ALERT_DETECTION_STEP = 7f;
    /// <value>The minimum distance before the enemy stops moving towards the player</value>
    public const float MINIMUM_CHASE_DISTANCE = 1.2f;
    public const int ENEMY_DAMAGE = 10;

    // ===== Player consts/vars =====
    public const int MAX_HEALTH = 100;

    public static int PaperCount = 0;
    public static int Health = MAX_HEALTH;
    public static List<Item> Items = new List<Item>();

    // ===== Game consts =====
    public const float MUSIC_TRANSITION_TIME = 1f;
    public const float OVERLAY_FADING_TIME = 1.5f;
    public const float DEATH_FADEOUT_TIME = 5f;

    /// <summary>Reset all the public vars</summary>
    public static void Reset()
    {
        LastEnteredDoorIndex = -1;

        PaperCount = 0;
        Health = MAX_HEALTH;
        Items = new List<Item>();
    }
}
