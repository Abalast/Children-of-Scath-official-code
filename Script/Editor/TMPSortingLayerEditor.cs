using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;

[CustomEditor(typeof(TMPSortingLayer))]
public class TMPSortingLayerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        TMPSortingLayer script = (TMPSortingLayer)target;

        script.sortingLayer = EditorGUILayout.IntField("Sorting layer", script.sortingLayer);
        TextMeshProUGUI t = script.gameObject.GetComponent<TextMeshProUGUI>();
        if(t != null)
        {
            t.canvas.sortingOrder = script.sortingLayer;
        }
    }
}
