using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FlashlightBot))]
public class PathEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

    }
    private FlashlightBot bot;
    private void OnSceneGUI()
    {
        bot = (FlashlightBot)target;

        if (bot.PathPoints.Length == 0) return;

        for (int i = 1; i < bot.PathPoints.Length; i++)
        {
            Handles.DrawBezier(bot.PathPoints[i - 1], bot.PathPoints[i],
                                bot.PathPoints[i - 1], bot.PathPoints[i], Color.white, null, 5f);
        }

        bot.PathPoints[0] = bot.transform.position;

        for (int i = 1; i < bot.PathPoints.Length; i++)
        {
            bot.PathPoints[i] = Handles.PositionHandle(
                new Vector3(bot.PathPoints[i].x, bot.transform.position.y, bot.PathPoints[i].z), bot.transform.rotation
            );
        }
    }
}