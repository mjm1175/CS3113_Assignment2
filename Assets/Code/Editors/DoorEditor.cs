using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Door))]
public class DoorEditor : Editor
{
    private void OnSceneGUI()
    {
        Door door = (Door)target;

        EditorGUI.BeginChangeCheck();

        Vector3 position = Handles.PositionHandle(door.transform.position + door.SpawnOffset, Quaternion.identity);
        Handles.Label(position, "SpawnPosition");

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(door, "Updated spawn position");
            door.SpawnOffset = position - door.transform.position;
        }
    }
}