#region 模块信息

/*----------------------------------------------------------------
// 模块名：ServerProxy
// 创建者：liwenfeng
// 修改者列表：
// 创建日期：2014.7.31
// 模块描述：远程服务代理类
//----------------------------------------------------------------*/

#endregion
/*
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using ProtoBuf;
using ProtoBuf.Meta;
using UnityEngine;

namespace Game.Util
{
    public enum ReconnectType
    {
        background,
        foreground
    }
    /// <summary>
    /// 远程服务控制类。
    /// </summary>
    public class ServerProxy
    {
        public class ProtocolAtom : IComparable
        {
            public Int32 msg_id;
            public Int32 count;
            public Int32 traffic;
            public int CompareTo(object obj)
            {
                ProtocolAtom b = (ProtocolAtom)obj;
                if (this.traffic > b.traffic)
                    return 0;
                return 1;
            }
        }
        private static ServerProxy mInstance;
        protected NetworkInterface networkInterface;

        protected int m_SerialNumber = 0;  // 前端消息序列号，通过心跳发送给服务器，服务器通过对比上一个序列号，从缓存队列中移除已被前端接收的消息
        protected short m_MessageSerial = 0;  // 消息的序列号, 0 - 65535, 每发送一个消息递增加1, 用于服务器判断是否处理过，也用于判断中间是否存在消息丢失
        public int totalSend = 0;
        protected Queue<byte[]> m_MsgSaved = new Queue<byte[]>(); // 消息缓存队列
        private bool reconnectFlag = false;  // 是否在重连的标志

        private bool encode = false;   // 消息是否加密
        private RC4Crypto encoder = new RC4Crypto();
        private RC4Crypto decoder = new RC4Crypto();

        public Action OnNetworkEnable;   // 恢复网络回调
        public Action OnNetworkDisable;  // 网络不可连接回调

        private long loginKey;
        protected const int MSGID_LENGTH = 4;
        private const int   RESERVE_SIZE = 2;
        static Dictionary<Int32, Type> msgType = new Dictionary<Int32, Type>();

        public bool collect = false;
        public long collectTime = 0;
        public Dictionary<Int32, ProtocolAtom> ProtocolCollect = new Dictionary<Int32, ProtocolAtom>();
        
        /// <summary>
        /// 控制类实例。
        /// </summary>
        public static ServerProxy Instance
        {
            get
            {
                if (mInstance == null)
                    mInstance = new ServerProxy();
                return mInstance;
            }
        }
        public void StartProtocolCollect()
        {
            ProtocolCollect.Clear();
            collect = true;
            collectTime = TimeHelper.TimeNow();
        }
        public void StopProtocolCollect()
        {
            collect = false;
        }
        public void PrintProtocolCollect()
        {
            if (!collect)
                return;
            string aiPointFile = Application.dataPath + "/../ProtocolCollect.txt";
            double total = 0;
            using (Stream stream = new FileStream(aiPointFile, FileMode.Create, FileAccess.Write))
            {
                List<ProtocolAtom> list = new List<ProtocolAtom>();
                foreach (var atom in ProtocolCollect)
                {
                    list.Add((ProtocolAtom)atom.Value);
                }
                list.Sort();
                string line;
                for(int i = 0;i < list.Count; i++)
                {
                    total += list[i].traffic;
                    line = string.Format("msg_id: {0}, count: {1}, traffic: {2}, mean: {3}\n", list[i].msg_id, list[i].count, list[i].traffic, list[i].traffic/list[i].count);
                    stream.Write(System.Text.Encoding.Default.GetBytes(line), 0, line.Length);
                }
                total = total/1000.0f;
                line = string.Format("total traffic: {0} k, mean: {1} k/s", total, total*1000/(TimeHelper.TimeNow()-collectTime));
                stream.Write(System.Text.Encoding.Default.GetBytes(line), 0, line.Length);
            }
        }
        public NetworkInterface NetworkInterface
        {
            get { return networkInterface; }
            set 
            { 
                networkInterface = value;
            }
        }
        public int SerialNumber
        {
            get { return m_SerialNumber; }
            set { m_SerialNumber = value; }
        }
        public ServerProxy()
        {
            networkInterface = new NetworkInterface();
            networkInterface.OnDataReceive = OnDataReceive;
            //EventDispatcher.Instance.AddEventListener(EventIdType.OnApplicationQuit, OnApplicationQuit);
        }
        public bool Encode
        {
            get { return encode; }
            set { encode = value; }
        }
        public void SetKey(string pass)
        {
            encoder.SetKey(pass);
            decoder.SetKey(pass);
        }
        public bool Connected
        {
            get
            {
                return networkInterface == null? false : networkInterface.valid();
            }
        }

        /// <summary>
        /// 限制了每帧 收包的数量。 
        /// </summary>
        private const ushort MAX_PACKETS_PER_FRAME = 5;
        protected const int MAX_MSG_SAVED = 30;   // 最多缓冲的消息个数

        private bool networkAvail = false;
        public void Disconnect()
        {
            encode = false;
            networkAvail = false;
            if (networkInterface != null)
                networkInterface.Close();
            EventDispatcher.Instance.TriggerEvent(EventIdType.StopHeartBeatTimer);
            LoggerHelper.Debug("disconnect.............");
        }

        /// <summary>
        /// 调用远程方法。
        /// </summary>
        public void SendMsg<T>(Int32 msgId, T data)
        {
            if (msgId != 10710 && msgId != 10100 && msgId != 10101)
            {
                LoggerHelper.Debug("----send:" + msgId + "----");
            }
            try
            {
                byte[] result = packMessage<T>(msgId, data);
                m_MessageSerial += 1;
                totalSend += 1;
                m_MsgSaved.Enqueue(result);  // 把消息缓存起来，直到收到服务器确认消息才从缓存中移除
                if (!reconnectFlag)
                {
                    if (encode)
                    {
                        byte[] result1 = encoder.EncryptEx(result);
                        networkInterface.Send(result1); // 进入发送队列等待发送
                    }
                    else
                    {
                        networkInterface.Send(result); // 进入发送队列等待发送
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("send msg error: " + msgId + " " + ex.Message);
            }
        }

        public void RawSendMsg<T>(Int32 msgId, T data)
        {
            byte[] result = packMessage<T>(msgId, data);
            networkInterface.Send(result);
        }

        public void ResendSavedMsg()
        {
            Queue<byte[]>.Enumerator enumerator = m_MsgSaved.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (encode)
                {
                    byte[] result = encoder.EncryptEx(enumerator.Current);
                    networkInterface.Send(result);
                }
                else
                {
                    networkInterface.Send(enumerator.Current);
                }
            }
        }
        public void SavedMsgDequeue(int Count)
        {
            if (Count > 0 && m_MsgSaved.Count >= Count)
            {
                while (Count > 0)
                {
                    m_MsgSaved.Dequeue();
                    Count--;
                }
            }
        }
        public void ClearSavedMsg()
        {
            m_MsgSaved.Clear();
            m_MsgSaved.TrimExcess();
        }
        public void ResetMessageSerial()
        {
            m_MessageSerial = 0;
            totalSend = 0;
        }

        private void CheckNetwork()
        {
            if (reconnectFlag)
                return;
            if (networkInterface.valid())
            {
                return;
            }
            else
            {
                Disconnect();
                EventDispatcher.Instance.TriggerEvent(EventIdType.NetworkDisconnect, networkInterface);
            }
        }

        public void Release()
        {
            LoggerHelper.Debug("ServerProxy Release");
            networkInterface.Close();
        }
        public void StartReceiveMsg()
        {
            networkAvail = true;
        }
        public void StopReceiveMsg()
        {
            networkAvail = false;
        }
        public bool ReConnectFlag
        {
            set {reconnectFlag = value;}
        }
        public void Update()
        {
            if (networkAvail && networkInterface != null)
            {
                CheckNetwork();
                networkInterface.process();
            }
        }

        public void OnDataReceive(byte[] data)
        {
            try
            {
                Int32 msg_id;
                object msg;
                if (encode)
                {
                    byte[] result = decoder.DecryptEx(data);
                    msg = unPackMessage(result, out msg_id);
                }
                else
                {
                    msg = unPackMessage(data, out msg_id);
                }
                if (!reconnectFlag)
                    m_SerialNumber++;
                EventDispatcher.Instance.TriggerEvent<object>(msg_id, msg);
            }
            catch (Exception ex)
            {
                LoggerHelper.Except(ex);
            }
        }
        private void OnApplicationQuit()
        {
            Release();
        }
        /// <summary>
        /// 打包消息， TODO: 加密
        /// </summary>
        private byte[] packMessage<T>(Int32 msgId, T data)
        {
            MemoryStream streamBuff = new MemoryStream();
            Serializer.Serialize<T>(streamBuff, data);
            byte[] msg_id_bytes = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(msgId));
            byte[] serial_bytes = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(m_MessageSerial));
            // 计算消息长度
            byte[] result = new byte[RESERVE_SIZE + serial_bytes.Length + msg_id_bytes.Length + streamBuff.Length];

            if (result.Length > 2048)
            {
                LoggerHelper.Error("message large, id={0}, len={1} k", msgId, result.Length / 1024.0f);
            }
            // 添加序列号
            Buffer.BlockCopy(serial_bytes, 0, result, RESERVE_SIZE, serial_bytes.Length);
            // 添加协议id
            Buffer.BlockCopy(msg_id_bytes, 0, result, RESERVE_SIZE + serial_bytes.Length, msg_id_bytes.Length);

            Buffer.BlockCopy(streamBuff.GetBuffer(), 0, result, RESERVE_SIZE + serial_bytes.Length + msg_id_bytes.Length,
                    (int)streamBuff.Length);
            return result;
        }
        /// <summary>
        /// 消息解包， TODO: 解密
        /// </summary>
        private object unPackMessage(byte[] data, out Int32 msg_id)
        {
            msg_id = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(data, RESERVE_SIZE));
            int msg_type = msg_id / 100;
            if (msg_id != 10710 && msg_id != 10101 && msg_id != 10100 && msg_id != 10201 && msg_id != 10208 && msg_type != 101 && msg_type != 106)
            {
                LoggerHelper.Debug("----receive:" + msg_id + "----");
            }
            Type type;
            if (!ServerProxy.msgType.TryGetValue(msg_id, out type))
            {
                type = typeof(ServerProxy).Assembly.GetType("msg.MSGS" + msg_id);
                ServerProxy.msgType.Add(msg_id, type);
            }
            if (collect)
            {
                ProtocolAtom atom;
                if (!ProtocolCollect.TryGetValue(msg_id, out atom))
                {
                    atom = new ProtocolAtom();
                    atom.msg_id = msg_id;
                    ProtocolCollect.Add(msg_id, atom);
                }
                atom.count += 1;
                atom.traffic += data.Length;
            }
            MemoryStream streamBuff = new MemoryStream(data, MSGID_LENGTH + RESERVE_SIZE, data.Length - RESERVE_SIZE - MSGID_LENGTH);
            streamBuff.Position = 0;
            object msg = RuntimeTypeModel.Default.Deserialize(streamBuff, null, type);
            return msg;
        }
    }
}
*/