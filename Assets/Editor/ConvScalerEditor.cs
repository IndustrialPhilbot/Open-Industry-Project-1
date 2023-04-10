#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;

[CustomEditor(typeof(ConvScaler))]
public class ConvScalerEditor : Editor
{
    void OnPreSceneGUI()
    {
        var player = (ConvScaler)target;
        player.RemakeObject();
    }
}
#endif