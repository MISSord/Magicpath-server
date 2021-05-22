using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using UnityEngine;

public class SendManager : MonoBehaviour
{
    public SendManager Send_Manager;
    
    public SendManager()
    {
        Send_Manager = this;
    }

    public void Awake()
    {
        Send_Manager = this;
        EventCenter.AddListener<int>(EventDefine.SendMessage, SendManagerThread);   
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener<int>(EventDefine.SendMessage, SendManagerThread);
    }

    public String MessageDiamond(int DiamondCount, string ID)
    {
        string str = "DiamondCoount," +DiamondCount.ToString() + ",ID," + ID;
        return str;
    }

    public String MessageID(string ID, string PassWord)
    {
        string str = "ID," + ID + ",PassWord," + PassWord + ",Time," + DateTime.Now.ToString() +",Name," +"ming";
        return str;
    }

    public String SelectSkin(int SelectSkin, string ID)
    {
        string str = "SelectSkin," + SelectSkin.ToString()+ ",ID" + ID;
        return str;
    }

    public String UpdateBestCore(int BestScore)
    {
        string str = null;
        return str;
    }

    private void SendManagerThread(int i)
    {
        Debug.Log("选择发送消息函数: " + i.ToString());
        Thread Send = null;
        switch (i)
        {
            case 1: Send = new Thread(SendMessageOne); break;
            case 2: Send = new Thread(SendMessageTwo); break;
            case 6: Send = new Thread(SendMessageSix); break;
            case 4: Send = new Thread(SendMessageFour); break;
            case 5: Send = new Thread(SendMessageFive);break;
            case 3: Send = new Thread(SendMessageThree);break;
        }
        Send.Start();
        ShowText();
    }

    private void ShowText()
    {
        string sendText = Client._instance.ShowText();
        if(sendText == "" || sendText == null){ return; }
        EventCenter.Broadcast<string>(EventDefine.ShowTipsPanel, sendText);
        if (sendText == "登录成功")
        {
            EventCenter.Broadcast(EventDefine.DonShowLoginPart);
            GameManager.Instance.SpliteData(Client._instance.Data);
            EventCenter.Broadcast(EventDefine.ShowMainPanelOne);
            Debug.Log("加载主场景");
        }
        new WaitForSeconds(2.0f);
        Client._instance.SetText();
    }
    string SendSuccess = null;
    private void SendMessageThree()
    {
        SendSuccess = "a";
        int[] ScoreArr = GameManager.Instance.GetScoreArr();
        string str = null;
        for(int i = 0; i < ScoreArr.Length; i++)
        {
            switch(i)
            {
                case 0: str = "BestScoreOne";break;
                case 1: str = "BestScoreSecond";break;
                case 2: str = "BestScoreThird";break;
            }
            string strOne = str + "," + ScoreArr[i].ToString() + ",ID," + GameManager.Instance.GetID();
            bool isSend = Client.SendMessages(strOne, 0);
        }
        EventCenter.Broadcast<string>(EventDefine.ShowTipsPanel, "发送成绩成功");
        SendSuccess = null;
    }

    //更新砖石总数量
    private void SendMessageFour()
    {
        while (SendSuccess == "a")
        {
            continue;
        }
        int Diamond = GameManager.Instance.GetAllDiamond();
        string str = "DiamondCount," + Diamond.ToString() + ",ID," + GameManager.Instance.GetID();
        bool ISsend = Client.SendMessages(str, 0);
    }
    //发送皮肤解锁数据
    private void SendMessageFive()
    {
        SendSuccess = "a";
        bool[] SkinArr = GameManager.Instance.GetSkinUnlock();
        string str = null;
        for (int i = 0; i < SkinArr.Length; i++)
        {
            switch (i)
            {
                case 0: str = "SkinFirst"; break;
                case 1: str = "SkinTwo"; break;
                case 2: str = "SkinThird"; break;
                case 3: str = "SkinFour"; break;
            }
            string strOne = str + "," + SkinArr[i].ToString() + ",ID," + GameManager.Instance.GetID();
            bool isSend = Client.SendMessages(strOne, 0);
        }
        EventCenter.Broadcast<string>(EventDefine.ShowTipsPanel, "发送皮肤数据成功");
        SendSuccess = null;
    }
    //发送选择皮肤数据
    private void SendMessageSix()
    {
        int index = GameManager.Instance.GetCurrentSelectedSkin();
        string str = "SelectSkin," + index.ToString() + ",ID," + GameManager.Instance.GetID();
        bool ISsend = Client.SendMessages(str, 0);
    }
    //注册事件
    private void SendMessageTwo()
    {
        string str = GameManager.Instance.GetTemporary();
        string[] arr = str.Split(',');
        string strOne = MessageID(arr[0], arr[1]);
        bool isSend = Client.SendMessages(strOne, 1);
    }

    int a = 0;
    float i = 0;
    //发送登录事件
    public void SendMessageOne()
    {
        a++;
        Debug.Log(a);
        if (i >= 3)
        {
            i = 0;
            a = 0;
        }
        if (a >= 1 && i <= 3)
        {
            return;
        }
        string ID = GameManager.Instance.GetID();
        string PassWord = GameManager.Instance.GetPassWord();
        string strOne = MessageID(ID, PassWord);
        bool Istrue = Client.SendMessages(strOne, 2);
        if (Istrue == true)
        {
            Debug.Log("发生id数据成功");
        }
        else
        {
            Debug.Log("发送失败");
        }
    }

    public void SendMessageZero()
    {
        string strOne = "LoseConnection";
        Client.SendMessages(strOne, 0);
    }

    int aa = 0;
    private void Update()
    {
        i += Time.deltaTime;
        if (aa != Client._instance.a)
        {
            ShowText();
            aa = Client._instance.a;
        }
    }
}
