// 模块名   :  TCPClientWorker
// 创建者   :  liwenfeng
// 创建日期 :  2014-7-31
// 描    述 :  客户端网络接收类

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;


    /// <summary>
    ///TCP Client 类，用于异步收发包
    ///做了并包处理
    ///使用 .net TcpClient 实现
    /// </summary>
    public class TCPClientWorker
    {
        private readonly Queue<byte[]> m_sendQueue = new Queue<byte[]>();
        private readonly Queue<byte[]> m_recvQueue = new Queue<byte[]>();
        private const int MAX_SEND_BUFFER_SIZE = 4096;
        private const int MAX_RECV_BUFFER_SIZE = 65535;

        private const int HEAD_LENGTH = 4;

        private readonly byte[] m_sendBuffer;
        private byte[] m_sendLargeBuffer;
        private readonly byte[] m_recvBuffer;
        private int m_nRecvBufferSize = 0;
        private Socket m_socket = null;

        /// <summary>
        /// 异步读取数据线程。
        /// </summary>
        private Thread m_receiveThread;

        /// <summary>
        /// 异步发送数据线程。
        /// </summary>
        private Thread m_sendThread;
        /// <summary>
        /// 接收数据队列同步锁。
        /// </summary>
        private readonly object m_recvQueueLocker = new object();

        /// <summary>
        /// 发送数据队列同步锁。
        /// </summary>
        private readonly object m_sendQueueLocker = new object();

        /// <summary>
        /// 网络通信同步锁。
        /// </summary>
        private readonly object m_tcpClientLocker = new object();

        /// <summary>
        /// 读取流数据量标记
        /// </summary>
        private Int32 bytesRead;

        private bool m_asynSendSwitch = true;

        public bool SocketValid = true;
        public TCPClientWorker(Socket socket)
        {
            m_socket = socket;
            this.m_sendBuffer = new byte[MAX_SEND_BUFFER_SIZE];
            this.m_recvBuffer = new byte[MAX_RECV_BUFFER_SIZE];
            StartSendThread();
            StartReceiveThread();
        }

        private Thread StartReceiveThread()
        {
            UnityEngine.Debug.Log("start receive thread.........");
            m_receiveThread = new Thread(new ThreadStart(DoReceive));
            m_receiveThread.IsBackground = true;
            m_receiveThread.Start();
            return m_receiveThread;
        }

        /// <summary>
        /// 启动异步发送线程
        /// </summary>
        private Thread StartSendThread() {
            UnityEngine.Debug.Log("start send thread.........");
            m_sendThread = new Thread(new ThreadStart(this.AsynSend));
            m_sendThread.IsBackground = true;

            if (!m_sendThread.IsAlive)
            {
                UnityEngine.Debug.Log("Start AsynSend: " + this.m_asynSendSwitch);
                m_sendThread.Start();
            }
            return m_sendThread;
        }

        /// <summary>
        /// 提供给上层的发送函数。 仅将数据放入发送队列
        /// </summary>
        /// <param name="bytes"></param>
        public void Push(byte[] bytes)
        {
            lock (this.m_sendQueueLocker)
                this.m_sendQueue.Enqueue(bytes);
        }

        /// <summary>
        ///提供给上层的接受函数， 仅从接受队列获取一个数据包
        ///空队列返回 null
        /// </summary>
        /// <returns></returns>
        public byte[] Recv()
        {
            if (this.m_recvQueue.Count > 0)
            {
                byte[] res;
                lock (this.m_recvQueueLocker)
                    res = this.m_recvQueue.Dequeue();
                return res;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 关闭链接，由主线程调用
        /// </summary>
        public void Close()
        {
            // 清除发送消息队列
            lock (this.m_sendQueueLocker)
            {
                this.m_sendQueue.Clear();
                this.m_sendQueue.TrimExcess();
            }
            this.m_asynSendSwitch = false; //关闭发送死循环
            this.m_sendThread = null;
            this.m_receiveThread = null;
            GC.Collect();
        }
        /// <summary>
        /// 启动线程发送数据。
        /// </summary>
        private void AsynSend()
        {
            while (this.m_asynSendSwitch)
            {
                this.DoSend();
                Thread.Sleep(20);
            }
        }

        /// <summary>
        /// 每帧调用， 发送数据。
        /// 并包处理
        /// </summary>
        private void DoSend()
        {
            if ((this.m_socket == null) || (this.m_socket.Connected == false))
            {
                return;
            }
            int nTotalLength = 0;
            int npack = 0;
            // 并包
            lock (this.m_sendQueueLocker)
            {
                while ((nTotalLength < MAX_SEND_BUFFER_SIZE) && this.m_sendQueue.Count > 0)
                {
                    byte[] packet = this.m_sendQueue.Peek();
                    if (nTotalLength + HEAD_LENGTH + packet.Length < MAX_SEND_BUFFER_SIZE)
                    {
					/*
                        //message struct: length(4 bytes), reserve(2 bytes), packet data
                        //packet data:  serial(2 bytes), msg data[msg_id(4 bytes), protobuff data]
                        byte[] length = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(packet.Length));
                        length.CopyTo(this.m_sendBuffer, nTotalLength);
                        nTotalLength += HEAD_LENGTH;
                        packet.CopyTo(this.m_sendBuffer, nTotalLength);
                        nTotalLength += packet.Length;
					*/
					Buffer.BlockCopy (packet, 0, this.m_sendBuffer, nTotalLength,packet.Length);
						nTotalLength += packet.Length;
                        this.m_sendQueue.Dequeue();
                        npack++;
                    }
                    else if (npack == 0)
                    {
                        m_sendLargeBuffer = new byte[HEAD_LENGTH + packet.Length];
                        /*
						byte[] length = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(packet.Length));
                        length.CopyTo(m_sendLargeBuffer, nTotalLength);
                        nTotalLength += HEAD_LENGTH;
                        packet.CopyTo(m_sendLargeBuffer, nTotalLength);
                        nTotalLength += packet.Length;
                        */
					Buffer.BlockCopy (packet, 0, m_sendLargeBuffer, nTotalLength,packet.Length);
						nTotalLength += packet.Length;
                        this.m_sendQueue.Dequeue();
                    }
                    else
                    {
                        break;
                    }
                }
            }
            if (nTotalLength > 0)
            {
                // 发送数据
                try
                {
                    int ret;
                    if (nTotalLength < MAX_SEND_BUFFER_SIZE)
                    {
                        ret = this.m_socket.Send(this.m_sendBuffer, 0, nTotalLength, SocketFlags.None);
                    }
                    else
                    {
                        ret = this.m_socket.Send(this.m_sendLargeBuffer, 0, nTotalLength, SocketFlags.None);
                    }
                    if (ret != nTotalLength)
                    {
					UnityEngine.Debug.LogFormat("socket send data, len={0}, send_len={1}", nTotalLength, ret);
                    }
                }
                catch (Exception)
                {
                    SocketValid = false;
                }
            }
        }

        private void DoReceive()
        {
            int zero_count = 0;
            int timeout = 0;
            //读取流
            do
            {
                this.bytesRead = 0;
                try
                {
                    int size = MAX_RECV_BUFFER_SIZE - this.m_nRecvBufferSize;
                    if (size > 0)
                    {
                        this.bytesRead = this.m_socket.Receive(this.m_recvBuffer,
                                this.m_nRecvBufferSize, size, SocketFlags.None);
                        this.m_nRecvBufferSize += this.bytesRead;
                        if (this.bytesRead == 0)
                        {
                            //读的长度为0
                            zero_count++;
                            if (zero_count < 3)
                            {
                                this.bytesRead = 1;
                            }
                        }
                        else
                        {
                            timeout = 0;
                            zero_count = 0;
                        }
                    }
                    else
                    {
                        this.bytesRead = 1; //缓存不够时继续循环，后面会对缓存数据进行处理
						UnityEngine.Debug.Log("buffer not enough");
                    }
                    this.SplitPackets();
                }
                catch (SocketException e)
                {
                    if (e.SocketErrorCode == System.Net.Sockets.SocketError.TimedOut && timeout < 1)
                    {
					UnityEngine.Debug.LogFormat("socket timeout {0}", timeout);
                        timeout++;
                        this.bytesRead = 1;
                    }
                }
                catch (Exception)
                {
                    this.bytesRead = 0;
                }
            }
            while (this.bytesRead > 0);
            SocketValid = false;
			UnityEngine.Debug.Log("DataReceive Thread Exit........");
        }

        /// <summary>
        /// 从RecvBuffer 中切分出多个Packets, 不足一个 Packet 的部分， 存留在 Buffer 中留待下次Split
        /// </summary>
        private void SplitPackets()
        {
            int offset = 0;
            while (this.m_nRecvBufferSize > HEAD_LENGTH)
            {
				int nLength = BitConverter.ToInt32(this.m_recvBuffer, offset + 1 + 4);
                //offset += HEAD_LENGTH;
                //int nLength = IPAddress.NetworkToHostOrder(len);
                //UnityEngine.Debug.Log("receive msg, length: " + nLength);
                if (this.m_nRecvBufferSize >= nLength + 4 +  HEAD_LENGTH + 1)
                {
                    int packageLength = nLength + 4 +4 + 1;
					byte[] packet = new byte[packageLength];
					Buffer.BlockCopy(this.m_recvBuffer, offset, packet, 0, packageLength);
                    lock (this.m_recvQueueLocker) //此处理为独立线程处理，需加锁，否则会出现丢包
                    {
                        this.m_recvQueue.Enqueue(packet);
                    }
					this.m_nRecvBufferSize -= packageLength;
					offset += packageLength;
                }
                else
                {
                    //offset -= HEAD_LENGTH; //需要调整偏移
                    break;
                }
            }
            // 整理 RecvBuffer， 将buffer内容前移
            Buffer.BlockCopy(this.m_recvBuffer, offset, this.m_recvBuffer, 0,
                    this.m_nRecvBufferSize);
        }
    }
