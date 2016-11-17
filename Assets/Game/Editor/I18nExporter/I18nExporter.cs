using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using System.Text;
// 国际化 导出
public class I18nExporter : Editor
{
    private static Dictionary<string, Dictionary<string, string>> _markDatas;
    private static Dictionary<string, string> _markFiles;
    private static List<string> ignorePath;
    private static int _markId;
    private static string _outputPath;

    [MenuItem("Tools/I18n Export")]
    public static void Run()
    {
        Load();
        Scan();
        Export();
    }

    private static void Load()
    {
        _outputPath = Application.dataPath + "/Game/@Lua/Game/Common/i18n.lua";
        ignorePath = new List<string>()
        {
            "i18n.lua"
        };

        if (File.Exists(_outputPath))
        {
            _markDatas = new Dictionary<string, Dictionary<string, string>>();
            _markFiles = new Dictionary<string, string>();

            StreamReader sr = new StreamReader(_outputPath, Encoding.UTF8);
            string line;
            Dictionary<string, string> markDic = null;
            while ((line = sr.ReadLine()) != null)
            {
                if (line.StartsWith("i18n["))
                {
                    int startIdx = line.LastIndexOf("i18n[\"") + 6;
                    int endIdx = line.IndexOf("\"]");
                    string key = line.Substring(startIdx, endIdx - startIdx);

                    startIdx = line.LastIndexOf("= \"") + 3;
                    endIdx = line.LastIndexOf("\"");
                    string value = line.Substring(startIdx, endIdx - startIdx);
                    markDic.Add(value,string.Format("i18n[\"{0}\"]",key));

                    _markId = Mathf.Max(_markId, int.Parse(key));
                }
                else if (line.StartsWith("--# "))
                {
                    int startIdx = line.IndexOf("--# ") + 4;
                    int endIdx = line.LastIndexOf(".lua") + 4;
                    string fileName = line.Substring(startIdx, endIdx - startIdx);

                    _markDatas.Add(fileName, new Dictionary<string, string>());
                    markDic = _markDatas[fileName];
                }
            }
            sr.Close();
            sr.Dispose();
        }
        else
        {
            _markDatas = new Dictionary<string, Dictionary<string, string>>();
            _markFiles = new Dictionary<string, string>();
            _markId = 1000;
        }
    }

    private static void Scan()
    {
        Helper.forEachHandle(Application.dataPath + "/Game/@Lua", new List<string> { "lua" }, (string filePath) =>
        {
            string basePath = Path.GetFileName(filePath);
            if (ignorePath.Contains(basePath)) return;

            Dictionary<string, string> markDic = null; //标记字典
            if (!_markDatas.ContainsKey(basePath))
            {
                markDic = new Dictionary<string, string>();
                _markDatas.Add(basePath, markDic);
            }
            else
            {
                markDic = _markDatas[basePath];
            }

            using (StreamReader sr = new StreamReader(filePath, Encoding.UTF8))
            {
                string line;
                StringBuilder content = new StringBuilder();
                while ((line = sr.ReadLine()) != null)
                {
                    StringBuilder newLine = new StringBuilder();
                    StringBuilder markKey = new StringBuilder();
                    bool isMarking = false;
                    char[] chars = line.ToCharArray();
                    for (int i = 0; i < chars.Length; i++)
                    {
                        char c = chars[i];
                        bool _isReset = false;
                        if (c == '"' && chars[i - 1] != '\\')
                        {
                            isMarking = !isMarking;
                            if (isMarking == false) //结束标记
                            {
                                string markKeyString = markKey.ToString();
                                
                                if (isChinese(markKeyString))
                                {
                                    string holder = getHolder(basePath, markKeyString);
                                    if (!markDic.ContainsKey(markKeyString))
                                        markDic.Add(markKeyString, holder);

                                    newLine.Append(holder);
                                    _isReset = true;
                                }
                                else //不是中文
                                {
                                    newLine.Append('"').Append(markKeyString);
                                }
                                markKey = new StringBuilder(); //清空
                            }
                        }
                        else
                        {
                            if (isMarking)
                            {
                                markKey.Append(c);
                            }
                        }
                        if (!isMarking && !_isReset)
                        {
                            newLine.Append(c);
                        }
                    }
                    content.Append(newLine).Append('\n');
                }

                if (markDic.Count > 0)
                {
                    _markFiles.Add(filePath, content.ToString());
                }
                sr.Close();
            }
        });

    }

    private static void Export()
    {
        StringBuilder i18n = new StringBuilder();
        i18n.Append("local i18n = {}\n");

        foreach (string fileName in _markDatas.Keys)
        {
            if(_markDatas[fileName].Count > 0)
            {
                i18n.AppendFormat("--# {0}\n", fileName);
            }
            foreach(string chinese in _markDatas[fileName].Keys)
            {
                i18n.AppendFormat("{0} = \"{1}\"\n", _markDatas[fileName][chinese], chinese);
            }
        }
        i18n.Append("\nLDeclare(\"i18n\", i18n)\n");
        i18n.Append("return i18n\n");

        using (FileStream fs = new FileStream(_outputPath, FileMode.Create))
        {
            StreamWriter sw = new StreamWriter(fs);
            sw.Write(i18n.ToString());
            sw.Flush();
            sw.Close();
            fs.Close();
        }

        foreach (string filePath in _markFiles.Keys)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                StreamWriter sw = new StreamWriter(fs);
                sw.Write(_markFiles[filePath]);
                sw.Flush();
                sw.Close();
                fs.Close();
            }  
        }

        AssetDatabase.Refresh();
        Debug.Log("导出完成 " + _outputPath);
    }

    private static string getHolder(string fileName ,string chinese)
    {
        Dictionary<string, string> dic = _markDatas[fileName];
        if (!dic.ContainsKey(chinese))
        {
            _markId++;
            dic.Add(chinese,string.Format("i18n[\"{0}\"]",_markId.ToString()));
        }
        return dic[chinese];
    }

    private static bool isChinese(string text)
    {
        bool hasChinese = false;
        char[] c = text.ToCharArray();
        int len = c.Length;
        for (int i = 0; i < len; i++)
        {
            if (c[i] >= 0x4e00 && c[i] <= 0x9fbb)
            {
                hasChinese = true;
                break;
            }
        }
        return hasChinese;
    }
}
