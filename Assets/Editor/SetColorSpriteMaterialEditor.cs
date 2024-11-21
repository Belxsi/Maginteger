using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(SetColorSpriteMaterial)), CanEditMultipleObjects]

public class SetColorSpriteMaterialEditor : Editor
{
    public void Awake()
    {
        SetColorSpriteMaterial tt = (SetColorSpriteMaterial)target;
        if (!UnityEngine.Application.isPlaying)
            tt.Awake();
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

    
    
        SetColorSpriteMaterial tt = (SetColorSpriteMaterial)target;
        if (!UnityEngine.Application.isPlaying)
            tt.FixedUpdate();

    }







}
