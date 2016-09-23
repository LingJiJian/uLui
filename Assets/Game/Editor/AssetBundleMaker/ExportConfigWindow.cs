using UnityEngine;
using UnityEditor;
using System.Collections;

public class ExportConfigWindow : EditorWindow
{
	public static string EXPORT_OUT_PATH = Application.streamingAssetsPath;
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
        ExportConfigWindow window = (ExportConfigWindow)EditorWindow.GetWindow(typeof(ExportConfigWindow), true, "Hotfix");
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Label("build target", EditorStyles.boldLabel);
        BUILD_TARGET = (BuildTarget)EditorGUILayout.EnumPopup("Target", BUILD_TARGET);

        if (GUILayout.Button("run"))
        {
            EditorApplication.delayCall += ExportAssetBundles.Run;
        }
    }

}
