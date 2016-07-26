using UnityEngine;
using System;
using SLua;
using System.Collections.Generic;

[CustomLuaClass]
public class Network : MonoBehaviour
{
    private static Network _instance;
    private Reactor _reactor;
    public Action<bool> onConnect;
    public Action onDisconnect;
	public Action<ushort, ByteArray> onHandleMessage;
    private Dictionary<string, ByteArray> _protoBytes;

    public Network()
    {
        _protoBytes = new Dictionary<string, ByteArray>();
        _reactor = new Reactor(OnConnect,OnDisconnect, OnHandleMessage);
    }

    public static Network GetInstance()
    {
        if (_instance == null)
        {
            GameObject obj = new GameObject();
            obj.name = "Network";
            DontDestroyOnLoad(obj);
            _instance = obj.AddComponent<Network>();
            
        }
        return _instance;
    }

    public ByteArray GetProtoBytes(string file)
    {
        if (!_protoBytes.ContainsKey(file))
        {
            if (LGameConfig.GetInstance().isDebug)
            {
                TextAsset textAsset = Resources.Load<TextAsset>(LGameConfig.DATA_CATAGORY_LUA + "/Game/Proto/" + file);
                _protoBytes.Add(file, new ByteArray(textAsset.bytes));
            }
            else
            {
                string strFullPath = LGameConfig.GetInstance().GetLoadUrl(LGameConfig.DATA_CATAGORY_LUA + "/Game/Proto/" + file + ".bytes");

                LArchiveBinFile cArc = new LArchiveBinFile();
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
        }
        
        return _protoBytes[file];
    }

    void Update()
    {
        _reactor.Poll();
    }

    public void connect(string ip, int port)
    {
        _reactor.connect(ip, port);
    }

	public void send(ushort msgid, ByteArray content)
    {
		byte[] data = content.data;
        byte[] packet = new byte[data.Length + 2 + 2];

		packet[0] = (byte)(msgid & 0xff);
		packet[1] = (byte)(msgid >> 8 & 0xff);
		packet[2] = (byte)((UInt16)data.Length & 0xff);
		packet[3] = (byte)((UInt16)data.Length >> 8 & 0xff);
		Array.Copy(data, 0, packet, 4, data.Length);

		_reactor.send(packet);
    }

    public bool valid()
    {
        return _reactor.valid();
    }
    
    public void close()
    {
        _reactor.close();
    }

    private void OnConnect(bool isConn)
    {
        if(onConnect!= null)
           onConnect.Invoke(isConn);
    }

    private void OnDisconnect()
    {
        if (onDisconnect != null)
            onDisconnect.Invoke();
    }

	private void OnHandleMessage(ushort msgId,ByteArray packet)
    {
        if (onHandleMessage != null)
			onHandleMessage.Invoke(msgId, packet);
    }
}
