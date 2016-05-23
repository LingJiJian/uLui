using UnityEngine;
using System;
using System.IO;
using System.Text;

public class LArchiveBin  {

    // buff size
    public readonly int TX_BUFSIZE = 8;

    // file stream
    protected Stream m_cStream = null;

    // open flag
    protected bool m_bOpen = false;

    /**
     * Constructor.
     * @param void.
     * @return void.
     */
    public LArchiveBin()
    {
    }

    /**
     * Get the stream of this archive.
     * @param void.
     * @return - Stream, currently used stream.
     */
    public Stream GetStream()
    {
        return m_cStream;
    }

    /**
     * Get if this archive is valid.
     * @param void.
     * @return - bool - true if valid, otherwise false.
     */
    public bool IsValid()
    {
        return (null != m_cStream && m_bOpen) ? true : false;
    }

    /**
     * Write buffer.
     * @param string strText - Text value.
     * @return - Data size.
     */
    public int ReadBuffer(ref byte[] aBuff, int nBuffSize)
    {
        m_cStream.Read(aBuff, 0, nBuffSize);
        return aBuff.Length;
    }

    /**
     * Read bool.
     * @param ref bool bValue - Bool value.
     * @return - Data size.
     */
    public int ReadBool(ref bool bValue)
    {
        byte yValue = 0;
        ReadInt8(ref yValue);
        bValue = (0 == yValue) ? false : true;

        return sizeof(byte);
    }

    /**
     * Read string.
     * @param ref string strText - Text value.
     * @return - Data size.
     */
    public int ReadString(ref string strValue)
    {
        return ReadString(ref strValue, false);
    }

    /**
     * Read string.
     * @param ref string strText - Text value.
     * @param bool bUnicode - Is unicode or not.
     * @return - Data size.
     */
    public int ReadString(ref string strValue, bool bUnicode)
    {
        if (!IsValid())
        {
            return 0;
        }

        if (bUnicode)
        {
            byte[] aBuff = new byte[TX_BUFSIZE * 8];
            m_cStream.Read(aBuff, 0, sizeof(int));
            int nStrLen = BitConverter.ToInt32(aBuff, 0);

            // check buff size
            if (nStrLen > TX_BUFSIZE)
            {
                aBuff = new byte[nStrLen];
            }

            m_cStream.Read(aBuff, 0, nStrLen);
            strValue = Encoding.Unicode.GetString(aBuff, 0, nStrLen);
            return nStrLen;
        }
        else
        {
            byte[] aBuff = new byte[TX_BUFSIZE * 8];
            m_cStream.Read(aBuff, 0, sizeof(int));
            int nStrLen = BitConverter.ToInt32(aBuff, 0);

            // check buff size
            if (nStrLen > TX_BUFSIZE)
            {
                aBuff = new byte[nStrLen];
            }

            m_cStream.Read(aBuff, 0, nStrLen);
            strValue = Encoding.ASCII.GetString(aBuff, 0, nStrLen);
            return nStrLen;
        }
    }

    /**
     * Read float.
     * @param float nValue - Float value.
     * @return - Data size.
     */
    public int ReadFloat(ref float fValue)
    {
        byte[] aBuff = new byte[TX_BUFSIZE * 2];
        m_cStream.Read(aBuff, 0, sizeof(float));
        fValue = BitConverter.ToSingle(aBuff, 0);

        return sizeof(float);
    }

    /**
     * Read double.
     * @param double dValue - Double value.
     * @return - Data size.
     */
    public int ReadDouble(ref double dValue)
    {
        byte[] aBuff = new byte[TX_BUFSIZE * 4];
        m_cStream.Read(aBuff, 0, sizeof(double));
        dValue = BitConverter.ToDouble(aBuff, 0);

        return sizeof(double);
    }

    /**
     * Read int8.
     * @param char n8Value - Char value.
     * @return - Data size.
     */
    public int ReadInt8(ref byte n8Value)
    {
        byte[] aBuff = new byte[TX_BUFSIZE * 2];
        m_cStream.Read(aBuff, 0, sizeof(byte));
        n8Value = aBuff[0];

        return sizeof(byte);
    }

