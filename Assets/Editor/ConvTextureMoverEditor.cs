#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(Conveyor))]
public class ConveyorTextureMoverEditor : Editor
{
    void OnSceneGUI()
    {
        //DrawDefaultInspector();
        var player = (Conveyor)target;
        player.GetComponentInChildren<ConvTextureMover>().RemakeMesh();
    }
}
#endif  