using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;


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

        Handles.color = new Color(255, 255, 255, 0.5f);

        Handles.DrawSolidArc(bot.transform.position, Vector3.up, Quaternion.AngleAxis(-bot.FrontAlertAngle / 2, Vector3.up) * bot.transform.forward, bot.FrontAlertAngle, bot.FrontAlertDistance);

        if (bot.PathPoints.Length == 0) return;

        for (int i = 1; i < bot.PathPoints.Length; i++)
        {
            Handles.DrawBezier(bot.PathPoints[i - 1], bot.PathPoints[i],
                                bot.PathPoints[i - 1], bot.PathPoints[i], Color.white, null, 5f);
        }

        for (int i = 0; i < bot.PathPoints.Length; i++)
        {
            bot.PathPoints[i] = Handles.PositionHandle(
                new Vector3(bot.PathPoints[i].x, bot.transform.position.y, bot.PathPoints[i].z), bot.transform.rotation
            );
        }
    }
}