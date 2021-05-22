using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class Client : MonoBehaviour
{
    public static Client _instance;

    private void Awake()
    {
        EventCenter.AddListener(EventDefine.SingleGame, SingleOne);
        EventCenter.AddListener(EventDefine.GameQuit, CloseSocket);
        _instance = this;
    }

    public void Start()
    {
        Connection();
    }

    public void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.SingleGame, SingleOne);
        EventCenter.RemoveListener(EventDefine.GameQuit, CloseSocket);
    }

    private static byte[] result = new byte[1024 * 1024];
    static Socket ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    public string Text = null;
    public int a = 0;
    public string Data = null;
    public void ReceiveMessage()
    { 
        int i = 0; 
        while (true)
        {
            try
            {
                string MessageChange = null;
                int ReceiveLength = ClientSocket.Receive(result);
                String Message = Encoding.UTF8.GetString(result, 0, ReceiveLength);
                if(Message == "")
                {
                    i++;
                }
                else
                { 
                    if (Message.Contains("|"))
                    {
                        Debug.Log("包含数据");
                        MessageChange = "登录成功";
                        Text = MessageChange;
                        Data = Message;
                    }
                    else
                    {
                        Text = Message;
                    }
                    Debug.Log("接收到服务端数据:" + Message);
                    if(Message == "114514")
                    {
                        break;
                    }
                    a++;
                }
                if( i >= 20)
                {
                    break;
                }
            }
            catch (Exception ex)
            {
                Debug.Log("出现异常了！！！");
                Debug.Log(ex.Message);
                ClientSocket.Shutdown(SocketShutdown.Both);
                ClientSocket.Close();
                break;
            }
        }
        ClientSocket.Shutdown(SocketShutdown.Both);
        ClientSocket.Close();
    }
    public void Connection()
    {
        int Port = 8333;
        IPAddress IP = IPAddress.Parse("127.0.0.1");
        try
        {
            ClientSocket.Connect(new IPEndPoint(IP, Port));
            Thread thread = new Thread(ReceiveMessage);
            thread.Start();
            //Thread.Sleep(2000);
        }
        catch (Exception ex)
        {
            Debug.Log("连接服务器失败，按回车键退出！");
            Debug.Log(ex.Message);
            return;
        }
        //Debug.Log("连接服务器成功！！！");
        //int ReceiveLength = ClientSocket.Receive(result);
        //Console.WriteLine("接收服务器消息：{0}", Encoding.UTF8.GetString(result, 0, ReceiveLength));
    }
    public static bool SendMessages(String str, int i)
    {
        string Final = str + "|";
        if (i == 1)
        {
            Final += "1";
        }
        else if(i == 2)
        {
            Final += "2";
        }
        try
        {
            byte[] arrMsg = System.Text.Encoding.UTF8.GetBytes(Final);
            string Client = ClientSocket.RemoteEndPoint.ToString();
            Debug.Log("向服务器发送消息："+ str);
            ClientSocket.Send(arrMsg);
            return true;
        }
        catch (Exception ex)
        {
            ClientSocket.Shutdown(SocketShutdown.Both);
            ClientSocket.Close();
            return false;
        }
    }

    public string ShowText()
    {
        return Text;
    }

    public void SetText()
    {
        Text = null;
    }
    
    public void SingleOne()
    {
        ClientSocket.Close();
    }

    private void CloseSocket()
    {
        SendMessages("114514," + GameManager.Instance.GetID(), 0);
    }
}
