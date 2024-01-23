using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class ObjectSelectAssist : EditorWindow
{
    string findPath = "";
    string findName = "";
    bool perfectMatching = false;
    int searchType = 0;
    readonly string[] searchTypeNames = {"名前", "タイプ", };

    public class Info
    {
        public string hpath;
        public Object obj;
        public string[] typeNames;
    }
    static List<Info> lstObjInfo = new List<Info>();

    // オブジェクト選択補助
    [MenuItem("Tools/ObjectSelectAssist")]
    public static void ShowWindow()
    {
        GetWindow<ObjectSelectAssist>("ObjSel");
        CreateHierarchyInfo();
    }

    // Hierarchy内の情報を作成
    static void CreateHierarchyInfo()
    {
        // オブジェクト情報クリア
        lstObjInfo.Clear();

        // 非アクティブなものも含めたHierarchy内全てのゲームオブジェクトを取得
        // Project内のAssetsも含めるため、Hierarchy以下に絞り込んでいる
        // ※ InternalIdentityTransform という名前のGameObjectが含まれてしまうが、とりあえず放置しているので実際に
        //    Hierarchyに表示されているものよりカウントが１つ多くなってる
        var objects = Resources.FindObjectsOfTypeAll(typeof(GameObject))
            .Select(o => o as GameObject)
            .Where(go => go.hideFlags != HideFlags.NotEditable && go.hideFlags != HideFlags.HideAndDontSave && !EditorUtility.IsPersistent(go.transform.root.gameObject))
            .ToArray();


        // Typeで指定した型の全てのオブジェクトを配列で取得し,その要素数分繰り返す.
        for(int i=0; i<objects.Length; i++) {
            // プログレスバーを表示
            if(EditorUtility.DisplayCancelableProgressBar("Create Info", string.Format("{0}/{1}", i+1, objects.Length), (float)(i/objects.Length))) {
                Debug.LogWarning("キャンセルされました");
                break;
            }
            // 情報追加
            var hpath = GetHierarchyPath(objects[i]);
            lstObjInfo.Add(new Info {
                hpath = hpath,
                obj = objects[i],
                typeNames = objects[i].GetComponents<Component>().Where(c => null != c).Select(c => c.GetType().Name).ToArray() });
        }
        // プログレスバーを消す
        EditorUtility.ClearProgressBar();
    }

    // 条件で絞り込む(名前)
    Object[] NarrowDownByConditionsFromName()
    {
        List<Object> lstSelect = new List<Object>();

        int objNum = lstObjInfo.Count;
        for(int i=0; i<objNum; i++) {
            bool add = false;
            // パスの一部の指定がない場合
            if(string.IsNullOrEmpty(findPath)) {
                // 名前がパスに含まれてれば選択、子供は含まない
                var buf = Path.GetFileName(lstObjInfo[i].hpath);
                if(perfectMatching)
                    add = (buf == findName);
                else
                    add = buf.Contains(findName);
            }
            // パスの指定もある
            else {
                // まずはパスの一部が含まれてるか
                if(lstObjInfo[i].hpath.Contains(findPath)) {
                    // 名前がパスの最後に存在するか
                    var buf = Path.GetFileName(lstObjInfo[i].hpath);
                    if(perfectMatching)
                        add = (buf == findName);
                    else
                        add = buf.Contains(findName);
                }
            }
            if(add)
                lstSelect.Add(lstObjInfo[i].obj);
        }
        return lstSelect.ToArray();
    }

    // 条件で絞り込む(タイプ)
    Object[] NarrowDownByConditionsFromType()
    {
        List<Object> lstSelect = new List<Object>();

        int objNum = lstObjInfo.Count;
        for(int i=0; i<objNum; i++) {
            // パスの一部の指定がない場合
            if(string.IsNullOrEmpty(findPath)) {
                // タイプがあるかどうか
                if(lstObjInfo[i].typeNames.Contains(findName, System.StringComparer.OrdinalIgnoreCase))
                    lstSelect.Add(lstObjInfo[i].obj);
            }
            // パスの指定がある
            else {
                // まずはパスの一部が含まれてるか
                if(lstObjInfo[i].hpath.Contains(findPath)) {
                    // タイプがあるかどうか
                    if(lstObjInfo[i].typeNames.Contains(findName, System.StringComparer.OrdinalIgnoreCase))
                        lstSelect.Add(lstObjInfo[i].obj);
                }
            }
        }
        return lstSelect.ToArray();
    }

    // Hierarchyのオブジェクトまでの階層情報を作成
    static string GetHierarchyPath(GameObject obj)
    {
        var path = obj.name;
        var parent = obj.transform.parent;

        // 親がいないなら終了
        while(null != parent) {
            path = string.Format("{0}/{1}", parent.name, path);
            parent = parent.parent;
        }
        return path;
    }

    void OnGUI()
    {
        using(new EditorGUILayout.VerticalScope()) {
            using(new EditorGUILayout.HorizontalScope(GUI.skin.box, GUILayout.ExpandWidth(true))) {
                EditorGUILayout.LabelField(string.Format("オブジェクト数：{0}", lstObjInfo.Count), GUILayout.Width(120));
                GUILayout.FlexibleSpace();
                if(GUILayout.Button("シーンを再検索", GUILayout.Width(120)))
                    CreateHierarchyInfo();
            }
            using(new EditorGUILayout.VerticalScope(GUI.skin.box, GUILayout.ExpandWidth(true))) {
                // 名前か、タイプか
                searchType = GUILayout.SelectionGrid(searchType, searchTypeNames, 2);
                GUILayout.Space(5);
                // 選択したい名前を設定して検索
                EditorGUILayout.LabelField(string.Format("選択したい{0}", searchTypeNames[searchType]));
                using(new EditorGUILayout.HorizontalScope()) {
                    findName = EditorGUILayout.TextField(findName, GUILayout.Width(140));
                    GUILayout.FlexibleSpace();
                    using(new EditorGUI.DisabledGroupScope(1 == searchType))
                    {
                        // 完全一致にチェックが入っている場合は、入力されている名前と全て一致したものが選択対象になる
                        perfectMatching = EditorGUILayout.ToggleLeft("名前の完全一致", perfectMatching, GUILayout.Width(110));
                    }
                }
                // 選択したいパスの一部を設定して条件を絞り込む
                EditorGUILayout.LabelField("絞り込みたい階層の一部");
                using(new EditorGUILayout.HorizontalScope()) {
                    findPath = EditorGUILayout.TextField(findPath, GUILayout.Width(140));
                    GUILayout.FlexibleSpace();
                    using (new EditorGUI.DisabledGroupScope(string.IsNullOrEmpty(findName)))
                    {
                        if(GUILayout.Button("選択", GUILayout.Width(100)))
                            Selection.objects = (0 == searchType) ? NarrowDownByConditionsFromName() : NarrowDownByConditionsFromType();
                    }
                }
            }
        }
    }
}