using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using SLua;

//namespace AppEngine
//{
    public class Reactor
    {
        private Socket socket_ = null;

        private byte[] rbuffer_;
        private int rwpos_ = 0;
        private int flags = 0;

        private byte[] wbuffer_;
        private int wwpos_ = 0;

        private bool sending = false;
        private Action<bool> callback_;
        private Action disconnect_;
		private Action<ushort,ByteArray> handleMessage_;
        private Action<bool> connectResult_;

        private string ipaddr_;
        private int port_;

        enum STATUS
        {
            CLOSED = 0,
            CONNECTING = 1,
            CONNECTED = 2,
        }

        private STATUS status_ = STATUS.CLOSED;
        private static UInt32 READBUFF_SIZE = 1024000;
        private static UInt32 WRITEBUFF_SIZE = 81920;

		public Reactor(Action<bool> callback, Action disconnect, Action<ushort,ByteArray> handleMessage)
        {
            rbuffer_ = new byte[READBUFF_SIZE];
            rwpos_ = 0;

            wbuffer_ = new byte[WRITEBUFF_SIZE];
            wwpos_ = 0;

            callback_ = callback;
            disconnect_ = disconnect;
            handleMessage_ = handleMessage;
        }

        public void connect(string ip, int port)
        {
            ipaddr_ = ip;
            port_ = port;

            connect();
        }

        public void connect()
        {
            rbuffer_ = new byte[READBUFF_SIZE];
            rwpos_ = 0;

            wbuffer_ = new byte[WRITEBUFF_SIZE];
            wwpos_ = 0;

            flags = 0;
            status_ = 0;

            var addrs = Dns.GetHostAddresses(ipaddr_);

            foreach(var a in addrs)
            {
                Debug.LogFormat("parse dns({0}) => {1}", ipaddr_, a.ToString());
            }

            int index = 0;
            connectResult_ = (bool result) => 
            {
                if (result)
                    callback_(true);
                else
                {
                    IPAddress addr = null;
                    if (index < addrs.Length)
                    {
                        addr = addrs[index++];
                        if (addr != null && addr.AddressFamily == AddressFamily.InterNetwork || addr.AddressFamily == AddressFamily.InterNetworkV6)
                        {
                            socket_ = new Socket(addr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                            socket_.NoDelay = true;
                            socket_.Blocking = false;

                            Debug.LogFormat("send buffer size {0}", socket_.SendBufferSize);

                            sending = false;

                            Debug.LogFormat("connect to {0}:{1} r{2} w{3}", addr.ToString(), port_, READBUFF_SIZE, WRITEBUFF_SIZE);

                            try
                            {
                                status_ = STATUS.CONNECTING;
                                socket_.Connect(new IPEndPoint(addr, port_));
                                if (socket_.Connected)
                                {
                                    status_ = STATUS.CONNECTED;
                                    callback_(true);
                                    return;
                                }
                            }
                            catch (SocketException e)
                            {
                                if (e.SocketErrorCode != SocketError.WouldBlock &&
                                    e.SocketErrorCode != SocketError.InProgress)
                                {
                                    Debug.LogFormat("connect to {0}:{1} error:{2}", addr, port_, e.Message);
                                    connectResult_(false);
                                }
                            }
                        }
                    }
                    else
                    {
                        callback_(false);
                    }
                }
            };
            connectResult_(false);
        }

        public bool valid()
        {
            return socket_ != null && socket_.Connected == true;
        }

        public void close()
        {
            if (valid())
            {
                socket_.Shutdown(SocketShutdown.Both);
                socket_ = null;
            }
        }

        public void send(byte[] data)
        {
            if (!valid()) return;

            if (!sending)
            {
                int bytes = 0;
                try
                {
                    bytes = socket_.Send(data);
                    if (bytes != data.Length)
                    {
                        Array.Copy(data, bytes, wbuffer_, wwpos_, data.Length - bytes);
                        wwpos_ += data.Length - bytes;
                        sending = true;
                        //Debug.LogFormat("socket.send block {0}<==>{1} wwpos=>{2}", bytes, data.Length, wwpos_);
                    }
                    else
                    {
                    //Debug.LogFormat("socket.send success {0}<==>{1} wwpos=>{2}", bytes, data.Length, wwpos_);
                }
                }
                catch(SocketException e)
                {
                    if (e.SocketErrorCode == SocketError.WouldBlock ||
                        e.SocketErrorCode == SocketError.TryAgain)
                    {
                        Array.Copy(data, bytes, wbuffer_, wwpos_, data.Length - bytes);
                        wwpos_ += data.Length - bytes;
                        sending = true;
                    Debug.LogFormat("socket.send ex block {0}<==>{1} wwpos=>{2} {3}", bytes, data.Length, wwpos_, e.Message);
                }
                else
                    {
                        Debug.LogFormat("socket.send exception==>{0}", e.Message);
                    }
                }
            }
        }

        public void Poll()
        {
            if (socket_ != null && status_ > 0)
            {
            if (status_ == STATUS.CONNECTING)
                {
                    /*if (socket_.Poll(0, SelectMode.SelectError))
                    {
                        status_ = STATUS.CLOSED;
                        object o = socket_.GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Error);
                        Debug.LogFormat("connect SelectError {0}", o.ToString);
                        socket_.Close();
                        callback_(false);
                    }
                    else */if (socket_.Poll(0, SelectMode.SelectWrite))
                    {
                        if (socket_.Connected)
                        {
                            status_ = STATUS.CONNECTED;
                            //callback_(true);
                            connectResult_(true);
                            object o = socket_.GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Error);
                            Debug.LogFormat("connect SelectError {0}", o);
                        }
                        else
                        {
                            object o = socket_.GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Error);
                            Debug.LogFormat("connect SelectError {0}", o);
                            status_ = STATUS.CLOSED;
                            socket_.Close();
                            connectResult_(false);
                        }
                    }
                }
                else if (status_ == STATUS.CONNECTED)
                {
                    if (socket_.Poll(0, SelectMode.SelectRead))
                    {
                    try
                        {
                            do
                            {
                                int bytesRead = socket_.Receive(rbuffer_, (int)rwpos_, (int)(rbuffer_.Length - rwpos_), SocketFlags.None);
                                if (bytesRead > 0)
                                {
                                    rwpos_ += bytesRead;

                                    splitPacket();
                                }
                                else
                                {
                                    Debug.LogFormat("PacketReceiver::onRecv(): recv bytes : {0}", bytesRead);
                                    socket_.Close();
                                    status_ = STATUS.CLOSED;
                                    if (disconnect_ != null)
                                        disconnect_();
                                    break;
                                }
                            } while (socket_ != null && socket_.Available > 0);
                        }
                        catch (SocketException e)
                        {
                            if (e.SocketErrorCode == SocketError.WouldBlock ||
                                e.SocketErrorCode == SocketError.TryAgain)
                            {
                                Debug.LogFormat("socket recv block ");
                            }
                            else
                            {
                                Debug.LogFormat("PacketReceiver::onRecv(): {0}", e.Message);
                                socket_.Close();
                                status_ = STATUS.CLOSED;
                                if (disconnect_ != null)
                                    disconnect_();
                            }
                        }
                    }

                    if (socket_ != null && socket_.Connected && sending && socket_.Poll(0, SelectMode.SelectWrite))
                    {
                        int bytes = 0;

                        try
                        {
                            bytes = socket_.Send(wbuffer_, wwpos_, SocketFlags.None);
                            if (bytes == wwpos_)
                            {
                                sending = false;
                                wwpos_ = 0;
                                //Debug.LogFormat("send complete {0}", bytes);
                            }
                            else
                            {
                                Array.Copy(wbuffer_, bytes, wbuffer_, 0, wwpos_ - bytes);
                                wwpos_ -= bytes;
                                //Debug.LogFormat("socket.send exx1 block {0}<==>{1} {2}", bytes, wwpos_);
                            }
                        }
                        catch (SocketException e)
                        {
                            if (e.SocketErrorCode == SocketError.WouldBlock ||
                                e.SocketErrorCode == SocketError.TryAgain)
                            {
                                Array.Copy(wbuffer_, bytes, wbuffer_, 0, wwpos_ - bytes);
                                wwpos_ -= bytes;
                                //Debug.LogFormat("socket.send exx block {0}<==>{1} {2}", bytes, wwpos_, e.Message);
                            }
                            else
                            {
                                //Debug.LogFormat("socket.send exception==>{0}", e.Message);
                            }
                        }
                    }
                }
            }
        }

        void splitPacket()
        {
            int length = rwpos_;

            int rpos = 0;
            do
            {
                if (length >= 4)
                {
                    ushort msgid = BitConverter.ToUInt16(rbuffer_, (int)rpos);
                    uint msglen = BitConverter.ToUInt16(rbuffer_, (int)rpos + 2);

                    ushort headlen = 4;

                    if (msglen == 0xffff)
                    {
                        msglen = BitConverter.ToUInt32(rbuffer_, (int)rpos + 4);
                        headlen += 4;
                    }

                    if (length - headlen >= msglen)
                    {
                        byte[] packet = new byte[msglen];
                        Array.Copy(rbuffer_, rpos + headlen, packet, 0, msglen);

                    //Message.handleMessage(msgid, packet);
					handleMessage_(msgid, new ByteArray(packet));

                    rpos += (int)(headlen + msglen);
                        length -= (int)(headlen + msglen);

                    //Debug.LogFormat("recv packet {0}:{1}", msgid, msglen);
                }
                    else
                    {
                    //Debug.LogFormat("splitPacket data length < msglen {0}:{1}>{2}", msgid, msglen, length - headlen);
                    break;
                    }
                }
                else
                {
                //Debug.LogFormat("splitPacket data length {0} < 4", length);
                break;
                }
            } while (true);

            if (length == 0)
            {
                rwpos_ = 0;
            }
            else
            {
                Array.Copy(rbuffer_, rpos, rbuffer_, 0, length);
                rwpos_ = length;
            }
        }
    }
//}