    /**
     * Read int32.
     * @param int nValue - int32 value.
     * @return - Data size.
     */
    public int ReadInt16(ref Int16 n16Value)
    {
        byte[] aBuff = new byte[TX_BUFSIZE * 2];
        m_cStream.Read(aBuff, 0, sizeof(Int16));
        n16Value = BitConverter.ToInt16(aBuff, 0);

        return sizeof(Int16);
    }

    /**
     * Read uint32.
     * @param UInt32 nValue - uint32 value.
     * @return - Data size.
     */
    public int ReadUInt16(ref UInt16 u16Value)
    {
        byte[] aBuff = new byte[TX_BUFSIZE * 2];
        m_cStream.Read(aBuff, 0, sizeof(UInt16));
        u16Value = BitConverter.ToUInt16(aBuff, 0);

        return sizeof(UInt16);
    }

    /**
     * Read int32.
     * @param int nValue - int32 value.
     * @return - Data size.
     */
    public int ReadInt32(ref int nValue)
    {
        byte[] aBuff = new byte[TX_BUFSIZE * 2];
        m_cStream.Read(aBuff, 0, sizeof(int));
        nValue = BitConverter.ToInt32(aBuff, 0);

        return sizeof(int);
    }

    /**
     * Read uint32.
     * @param UInt32 nValue - uint32 value.
     * @return - Data size.
     */
    public int ReadUInt32(ref UInt32 u32Value)
    {
        byte[] aBuff = new byte[TX_BUFSIZE * 2];
        m_cStream.Read(aBuff, 0, sizeof(UInt32));
        u32Value = BitConverter.ToUInt32(aBuff, 0);

        return sizeof(UInt32);
    }

    /**
     * Read int32.
     * @param int nValue - Int64 n64Value.
     * @return - Data size.
     */
    public int ReadInt64(ref Int64 n64Value)
    {
        byte[] aBuff = new byte[TX_BUFSIZE * 4];
        m_cStream.Read(aBuff, 0, sizeof(Int64));
        n64Value = BitConverter.ToInt64(aBuff, 0);

        return sizeof(Int64);
    }

    /**
     * Read uint32.
     * @param UInt32 nValue - UInt64 dw64Value.
     * @return - Data size.
     */
    public int ReadUInt64(ref UInt64 dw64Value)
    {
        byte[] aBuff = new byte[TX_BUFSIZE * 4];
        m_cStream.Read(aBuff, 0, sizeof(UInt64));
        dw64Value = BitConverter.ToUInt64(aBuff, 0);

        return sizeof(UInt64);
    }

    /**
     * Write buffer.
     * @param string strText - Text value.
     * @return - Data size.
     */
    public int WriteBuffer(byte[] aBuff)
    {
        if (!IsValid())
        {
            return 0;
        }

        m_cStream.Write(aBuff, 0, aBuff.Length);
        return aBuff.Length;
    }

    /**
     * Write bool value.
     * @param bool bValue - bool value.
     * @return - Data size.
     */
    public int WriteBool(bool bValue)
    {
        if (!IsValid())
        {
            return 0;
        }

        byte yValue = (byte)(bValue ? 1 : 0);
        return WriteInt8(yValue);
    }

    /**
     * Write string.
     * @param string strText - Text value.
     * @return - Data size.
     */
    public int WriteString(string strValue)
    {
        return WriteString(strValue, false);
    }

    /**
     * Write string.
     * @param string strText - Text value.
     * @param bool bUnicode - Is unicode or not.
     * @return - Data size.
     */
    public int WriteString(string strValue, bool bUnicode)
    {
        if (!IsValid())
        {
            return 0;
        }

        if (bUnicode)
        {
            // Write string length.
            byte[] aLength = BitConverter.GetBytes(strValue.Length * sizeof(char));
            m_cStream.Write(aLength, 0, sizeof(int));

            // Write string.
            byte[] aString = Encoding.Unicode.GetBytes(strValue);
            m_cStream.Write(aString, 0, strValue.Length * sizeof(char));
            return (strValue.Length * sizeof(char) + sizeof(int));
        }
        else
        {
            // Write string length.
            byte[] aLength = BitConverter.GetBytes(strValue.Length);
            m_cStream.Write(aLength, 0, sizeof(int));

            // Write string.
            byte[] aString = Encoding.ASCII.GetBytes(strValue);
            m_cStream.Write(aString, 0, strValue.Length);
            return (strValue.Length + sizeof(int));
        }
    }

