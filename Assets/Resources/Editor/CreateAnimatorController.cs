using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;//5.0改变 UnityEditorInternal;并不能用了。
using System.IO;

public class CreateAnimatorController : Editor
{
    [MenuItem("LUtil/创建AnimController")]
    static void DoCreateAnimationAssets()
    {
        string curDir = System.Environment.CurrentDirectory + "/Assets/Resources/Models";
        string[] files = Directory.GetFiles(curDir);

        foreach (string file in files)
        {
            string baseName = System.IO.Path.GetFileNameWithoutExtension(file);
            if (baseName.IndexOf(".fbx") != -1)
            {
                string fileName = baseName.Substring(baseName.LastIndexOf("\\") + 1, (baseName.LastIndexOf(".") - baseName.LastIndexOf("\\") - 1));
                //创建Controller
                AnimatorController animatorController = AnimatorController.CreateAnimatorControllerAtPath(string.Format("Assets/Resources/Animators/{0}.controller", fileName));
                //得到它的Layer
                AnimatorControllerLayer layer = animatorController.layers[0];
                //将动画保存到 AnimatorController中
                AddStateTransition( animatorController, string.Format("Assets/Resources/Models/{0}", baseName), layer);

                Debug.Log(string.Format("创建AnimatorController {0} 成功 ", baseName));
            }
        }
    }

    private static void AddStateTransition(AnimatorController ctrl, string path, AnimatorControllerLayer layer)
    {
        AnimatorStateMachine sm = layer.stateMachine;
        //根据动画文件读取它的AnimationClip对象
        Object[] objs = AssetDatabase.LoadAllAssetsAtPath(path);
        List<AnimationClip> clips = new List<AnimationClip>();
        for (int i = 0; i < objs.Length;i++ )
        {
            AnimationClip clip = objs[i] as AnimationClip;
            if (clip != null && clip.name.IndexOf("__preview__") == -1)
            {
                clips.Add(clip);
            }
        }

        foreach (AnimationClip newClip in clips)
        {
            AnimatorState state = sm.AddState(newClip.name);
            state.motion = newClip;

            string cond = string.Format("is{0}", state.motion.name);
            ctrl.AddParameter(cond,AnimatorControllerParameterType.Bool);
            
            AnimatorStateTransition tran = sm.AddAnyStateTransition(state);
            tran.AddCondition(AnimatorConditionMode.If, 0, cond);
            tran.duration = 0;
        }
    }
}