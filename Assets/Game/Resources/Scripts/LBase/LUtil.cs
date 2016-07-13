using System;
using System.Text.RegularExpressions;
using ICSharpCode.SharpZipLib.Zip;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using SLua;
using Lui;

[CustomLuaClassAttribute]
public class LUtil {

    /**
     * Format a exception string.
     * 
     * @param System.Exception e - The exception object.
     * @return string - The result.
     */
	[DoNotToLua]
    public static string FormatException(System.Exception e)
    {
        string strSource = string.IsNullOrEmpty(e.Source) ? "<no source>" : e.Source.Substring(0, e.Source.Length - 2);
        return string.Format("{0}\nLua (at {2})", e.Message, string.Empty, strSource);
    }

    //zip压缩
	[DoNotToLua]
    public static void PackFiles(string filename, string directory)
    {
        try
        {
            FastZip fz = new FastZip();
            fz.CreateEmptyDirectories = true;
            fz.CreateZip(filename, directory, true, "");
            fz = null;
        }
        catch (Exception e)
        {
            Debug.LogError(FormatException(e));
        }
    }

    //zip解压
	[DoNotToLua]
    public static bool UnpackFiles(string file, string dir)
    {
        try
        {
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            ZipInputStream s = new ZipInputStream(File.OpenRead(file));

            ZipEntry theEntry;
            while ((theEntry = s.GetNextEntry()) != null)
            {

                string directoryName = Path.GetDirectoryName(theEntry.Name);
                string fileName = Path.GetFileName(theEntry.Name);

                if (directoryName != String.Empty)
                    Directory.CreateDirectory(dir + directoryName);

                if (fileName != String.Empty)
                {
                    FileStream streamWriter = File.Create(dir + theEntry.Name);

                    int size = 2048;
                    byte[] data = new byte[2048];
                    while (true)
                    {
                        size = s.Read(data, 0, data.Length);
                        if (size > 0)
                        {
                            streamWriter.Write(data, 0, size);
                        }
                        else
                        {
                            break;
                        }
                    }

                    streamWriter.Close();
                }
            }
            s.Close();
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError(FormatException(e));
        }
        return false;
    }


	// If currently build platform is windows.
	public static bool Windows
	{
		#if UNITY_STANDALONE_WIN
		get { return true; }
		#else
		get { return false; }
		#endif
	}

	// If currently build platform is osx.
	public static bool OSX
	{
		#if UNITY_STANDALONE_OSX
		get { return true; }
		#else
		get { return false; }
		#endif
	}

	// If currently build platform is iphone.
	public static bool iPhone
	{
		#if UNITY_IPHONE
		get { return true; }
		#else
		get { return false; }
		#endif
	}

	// If currently build platform is android.
	public static bool Android
	{
		#if UNITY_ANDROID
		get { return true; }
		#else
		get { return false; }
		#endif
	}

    /// <summary>
    /// 获取当前时间戳
    /// </summary>
    /// <param name="bflag">为真时获取10位时间戳,为假时获取13位时间戳.</param>
    /// <returns></returns>
    public static long GetTimeStamp(bool bflag = true)
    {
        TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
        long ret;
        if (bflag)
            ret = Convert.ToInt64(ts.TotalSeconds);
        else
            ret = Convert.ToInt64(ts.TotalMilliseconds);
        return ret;
    }

    static DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
    public static string NormalizeTimpstamp0(long timpStamp)
    {
        long unixTime = timpStamp * 10000000L;
        TimeSpan toNow = new TimeSpan(unixTime);
        DateTime dt = dtStart.Add(toNow);
        return dt.ToString("yyyy-MM-dd");
    }


    /// <summary>
    /// 时钟式倒计时
    /// </summary>
    /// <param name="second"></param>
    /// <returns></returns>
    public string GetSecondString(int second)
    {
        return string.Format("{0:D2}", second / 3600) + string.Format("{0:D2}", second % 3600 / 60) + ":" + string.Format("{0:D2}", second % 60);
    }

    /// 将Unix时间戳转换为DateTime类型时间
    /// </summary>
    /// <param name="d">double 型数字</param>
    /// <returns>DateTime</returns>
    public static System.DateTime ConvertIntDateTime(double d)
    {
        System.DateTime time = System.DateTime.MinValue;
        System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0));
        Debug.Log(startTime);
        time = startTime.AddSeconds(d);
        return time;
    }

    /// <summary>
    /// 将c# DateTime时间格式转换为Unix时间戳格式
    /// </summary>
    /// <param name="time">时间</param>
    /// <returns>double</returns>
    public static double ConvertDateTimeInt(System.DateTime time)
    {
        double intResult = 0;
        System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
        intResult = (time - startTime).TotalSeconds;
        return intResult;
    }


    /// <summary>
    /// 日期转换成unix时间戳
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static long DateTimeToUnixTimestamp(DateTime dateTime)
    {
        var start = new DateTime(1970, 1, 1, 0, 0, 0, dateTime.Kind);
        return Convert.ToInt64((dateTime - start).TotalSeconds);
    }

    /// <summary>
    /// unix时间戳转换成日期
    /// </summary>
    /// <param name="unixTimeStamp">时间戳（秒）</param>
    /// <returns></returns>
    public static DateTime UnixTimestampToDateTime(DateTime target, long timestamp)
    {
        DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, target.Kind);
        return start.AddSeconds(timestamp);
    }

    public static Color StringToColor(string color)
    {
        int red, green, blue = 0;
        char[] rgb;
        color = color.TrimStart('#');
        color = Regex.Replace(color.ToLower(), "[g-zG-Z]", "");
        switch (color.Length)
        {
            case 3:
                rgb = color.ToCharArray();
                red = Convert.ToInt32(rgb[0].ToString() + rgb[0].ToString(), 16);
                green = Convert.ToInt32(rgb[1].ToString() + rgb[1].ToString(), 16);
                blue = Convert.ToInt32(rgb[2].ToString() + rgb[2].ToString(), 16);
                return new Color(red, green, blue);
            case 6:
                rgb = color.ToCharArray();
                red = Convert.ToInt32(rgb[0].ToString() + rgb[1].ToString(), 16);
                green = Convert.ToInt32(rgb[2].ToString() + rgb[3].ToString(), 16);
                blue = Convert.ToInt32(rgb[4].ToString() + rgb[5].ToString(), 16);
                return new Color(red, green, blue);
            default:
                return Color.white;
        }
    }

	public static int[] ints(int i){
		return new int[] { i };
	}
}
