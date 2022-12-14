using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LoginManager))]
public class LoginManagerEditorScript : Editor
{
   public override void OnInspectorGUI()
   {
      EditorGUILayout.HelpBox("This script is responsible for connecting to the Photon servers." , MessageType.Info);

      LoginManager loginManager = (LoginManager) target;
      
      if (GUILayout.Button("Connect Anonymously"))
      {
         loginManager.ConnectToPhotonServer();
      }
   }
}
