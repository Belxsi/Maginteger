using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(UITextNoiseScriptControl)), CanEditMultipleObjects]

public class UITNG : Editor
{
    public void OnSceneGUI()
    {
        UITextNoiseScriptControl tt= (UITextNoiseScriptControl)target;
        if(!UnityEngine.Application.isPlaying)
        tt.Update();

    }
    
      
      
        
    


}

