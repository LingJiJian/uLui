using System;
using System.Text.RegularExpressions;
using ICSharpCode.SharpZipLib.Zip;
using System.IO;
using UnityEngine;
using System.Security.Cryptography;
using System.Text;
using SLua;

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


    /// <summary>  
    /// AES加密  
    /// </summary>  
    /// <param name="Data">被加密的明文</param>  
    /// <param name="Key">密钥</param>  
    /// <param name="Vector">向量</param>  
    /// <returns>密文</returns>  
    public static Byte[] AESEncrypt(Byte[] Data, String Key, String Vector)
    {
        Byte[] bKey = new Byte[32];
        Array.Copy(Encoding.UTF8.GetBytes(Key.PadRight(bKey.Length)), bKey, bKey.Length);
        Byte[] bVector = new Byte[16];
        Array.Copy(Encoding.UTF8.GetBytes(Vector.PadRight(bVector.Length)), bVector, bVector.Length);
        Byte[] Cryptograph = null; // 加密后的密文  
        Rijndael Aes = Rijndael.Create();
        try
        {
            // 开辟一块内存流  
            using (MemoryStream Memory = new MemoryStream())
            {
                // 把内存流对象包装成加密流对象  
                using (CryptoStream Encryptor = new CryptoStream(Memory,
                 Aes.CreateEncryptor(bKey, bVector),
                 CryptoStreamMode.Write))
                {
                    // 明文数据写入加密流  
                    Encryptor.Write(Data, 0, Data.Length);
                    Encryptor.FlushFinalBlock();

                    Cryptograph = Memory.ToArray();
                }
            }
        }
        catch
        {
            Cryptograph = null;
        }
        return Cryptograph;
    }

    /// <summary>  
    /// AES解密  
    /// </summary>  
    /// <param name="Data">被解密的密文</param>  
    /// <param name="Key">密钥</param>  
    /// <param name="Vector">向量</param>  
    /// <returns>明文</returns>  
    public static Byte[] AESDecrypt(Byte[] Data, String Key, String Vector)
    {
        Byte[] bKey = new Byte[32];
        Array.Copy(Encoding.UTF8.GetBytes(Key.PadRight(bKey.Length)), bKey, bKey.Length);
        Byte[] bVector = new Byte[16];
        Array.Copy(Encoding.UTF8.GetBytes(Vector.PadRight(bVector.Length)), bVector, bVector.Length);

        Byte[] original = null; // 解密后的明文  

        Rijndael Aes = Rijndael.Create();
        try
        {
            // 开辟一块内存流，存储密文  
            using (MemoryStream Memory = new MemoryStream(Data))
            {
                // 把内存流对象包装成加密流对象  
                using (CryptoStream Decryptor = new CryptoStream(Memory,
                Aes.CreateDecryptor(bKey, bVector),
                CryptoStreamMode.Read))
                {
                    // 明文存储区  
                    using (MemoryStream originalMemory = new MemoryStream())
                    {
                        Byte[] Buffer = new Byte[1024];
                        Int32 readBytes = 0;
                        while ((readBytes = Decryptor.Read(Buffer, 0, Buffer.Length)) > 0)
                        {
                            originalMemory.Write(Buffer, 0, readBytes);
                        }

                        original = originalMemory.ToArray();
                    }
                }
            }
        }
        catch
        {
            original = null;
        }
        return original;
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
                return new Color(red, green, blue, 255);
            case 6:
                rgb = color.ToCharArray();
                red = Convert.ToInt32(rgb[0].ToString() + rgb[1].ToString(), 16);
                green = Convert.ToInt32(rgb[2].ToString() + rgb[3].ToString(), 16);
                blue = Convert.ToInt32(rgb[4].ToString() + rgb[5].ToString(), 16);
                return new Color(red, green, blue,255);
            default:
                return Color.white;
        }
    }

	public static int[] ints(int i){
		return new int[] { i };
	}
}
