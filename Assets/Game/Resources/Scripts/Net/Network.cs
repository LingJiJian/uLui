using UnityEngine;
using System;
using SLua;
using System.IO;  
using System.Threading;
using System.Collections;
using System.Collections.Generic;

[CustomLuaClass]
public class LNetwork : MonoBehaviour
{
    private static LNetwork _instance;
    public Action<bool> onConnect;
    public Action onDisconnect;
	public Action<int, ByteArray,bool> onHandleMessage;
    private Dictionary<string, ByteArray> _protoBytes;

	protected bool _isOnConn;
	protected bool _isOnDisConn;
	private bool _connParam;
	private NetworkInterface inter;

	public struct MsgProto
	{
		public int msgId;
		public ByteArray packet;
		public bool isEncrypt;
	}

    public LNetwork()
    {
        _protoBytes = new Dictionary<string, ByteArray>();
    }

    public static LNetwork GetInstance()
    {
        if (_instance == null)
        {
            GameObject obj = new GameObject();
            obj.name = "LNetwork";
            DontDestroyOnLoad(obj);
            _instance = obj.AddComponent<LNetwork>();
        }
        return _instance;
    }

	public void Awake()
	{
		inter = new NetworkInterface (OnConnect, OnHandleMessage);
	}

    public ByteArray GetProtoBytes(string file)
    {
        if (!_protoBytes.ContainsKey(file))
        {
           
            if (LGameConfig.GetInstance().isDebug)
            {
                LArchiveBinFile cArc = new LArchiveBinFile();
                string strFullPath = LGameConfig.GetInstance().GetLoadUrl(LGameConfig.DATA_CATAGORY_LUA + "/Game/Proto/" + file + ".bytes");

                if (!cArc.Open(strFullPath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                {
                    return null;
                }

                if (!cArc.IsValid())
                {
                    return null;
                }
                int nContentLength = (int)cArc.GetStream().Length;
                byte[] aContents = new byte[nContentLength];
                cArc.ReadBuffer(ref aContents, nContentLength);
                cArc.Close();
                _protoBytes.Add(file, new ByteArray(aContents));
            }
            else
            {
                TextAsset asset = LLoadBundle.GetInstance().LoadAsset<TextAsset>("@lua.ab", "@Lua/Game/Proto/" + file + ".bytes");
                _protoBytes.Add(file, new ByteArray(asset.bytes));
            }
        }
        
        return _protoBytes[file];
    }

	public void Update()
	{
		inter.process();
	}

    public void connect(string ip, int port)
    {

		inter.connectTo (ip, port, null);
    }

	public void send(int msgid, ByteArray content,bool isEncrypt)
    {
		byte[] data = content.GetData();
        byte[] packet = new byte[data.Length + 1 + 4 + 4];

        packet[0] = isEncrypt ? (byte)1 : (byte)0;

		byte[] msgIdBytes = BitConverter.GetBytes(msgid);
		Array.Reverse (msgIdBytes);
		Buffer.BlockCopy(msgIdBytes, 0, packet, 1, msgIdBytes.Length);

		byte[] msgLenBytes = BitConverter.GetBytes(data.Length);
		Array.Reverse (msgLenBytes);
		Buffer.BlockCopy(msgLenBytes, 0, packet, 1+4, msgLenBytes.Length);

		Buffer.BlockCopy(data, 0, packet, 1+4+4, data.Length);

		inter.Send (packet);
    }

    public bool valid()
    {
		return inter.valid();
    }
    
    public void close()
    {
		inter.Close();
    }

    private void OnConnect(bool isConn)
    {
		if(onConnect!= null)
			onConnect.Invoke(isConn);
    }
		
	private void OnHandleMessage(int msgId,ByteArray packet,bool isEncrypt)
    {
		if (onHandleMessage != null)
			onHandleMessage.Invoke(msgId, packet,isEncrypt);
    }
}