    /**
     * Write float.
     * @param float nValue - Float value.
     * @return - Data size.
     */
    public int WriteFloat(float fValue)
    {
        if (!IsValid())
        {
            return 0;
        }

        byte[] aInt32 = BitConverter.GetBytes(fValue);
        m_cStream.Write(aInt32, 0, sizeof(float));
        return sizeof(float);
    }

    /**
     * Write double.
     * @param double dValue - Double value.
     * @return - Data size.
     */
    public int WriteDouble(double dValue)
    {
        if (!IsValid())
        {
            return 0;
        }

        byte[] aInt32 = BitConverter.GetBytes(dValue);
        m_cStream.Write(aInt32, 0, sizeof(double));
        return sizeof(double);
    }

    /**
     * Write int8.
     * @param byte n8Value - Byte n8Value.
     * @return - Data size.
     */
    public int WriteInt8(byte n8Value)
    {
        if (!IsValid())
        {
            return 0;
        }

        byte[] aBuff = { n8Value };
        m_cStream.Write(aBuff, 0, sizeof(byte));
        return sizeof(byte);
    }

    /**
     * Write int16.
     * @param Int16 nValue - Int16 value.
     * @return - Data size.
     */
    public int WriteInt16(Int16 nValue)
    {
        if (!IsValid())
        {
            return 0;
        }

        byte[] aInt16 = BitConverter.GetBytes(nValue);
        m_cStream.Write(aInt16, 0, sizeof(Int16));
        return sizeof(Int16);
    }

    /**
     * Write uint16.
     * @param UInt16 nValue - UInt16 value.
     * @return - Data size.
     */
    public int WriteUInt16(UInt16 nValue)
    {
        if (!IsValid())
        {
            return 0;
        }

        byte[] aUInt16 = BitConverter.GetBytes(nValue);
        m_cStream.Write(aUInt16, 0, sizeof(UInt16));
        return sizeof(UInt16);
    }

    /**
     * Write int32.
     * @param int nValue - int32 value.
     * @return - Data size.
     */
    public int WriteInt32(int nValue)
    {
        if (!IsValid())
        {
            return 0;
        }

        byte[] aInt32 = BitConverter.GetBytes(nValue);
        m_cStream.Write(aInt32, 0, sizeof(Int32));
        return sizeof(Int32);
    }

    /**
     * Write uint32.
     * @param UInt32 nValue - uint32 value.
     * @return - Data size.
     */
    public int WriteUInt32(UInt32 nValue)
    {
        if (!IsValid())
        {
            return 0;
        }

        byte[] aUInt32 = BitConverter.GetBytes(nValue);
        m_cStream.Write(aUInt32, 0, sizeof(UInt32));
        return sizeof(UInt32);
    }

    /**
     * Write int64.
     * @param Int64 n64Value - int64 value.
     * @return - Data size.
     */
    public int WriteInt64(Int64 n64Value)
    {
        if (!IsValid())
        {
            return 0;
        }

        byte[] aInt64 = BitConverter.GetBytes(n64Value);
        m_cStream.Write(aInt64, 0, sizeof(Int64));
        return sizeof(Int64);
    }

    /**
     * Write uint64.
     * @param UInt64 dw64Value - uint64 value.
     * @return - Data size.
     */
    public int WriteUInt64(UInt64 dw64Value)
    {
        if (!IsValid())
        {
            return 0;
        }

        byte[] aUInt64 = BitConverter.GetBytes(dw64Value);
        m_cStream.Write(aUInt64, 0, sizeof(UInt64));
        return sizeof(UInt64);
    }
}
