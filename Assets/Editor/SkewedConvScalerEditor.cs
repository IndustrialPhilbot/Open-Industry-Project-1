#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;

[CustomEditor(typeof(SkewedConvScaler))]
public class SkewedConvScalerEditor : Editor
{
    void OnPreSceneGUI()
    {
        var player = (SkewedConvScaler)target;
        player.RemakeObject();
    }
}
#endif