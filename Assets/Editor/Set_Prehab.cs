using UnityEngine;
using UnityEditor;

public class Set_Prefab : EditorWindow {

  private Vector3 pos;
  private GameObject prefab;

  [MenuItem("GameObject/SetPrefab")]
  static void init()
    {
        EditorWindow.GetWindow<Set_Prefab>("Set_Prefab");
    }

    private void OnGUI()
    {
        prefab = EditorGUILayout.ObjectField("prefab", prefab, typeof(GameObject), true) as GameObject;
        pos = EditorGUILayout.Vector3Field("postion", pos);

        if (GUILayout.Button("set")) 
        {
            GameObject obj = Instantiate(prefab);
            obj.transform.position = pos;
        }
    }
}