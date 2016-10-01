using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;
using System.Collections.Generic;

public class TexturePackerImporter : Editor {

    [MenuItem("Tools/Atlas Build")]
    public static void Build()
    {
        CreateAtlas();
        MakeSprites();
    }

    protected static void CreateAtlas()
    {
        //选择并设置TP命令行的参数和参数值
        string args = " --sheet {0}.png --data {1}.txt --format unity-texture2d --trim-mode None --pack-mode Best  --algorithm MaxRects --max-size 2048 --size-constraints POT  --disable-rotation --scale 1 {2}";
        string inputPath = string.Format("{0}/Game/Images", Application.dataPath);//小图目录

        string[] paths = Directory.GetDirectories(inputPath);
        foreach(string path in paths)
        {
            StringBuilder sb = new StringBuilder("");
            string[] files = Directory.GetFiles(path);
            foreach(string file in files)
            {
                if(file.EndsWith(".png") || file.EndsWith(".jpg"))
                {
                    sb.Append(file);
                    sb.Append(" ");
                }
            }
            string name = Path.GetFileName(path);
            string sheetPath = string.Format("{0}/Game/Resources/Atlas/{1}", Application.dataPath, name);//用TP打包好的图集存放目录
            Helper.RunCmd("E:/TexturePacker/bin/TexturePacker.exe", string.Format(args, sheetPath, sheetPath, sb.ToString()), "");
            Debug.Log("生成图集:" + sheetPath);
        }
        AssetDatabase.Refresh();
    }

    protected static void MakeSprites()
    {
        string atlasPath = string.Format("{0}/Game/Resources/Atlas", Application.dataPath);
        string[] files = Directory.GetFiles(atlasPath);
        foreach(string file in files)
        {
            if (file.EndsWith(".txt"))
            {
                Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(file.Replace(Application.dataPath, "Assets").Split('.')[0] + ".png");
                TextAsset textAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(file.Replace(Application.dataPath, "Assets"));
                string[] lineArray = textAsset.text.Split(new char[] { '\n' });

                Dictionary<string, Vector4> tIpterMap = new Dictionary<string, Vector4>();
                TextureImporter asetImp = GetTextureIpter(texture);
                SaveBoreder(tIpterMap, asetImp);
                
                List<SpriteMetaData> metaDatas = new List<SpriteMetaData>();
                for (int i = 0; i < lineArray.Length; i++)
                {
                    if ((lineArray[i].IndexOf('#') == -1) && (lineArray[i].IndexOf(':') == -1))
                    {
                        lineArray[i] = lineArray[i].Replace("\r", "");
                        if ((lineArray[i] != ""))
                        {
                            string[] str = lineArray[i].Split(new char[] { ';' });

                            SpriteMetaData metaData = new SpriteMetaData();
                            metaData.name = str[0];
                            metaData.rect = new Rect(
                                float.Parse(str[1]),
                                float.Parse(str[2]),
                                float.Parse(str[3]),
                                float.Parse(str[4]));
                            metaData.pivot = new Vector2(0.5f, 0.5f);
                            if (tIpterMap.ContainsKey(metaData.name)) //保存九宫格数据
                            {
                                metaData.border = tIpterMap[metaData.name];
                            }
                            metaDatas.Add(metaData);
                        }
                    }
                }
                asetImp.spritesheet = metaDatas.ToArray();
                asetImp.textureType = TextureImporterType.Sprite;
                asetImp.spriteImportMode = SpriteImportMode.Multiple;
                asetImp.mipmapEnabled = false;
                asetImp.SaveAndReimport();

                AssetDatabase.DeleteAsset(file.Replace(Application.dataPath, "Assets"));
            } 
        }
    }

    //如果这张图集已经拉好了9宫格，需要先保存起来
    protected static void SaveBoreder(Dictionary<string, Vector4> tIpterMap, TextureImporter tIpter)
    {
        for (int i = 0, size = tIpter.spritesheet.Length; i < size; i++)
        {
            tIpterMap.Add(tIpter.spritesheet[i].name, tIpter.spritesheet[i].border);
        }
    }

    protected static TextureImporter GetTextureIpter(Texture2D texture)
    {
        TextureImporter textureIpter = null;
        string impPath = AssetDatabase.GetAssetPath(texture);
        textureIpter = TextureImporter.GetAtPath(impPath) as TextureImporter;
        return textureIpter;
    }
}
