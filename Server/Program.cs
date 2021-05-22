using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Linq;

namespace Server
{
    class Program
    {
        public static SqlManager instance = new SqlManager();
        
        public static void ListenConnection()
        {
            Socket ConnectionSocket = null;
            while (true)
            {
                try
                {
                    ConnectionSocket = ServerSocket.Accept();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("监听套接字异常{0}", ex.Message);
                    break;
                }
                //获取客户端端口号和IP
                IPAddress ClientIP = (ConnectionSocket.RemoteEndPoint as IPEndPoint).Address;
                int ClientPort = (ConnectionSocket.RemoteEndPoint as IPEndPoint).Port;
                string SendMessage = "连接服务器成功\r\n" + "本地IP:" + ClientIP +",本地端口:" + ClientPort.ToString();
                
                ConnectionSocket.Send(Encoding.UTF8.GetBytes(SendMessage));
                string remotePoint = ConnectionSocket.RemoteEndPoint.ToString();
                Console.WriteLine("成功与客户端{0}建立连接!\t\n", remotePoint);
                
                ClientInformation.Add(remotePoint, ConnectionSocket);

                Thread threadReceive = new Thread(ReceiveMessage);
                threadReceive.IsBackground = true;
                threadReceive.Start(ConnectionSocket);
            }
        }

        public static void SendMessageOne(object Socket)
        {
            //Socket SendSocket = 
        }

        public static void ReceiveMessage(Object SocketClient)
        {
            Socket ReceiveSocket = (Socket)SocketClient;
            while (true)
            {
                byte[] result = new byte[1024 * 1024];
                try
                {
                    string SendMessage = null;
                    IPAddress ClientIP = (ReceiveSocket.RemoteEndPoint as IPEndPoint).Address;
                    int ClientPort = (ReceiveSocket.RemoteEndPoint as IPEndPoint).Port;

                    int ReceiveLength = ReceiveSocket.Receive(result);
                    string ReceiveMessage = Encoding.UTF8.GetString(result, 0, ReceiveLength);
                    if (ReceiveMessage == "") { continue; }
                    Console.WriteLine("接收客户端:" + ReceiveSocket.RemoteEndPoint.ToString() +
                        "时间：" + DateTime.Now.ToString() + "\r\n" + "消息：" + ReceiveMessage + "\r\n\n");

                    SendMessage = instance.SplitString(ReceiveMessage);


                    if(SendMessage == null || SendMessage == "")
                    {
                        continue;
                    }
                    else
                    {
                        foreach (string key in new List<string>(ClientInformation.Keys))
                        {
                            string s = ReceiveSocket.RemoteEndPoint.ToString();
                            if (key == s)
                            {
                                if (SendMessage == "114514")
                                {
                                    Console.WriteLine("客户端断开了连接");
                                    SendMessage = "114514";
                                    break;
                                }
                                Socket socket = ClientInformation[key];
                                Console.WriteLine("向客户端{0}发送消息：{1}", key, SendMessage);
                                socket.Send(Encoding.UTF8.GetBytes(SendMessage));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("监听出现异常!!!");
                    Console.WriteLine("客户端" + ReceiveSocket.RemoteEndPoint + "已经连接中断" + "\r\n" +
                        ex.Message + "\r\n" + ex.StackTrace + "\r\n");
                    foreach (string key in new List<string>(ClientInformation.Keys))
                    {
                        string s = ReceiveSocket.RemoteEndPoint.ToString();
                        if (key.Equals(s))
                        {
                            ClientInformation.Remove(key);
                        }
                    }
                    ReceiveSocket.Shutdown(SocketShutdown.Both);
                    ReceiveSocket.Close();
                    break;
                }
            }
        }

        static Dictionary<string, Socket> ClientInformation = new Dictionary<string, Socket> { };
        static Socket ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        static void Main(String[] args)
        {
            instance.ConnectionSql();
            try
            {
                int Port = 8333;
                IPAddress IP = IPAddress.Parse("127.0.0.1");
                ServerSocket.Bind(new IPEndPoint(IP, Port));
                ServerSocket.Listen(10);
                Console.WriteLine("启动监听成功！");
                Console.WriteLine("监听本地{0}成功", ServerSocket.LocalEndPoint.ToString());
                Thread ThreadListen = new Thread(ListenConnection);
                ThreadListen.IsBackground = true;
                ThreadListen.Start();
                Thread.Sleep(2000);
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("监听异常!!!");
                ServerSocket.Shutdown(SocketShutdown.Both);
                ServerSocket.Close();
            }
            Console.ReadKey();
            Console.WriteLine("监听完毕，按任意键退出！");
        }
    }
}