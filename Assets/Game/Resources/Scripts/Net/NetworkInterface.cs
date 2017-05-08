
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;


    public enum SocketErrorType
    {
        SocketException,
        Exception
    }
    public class NetworkInterface
    {
        public const int MAX_BUFFER_SIZE = 65535;
        public const int MAX_PACKETS_PER_FRAME = 200;
        public const float CONNECT_TIME_OUT = 25;

        public delegate void ConnectCallback(string ip, int port, bool success, SocketErrorType errorType, object userData);
        public class ConnectState
        {
            public bool success = true;
            public string connectIP = "";
            public int connectPort = 0;
            public ConnectCallback connectCB = null;
            public object userData = null;
            public Socket socket = null;
            public NetworkInterface networkInterface = null;
            public SocketErrorType errorType;
            public string error = "";
        }
        protected TCPClientWorker m_tcpWorker;
        private Socket m_socket;

        
        public int gateType = 0;   // 0 login  1 game

        private string ip = "";
        private int port = 0;
        public bool connectCallbackFlag = false;
        public bool Connecting { get; set; }
		private static Action<bool> OnConn;
		private static Action<int,SLua.ByteArray,bool> OnDataReceive;
		public NetworkInterface(Action<bool> onConn,Action<int,SLua.ByteArray,bool> onDataReceive)
        {
            this.Connecting = false;
			NetworkInterface.OnConn = onConn;
			NetworkInterface.OnDataReceive = onDataReceive;
        }
        
        public bool valid()
        {
            if ((this.m_socket != null) && (this.m_socket.Connected == true) && m_tcpWorker != null && m_tcpWorker.SocketValid)
            {
                return true;
            }
            return false;
        }

        public void Close()
        {
            if (this.m_socket != null)
            {
                try
                {
                    this.m_socket.Shutdown(SocketShutdown.Both);
                    this.m_socket.Close();
                    this.m_socket = null;
                }
                catch (Exception ex)
                {
				UnityEngine.Debug.LogFormat("Close Exception: {0}",ex.ToString());
                }
            }
            if (m_tcpWorker != null)
            {
                m_tcpWorker.Close();
                m_tcpWorker = null;
            }
        }

        public void Send(byte[] data)
        {
            if (m_tcpWorker != null)
                m_tcpWorker.Push(data);
        }

        public void connectTo(string ip, int port, object userData)
        {
            if (valid())
                throw new InvalidOperationException("Have already connected!");
            string newip = ip;
            #if UNITY_IOS
			newip = NtUniSdk.Unity3d.SdkU3d.getIpStr(ip);
			IPAddress[] addrIps = Dns.GetHostAddresses(newip);
			AddressFamily ipTypes = addrIps[0].AddressFamily;
            m_socket = new Socket(ipTypes, SocketType.Stream, ProtocolType.Tcp);
            LoggerHelper.Warning("IpTypes = {0}",ipTypes);
            #else
            m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            #endif
            
            m_socket.SetSocketOption(System.Net.Sockets.SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer, MAX_BUFFER_SIZE);
            m_socket.NoDelay = true;
            ConnectState state = new ConnectState();
            this.ip = newip;
            this.port = port;
            state.connectIP = newip;
            state.connectPort = port;
            state.userData = userData;
            state.socket = m_socket;
            state.networkInterface = this;

            UnityEngine.Debug.Log("connect to " + newip + ":" + port);
            try
            {
                m_socket.BeginConnect(new IPEndPoint(IPAddress.Parse(newip), port), new AsyncCallback(connectCB), state);
                connectCallbackFlag = false;
				LNetwork.GetInstance().StartCoroutine(waitForConnect(state));
                this.Connecting = true;
            }
            catch (SocketException e)
            {
			UnityEngine.Debug.LogFormat("connectTo error: {0}", e.ToString());
                state.errorType = SocketErrorType.SocketException;
                state.error = e.ToString();
                state.success = false;
                _onConnectStatus(state);
            }
            catch (Exception e)
            {
			UnityEngine.Debug.LogFormat("connectTo error: {0}", e.ToString());
                state.errorType = SocketErrorType.Exception;
                state.error = e.ToString();
                state.success = false;
                _onConnectStatus(state);
            }
        }

        public void reconnectTo(object userData)
        {
            connectTo(ip, port, userData);
        }

        private IEnumerator waitForConnect(ConnectState state)
        {
            float connectTimeAcc = 0.0f;
            bool connectTimeout = false;
            while (!state.networkInterface.connectCallbackFlag)
            {
                // 等待异步连接返回
                connectTimeAcc += Time.deltaTime;
                if (connectTimeAcc > CONNECT_TIME_OUT)
                {
                    connectTimeout = true;
                    break;
                }
                yield return null;
            }
            if (connectTimeout)
            {
                state.error = "connect timeout";
                state.errorType = SocketErrorType.SocketException;
                state.success = false;
                _onConnectStatus(state);
            }
            else
            {
                _onConnectStatus(state);
            }
        }
        private static IEnumerator CheckNetworkStateLoop(NetworkInterface networkInterface)
        {
            if (networkInterface.valid())
                yield return null;
            // 网络断开，回调接口
        }
        private static void _onConnectStatus(ConnectState state)
        {
            NetworkInterface networkInterface = state.networkInterface;
            networkInterface.Connecting = false;
            if (state.success)
            {
                // 创建发送和接收进程开始接收数据
                networkInterface.startTcpWorker();
                // 启动协程判断网络是否断开
				LNetwork.GetInstance().StartCoroutine(CheckNetworkStateLoop(networkInterface));
            }
            else
            {
                networkInterface.Close();
                UnityEngine.Debug.Log("connect error: " + state.error);
            }
		if (NetworkInterface.OnConn != null)
			NetworkInterface.OnConn.Invoke (state.success);
            //EventDispatcher.Instance.TriggerEvent<bool, NetworkInterface.ConnectState>(EventIdType.ConnectSuccess, state.success, state);
        }
        private static void connectCB(IAsyncResult ar)
        {
            UnityEngine.Debug.Log("connectCB...........");
            ConnectState state = null;
            try
            {
                state = (ConnectState)ar.AsyncState;
                state.socket.EndConnect(ar);
            }
            catch (SocketException e)
            {
                state.errorType = SocketErrorType.SocketException;
                state.error = e.ToString();
                state.success = false;
            }
            catch (Exception e)
            {
                state.errorType = SocketErrorType.Exception;
                state.error = e.ToString();
                state.success = false;
            }
            state.networkInterface.connectCallbackFlag = true;
        }

        private void startTcpWorker()
        {
            if (gateType == 1)
            { // 连游戏节点，设置超时时间为30s
                m_socket.ReceiveTimeout = 30000;
            }
            m_tcpWorker = new TCPClientWorker(m_socket);
            
        }

        public void process()
        {
            int hasRecieved = 0;
            while (hasRecieved < MAX_PACKETS_PER_FRAME)
            {
                if (m_tcpWorker == null)
                    return;
                
                byte[] data = m_tcpWorker.Recv();
                if (data == null || data.Length == 0)
                {
                    // check network state
                    break;
                }
                else
                {
				    if (NetworkInterface.OnDataReceive != null)
                    {
						bool isEncrypt = BitConverter.ToBoolean(data,0);
						int msgid = BitConverter.ToInt32(data,1);
						int nLength = BitConverter.ToInt32(data, 1 + 4);

						byte[] packet = new byte[nLength];
						Buffer.BlockCopy(data, 4 + 4 + 1, packet, 0, nLength);

						NetworkInterface.OnDataReceive(msgid,new SLua.ByteArray(packet),isEncrypt);
                        
                    }
                    hasRecieved++;
                }
            }
        }
    }

