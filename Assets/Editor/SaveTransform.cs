// #if UNITY_EDITOR
// using UnityEngine;
// using UnityEditor;
// using System.Reflection;
// [SerializeField]
// public class SaveTransform
// {
//     //参照型を保存するとインスタンスIDのみが保存され、再生前の状態に戻るので値型を保存する
//     [SerializeField] private Vector3 position;
//     [SerializeField] private Quaternion rotation;
//     [SerializeField] private Vector3 scale;
//     public Transform GetValue(Transform t)
//     {
//         t.position = position;
//         t.rotation = rotation;
//         t.localScale = scale;
//         return t;
//     }

//     public void SetValue(Transform t)
//     {
//         position = t.position;
//         rotation = t.rotation;
//         scale = t.localScale;
//     }
// }

// //再生中に変更されたTransformの値を再生終了後に反映させるスクリプト
// [CanEditMultipleObjects]
// [CustomEditor(typeof(Transform), true)]

// public class InspectorTransform : Editor
// {
//     private Editor editor;
//     private bool set;

//     private void OnEnable()
//     {
//         Transform transform = target as Transform;
//         System.Type t = typeof(UnityEditor.EditorApplication).Assembly.GetType("UnityEditor.TransformInspector");
//         editor = Editor.CreateEditor(targets, t);
//     }

//     private void OnDisable()
//     {
//         MethodInfo disableMethod = editor.GetType().GetMethod("OnDisable", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
//         if (disableMethod != null) disableMethod.Invoke(editor, null);
//         DestroyImmediate(editor);
//     }

//     public override void OnInspectorGUI()
//     {
        
//         editor.OnInspectorGUI();
//         if (EditorApplication.isPlaying || EditorApplication.isPaused)
//         {
//             if (GUILayout.Button("再生中の状態を保存"))
//             {
//                 // 現在選択されている複数のオブジェクトのTransformの値を保存
//                 foreach (var t in targets)
//                 {
//                     SaveTransform s = new SaveTransform();
//                     s.SetValue((Transform)t);
//                     string json = JsonUtility.ToJson(s);
//                     Debug.Log(json);
//                     EditorPrefs.SetString("Save Param " + ((Transform)t).GetInstanceID().ToString(), json);
//                 }
//                 if (!set)
//                 {
//                     EditorApplication.playModeStateChanged += OnChangedPlayMode; set = true;
//                 }
//             }
//         }
//     }

//     private void OnChangedPlayMode(PlayModeStateChange state)
//     {
//         //Unityの再生が終了した
//         if (state == PlayModeStateChange.EnteredEditMode)
//         {
//             //選択されている複数のオブジェクトに対して、「Save Param」で保存した値を適用する処理を追加します。
//             foreach (var t in targets)
//             {
//                 Transform transform = (Transform)t;
//                 string key = "Save Param " + transform.GetInstanceID().ToString();
//                 string json = EditorPrefs.GetString(key);
//                 SaveTransform savedTransform = JsonUtility.FromJson<SaveTransform>(json);
//                 EditorPrefs.DeleteKey(key);
//                 transform = savedTransform.GetValue(transform);
//                 EditorUtility.SetDirty(t);
//             }
//             EditorApplication.playModeStateChanged -= OnChangedPlayMode;
//         }
//     }
// }
// #endif
