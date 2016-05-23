using System.IO;
using System;

public class LArchiveBinFile : LArchiveBin {

    /**
     * open file.
     * @param void.
     * @return void.
     */
    public bool Open(string strFileName, FileMode eMode, FileAccess eAccess)
    {
        if (string.IsNullOrEmpty(strFileName))
        {
            return false;
        }

        if ((FileMode.Open == eMode) && !File.Exists(strFileName))
        {
            return false;
        }

        try
        {
            m_cStream = new FileStream(strFileName, eMode, eAccess);
        }
        catch (Exception cEx)
        {
            Console.Write(cEx.Message);
        }

        if (null == m_cStream)
        {
            return false;
        }

        m_bOpen = true;
        return true;
    }

    /**
     * Close this stream.
     * @param void.
     * @return bool - true if success, otherwise false.
     */
    public bool Close()
    {
        if (null == m_cStream)
        {
            m_bOpen = false;
            return false;
        }

        m_cStream.Close();
        m_bOpen = false;
        return true;
    }
}
