using System.Reflection;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

// ScriptableObjectを継承し、インスタンスを保持するクラス
[CreateAssetMenu]
public class LockInspectorParameter : ScriptableObject
{
    // LockInspectorで設定されたGameObjectのリスト
    public List<GameObject> settingsObjectList = new List<GameObject>();
    // インスペクタウィンドウのリスト
    public List<EditorWindow> windowList = new List<EditorWindow>();
}

// インスペクタウィンドウを複製し、ロックするクラス
public class LockInspector
{
    private const string SETTINGS_PATH = "Assets/LockInspectorParameter.asset";

    private static LockInspectorParameter settings;

    // メニューから呼び出す関数 : インスペクタウィンドウを複製し、ロックし、リストに追加
    [MenuItem("Window/LockInspector/Tab %l")]
    public static void ShowInspectorWindow()
    {
        if (settings == null)
        {
            settings = AssetDatabase.LoadAssetAtPath<LockInspectorParameter>(SETTINGS_PATH);
            if (settings == null)
            {
                settings = ScriptableObject.CreateInstance<LockInspectorParameter>();
                AssetDatabase.CreateAsset(settings, SETTINGS_PATH);
            }
        }

        // 対象のGameObjectリストを取得
        List<GameObject> targetList = new List<GameObject>();

        // 選択中のオブジェクト
        GameObject selectedObject = Selection.activeGameObject;

        targetList.Add(selectedObject);

        // それぞれのGameObjectについて、インスペクタウィンドウを複製し、ロックし、リストに追加
        foreach (GameObject target in targetList)
        {
            if (target == null)
            {
                continue;
            }

            // インスペクタウィンドウのタイプ
            var inspectorType = typeof(Editor).Assembly.GetType("UnityEditor.InspectorWindow");
            // インスタンスを作成
            var inspectorInstance = ScriptableObject.CreateInstance(inspectorType) as EditorWindow;
            inspectorInstance.ShowUtility();

            // 選択中のオブ
            Selection.activeObject = target;
            inspectorInstance.Focus();

            // isLockedプロパティを取得し、値をtrueに設定
            var isLocked = inspectorType.GetProperty("isLocked", BindingFlags.Instance | BindingFlags.Public);
            isLocked.GetSetMethod().Invoke(inspectorInstance, new object[] { true });

            // InspectorWindowの位置とサイズを設定
            var position = inspectorInstance.position;
            position.x += 50f;
            position.y += -1000f;
            position.x += 400f * settings.windowList.Count;
            position.y += 0f * settings.windowList.Count;
            inspectorInstance.position = position;

            // リストに追加
            settings.windowList.Add(inspectorInstance);
        }

        // 元の選択中のオブジェクトにフォーカスを戻す
        if (targetList.Count > 0)
        {
            Selection.activeGameObject = targetList[0];
        }
    }

    // メニューから呼び出す関数 : インスペクタウィンドウを複製し、ロックし、リストに追加
    [MenuItem("Window/LockInspector/Set Object Tab %&l")]
    public static void ShowSetInspectorWindow()
    {
        if (settings == null)
        {
            settings = AssetDatabase.LoadAssetAtPath<LockInspectorParameter>(SETTINGS_PATH);
            if (settings == null)
            {
                settings = ScriptableObject.CreateInstance<LockInspectorParameter>();
                AssetDatabase.CreateAsset(settings, SETTINGS_PATH);
            }
        }

        // 対象のGameObjectリストを取得
        List<GameObject> targetList = new List<GameObject>();

        targetList.AddRange(settings.settingsObjectList);

        // それぞれのGameObjectについて、インスペクタウィンドウを複製し、ロックし、リストに追加
        foreach (GameObject target in targetList)
        {
            if (target == null)
            {
                continue;
            }

            // インスペクタウィンドウのタイプ
            var inspectorType = typeof(Editor).Assembly.GetType("UnityEditor.InspectorWindow");


            // インスタンスを作成
            var inspectorInstance = ScriptableObject.CreateInstance(inspectorType) as EditorWindow;
            inspectorInstance.ShowUtility();

            // 選択中のオブ
            Selection.activeObject = target;
            inspectorInstance.Focus();

            // isLockedプロパティを取得し、値をtrueに設定
            var isLocked = inspectorType.GetProperty("isLocked", BindingFlags.Instance | BindingFlags.Public);
            isLocked.GetSetMethod().Invoke(inspectorInstance, new object[] { true });

            // InspectorWindowの位置とサイズを設定
            var position = inspectorInstance.position;
            position.x += 50f;
            position.y += -1000f;
            position.x += 400f * settings.windowList.Count;
            position.y += 0f * settings.windowList.Count;
            inspectorInstance.position = position;

            // リストに追加
            settings.windowList.Add(inspectorInstance);
        }

        // 元の選択中のオブジェクトにフォーカスを戻す
        if (targetList.Count > 0)
        {
            Selection.activeGameObject = targetList[0];
        }
    }

    // メニューから呼び出す関数 : 一番新しいインスペクタウィンドウを閉じる
    [MenuItem("Window/LockInspector/Close Last Tab %#l")]
    public static void CloseLastWindow()
    {
        if (settings == null)
        {
            settings = AssetDatabase.LoadAssetAtPath<LockInspectorParameter>(SETTINGS_PATH);
            if (settings == null)
            {
                settings = ScriptableObject.CreateInstance<LockInspectorParameter>();
                AssetDatabase.CreateAsset(settings, SETTINGS_PATH);
            }
        }

        var instance = settings;
        if (instance.windowList.Count == 0)
        {
            return;
        }

        // 一番新しいインスペクタウィンドウを取得し、閉じる
        var last = instance.windowList[instance.windowList.Count - 1];
        instance.windowList.RemoveAt(instance.windowList.Count - 1);
        last.Close();
    }

    // メニューから呼び出す関数 : 全てのインスペクタウィンドウを閉じる
    [MenuItem("Window/LockInspector/Close All Window")]
    public static void CloseAllWindows()
    {
        if (settings == null)
        {
            settings = AssetDatabase.LoadAssetAtPath<LockInspectorParameter>(SETTINGS_PATH);
            if (settings == null)
            {
                settings = ScriptableObject.CreateInstance<LockInspectorParameter>();
                AssetDatabase.CreateAsset(settings, SETTINGS_PATH);
            }
        }

        var instance = settings;
        // 保持している全てのインスペクタウィンドウを閉じる
        foreach (var window in instance.windowList)
        {
            if (window != null)
            {
                window.Close();
            }
        }
        // リストをクリア
        instance.windowList.Clear();
    }

    // メニューから呼び出す関数 : Hierarchyビューで右クリックしたオブジェクトを追加する
    [MenuItem("GameObject/Add to LockInspector")]
    public static void AddGameObjectToSettings()
    {
        if (settings == null)
        {
            settings = AssetDatabase.LoadAssetAtPath<LockInspectorParameter>(SETTINGS_PATH);
            if (settings == null)
            {
                settings = ScriptableObject.CreateInstance<LockInspectorParameter>();
                AssetDatabase.CreateAsset(settings, SETTINGS_PATH);
            }
        }

        var selectedObject = Selection.activeGameObject;
        if (selectedObject == null)
        {
            return;
        }

        if (!settings.settingsObjectList.Contains(selectedObject))
        {
            settings.settingsObjectList.Add(selectedObject);
            EditorUtility.SetDirty(settings);
        }
    }
}
