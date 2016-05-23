using UnityEngine;
using UnityEditor;
using System.Collections;

public class ExportConfigWindow : EditorWindow
{
    public static string EXPORT_PREFABS_PATH = "Assets/Game/Resources/Prefabs";
    public static string EXPORT_SCENE_PATH = "Assets/Game/Resources/Scenes";
    public static string EXPORT_OUT_PATH = Application.dataPath;
	public static BuildTarget BUILD_TARGET = BuildTarget.StandaloneWindows;

    [MenuItem("Tools/HotFix Config")]
    static void Init()
    {
#if UNITY_STANDALONE_WIN
		BUILD_TARGET = BuildTarget.StandaloneWindows;
#elif UNITY_ANDROID
        BUILD_TARGET = BuildTarget.Android;
#elif UNITY_ANDROID
		BUILD_TARGET = BuildTarget.iPhone;
#elif UNITY_STANDALONE_OSX
		BUILD_TARGET = BuildTarget.StandaloneOSXUniversal;
#endif
        // Get existing open window or if none, make a new one:
        ExportConfigWindow window = (ExportConfigWindow)EditorWindow.GetWindow(typeof(ExportConfigWindow),true,"Hotfix");
        window.Show();
    }

    void OnGUI()
    {

        GUILayout.Label("Resource Path", EditorStyles.boldLabel);
        EXPORT_SCENE_PATH = EditorGUILayout.TextField("Scenes", EXPORT_SCENE_PATH);
        EXPORT_PREFABS_PATH = EditorGUILayout.TextField("Prefabs", EXPORT_PREFABS_PATH);

        GUILayout.Label("Output Path", EditorStyles.boldLabel);
        EXPORT_OUT_PATH = EditorGUILayout.TextField("Output", EXPORT_OUT_PATH);

		GUILayout.Label("build target", EditorStyles.boldLabel);
		BUILD_TARGET =(BuildTarget)EditorGUILayout.EnumPopup("Target",BUILD_TARGET);

        if (GUILayout.Button("run"))
        {
            EditorApplication.delayCall += ExportAssetBundles.Run;
        }

    }

}
