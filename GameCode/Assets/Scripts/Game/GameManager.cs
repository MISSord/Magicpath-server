using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.IO;
using System.Linq;


public class GameManager : MonoBehaviour
{
    public int IsFirstTime = 0;
    public static GameManager Instance;
    public GameData data;
    private ManagerVars vars;
    public bool IsGameStarted { get; set; }
    public bool IsGameOver { get; set; }
    public bool IsPause { get; set; }
    /// <summary>
    /// 玩家是否开始移动
    /// </summary>
    public bool PlayerIsMove { get; set; }
    public int gameScore;
    public int gameDiamond;

    private bool isFirstGame;
    private bool isMusicOn;
    private int[] bestScoreArr;
    private int selectSkin;
    private bool[] skinUnlocked;
    private int diamondCount;

    private string ID;
    private string PassWord;

    private string temporary;

    public Client ClientOne;

    private void Awake()
    {
        vars = ManagerVars.GetManagerVars();
        Instance = this;
        EventCenter.AddListener(EventDefine.AddScore, AddGameScore);
        EventCenter.AddListener(EventDefine.PlayerMove, PlayerMove);
        EventCenter.AddListener(EventDefine.AddDiamond, AddGameDiamond);
        EventCenter.AddListener(EventDefine.GameStart, GameStart);

        ClientOne = Client._instance;

        if (GameData.IsAgainGame)
        {
            IsGameStarted = true;
        }
        InitGameData();
    }

    private void GameStart()
    {
        PlayerIsMove = false;
        IsGameOver = false;
        gameDiamond = 0;
        gameScore = 0;
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.AddScore, AddGameScore);
        EventCenter.RemoveListener(EventDefine.PlayerMove, PlayerMove);
        EventCenter.RemoveListener(EventDefine.AddDiamond, AddGameDiamond);
        EventCenter.RemoveListener(EventDefine.GameStart, GameStart);
    }
    /// <summary>
    /// 玩家移动会调用到此方法
    /// </summary>
    private void PlayerMove()
    {
        PlayerIsMove = true;
    }
    /// <summary>
    /// 保存成绩
    /// </summary>
    public void SaveScore(int score)
    {
        List<int> list = bestScoreArr.ToList();
        list.Sort((x, y) => (-x.CompareTo(y)));
        bestScoreArr = list.ToArray();

        int index = -1;
        for (int i = 0; i < bestScoreArr.Length; i++)
        {
            if (score > bestScoreArr[i])
            {
                index = i;
                break;
            }
        }

        if (index == -1) return;

        for (int i = bestScoreArr.Length - 1; i > index; i--)
        {
            bestScoreArr[i] = bestScoreArr[i - 1];
        }
        bestScoreArr[index] = score;
        Save();
        EventCenter.Broadcast<int>(EventDefine.SendMessage, 3);
    }
    /// <summary>
    /// 获取最高分
    /// </summary>
    /// <returns></returns>
    public int GetBestScore()
    {
        return bestScoreArr.Max();
    }
    /// <summary>
    /// 获得最高分数数组
    /// </summary>
    /// <returns></returns>
    public int[] GetScoreArr()
    {
        List<int> list = bestScoreArr.ToList();
        //从大到小排序list
        list.Sort((x, y) => (-x.CompareTo(y)));
        bestScoreArr = list.ToArray();

        return bestScoreArr;
    }
    /// <summary>
    /// 增加游戏成绩
    /// </summary>
    private void AddGameScore()
    {
        if (IsGameStarted == false || IsGameOver || IsPause)
        {
            return;
        }
        gameScore++;
        EventCenter.Broadcast(EventDefine.UpdateScoreText, gameScore);
    }
    /// <summary>
    /// 获取游戏成绩
    /// </summary>
    public int GetGameScore()
    {
        return gameScore;
    }
    /// <summary>
    /// 更新游戏钻石数量
    /// </summary>
    private void AddGameDiamond()
    {
        gameDiamond++;
        EventCenter.Broadcast(EventDefine.UpdateDiamondText, gameDiamond);
    }
    /// <summary>
    /// 吃到的钻石数
    /// </summary>
    /// <returns></returns>
    public int GetGameDiamond()
    {
        return gameDiamond;
    }
    /// <summary>
    /// 获取当前皮肤是否解锁
    /// </summary>
    /// <returns></returns>
    public bool GetSkinUnlocked(int index)
    {
        return skinUnlocked[index];
        //return true;
    }
    /// <summary>
    /// 设置当前皮肤解锁
    /// </summary>
    /// <param name="index"></param>
    public void SetSkinUnlocked(int index)
    {
        skinUnlocked[index] = true;
        Save();
        EventCenter.Broadcast<int>(EventDefine.SendMessage, 5);
    }
    /// <summary>
    /// 当前选择的皮肤
    /// </summary>
    /// <returns></returns>
    public int GetCurrentSelectedSkin()
    {
        return selectSkin;
    }

    public bool[] GetSkinUnlock()
    {
        return skinUnlocked;
    }
    public int GetAllDiamond()
    {
        return diamondCount;
    }
    /// <summary>
    /// 更新总钻石数量
    /// </summary>
    /// <param name="value"></param>
    public void UpdateDiamond(int value)
    {
        diamondCount += value;
        Save();
        EventCenter.Broadcast<int>(EventDefine.SendMessage, 4);
    }
    /// <summary>
    /// 设置当前皮肤的皮肤的下标
    /// </summary>
    /// <param name="index"></param>
    public void SetSelectedSkin(int index)
    {
        selectSkin = index;
        Save();
        EventCenter.Broadcast<int>(EventDefine.SendMessage, 6);
    }

    /// <summary>
    /// 设置音效是否开启
    /// </summary>
    /// <param name="value"></param>
    public void SetIsMusicOn(bool value)
    {
        isMusicOn = value;
        Save();
    }
    /// <summary>
    /// 获取音效是否开启
    /// </summary>
    public bool GetIsMusicOn()
    {
        return isMusicOn;
    }

    public void SetID(string IDOne)
    {
        ID = IDOne;
        Save();
    }

    public void SetPassWord(string password)
    {
        PassWord = password;
        Save();
    }

    public void SetTemporary(string TM)
    {
        this.temporary = TM;
    }

    public string GetTemporary()
    {
        return temporary;
    }

    public string GetID()
    {
        return ID;
    }

    public string GetPassWord()
    {
        return PassWord;
    }

    private void InitGameData()
    {
        if (data != null)
        {
            Read();
            isFirstGame = data.GetIsFirstGame();
        }

        //isFirstGame = true;

        ///第一次开始游戏
        if (isFirstGame)
        {
            isFirstGame = false;
            isMusicOn = true;
            bestScoreArr = new int[3];
            selectSkin = 0;
            skinUnlocked = new bool[vars.skinSpriteList.Count];
            for (int i = 0; i < skinUnlocked.Length; i++)
            {
                skinUnlocked[i] = false;
                Debug.Log(i);
            }
            diamondCount = 10;
            data = new GameData();
            Save();
        }
        else
        {
            isMusicOn = data.GetIsMusicOn();
            bestScoreArr = data.GetBestScoreArr();
            selectSkin = data.GetSelectSkin();
            skinUnlocked = data.GetSkinUnlocked();
            diamondCount = data.GetDiamondCount();
            ID = data.GetID();
            PassWord = data.GetPassWord();
        }
    }
    /// <summary>
    /// 储存数据
    /// </summary>
    private void Save()
    {
        //string PlayOne = 
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream fs = File.Create(Application.persistentDataPath + "/GameData.data"))
            {
                data.SetBestScoreArr(bestScoreArr);
                data.SetDiamondCount(diamondCount);
                data.SetIsFirstGame(isFirstGame);
                data.SetIsMusicOn(isMusicOn);
                data.SetSelectSkin(selectSkin);
                data.SetSkinUnlocked(skinUnlocked);
                data.SetID(ID);
                data.SetPassWord(PassWord);
                bf.Serialize(fs, data);
            }
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    private void Read()
    {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream fs = File.Open(Application.persistentDataPath + "/GameData.data", FileMode.Open))
            {
                data = (GameData)bf.Deserialize(fs);
            }
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
        }
    }
    /// <summary>
    /// 重置数据
    /// </summary>
    public void ResetData()
    {
        isFirstGame = false;
        isMusicOn = true;
        bestScoreArr = new int[3]{0,0,0};
        selectSkin = 0;
        skinUnlocked = new bool[vars.skinSpriteList.Count];
        skinUnlocked[0] = true;
        diamondCount = 10;
        Save();
    }

    public void SpliteData(string Message)
    {
        string[] stringArrFirst = Message.Split('|');
        int s = 0;
        foreach (string Ina in stringArrFirst)
        {
            if (Ina == "" || Ina == null)
            {
                break;
            }
            Debug.Log(Ina);
            string[] stringArr = Ina.Split(',');
            int a = stringArr.Length;
            Debug.Log("切分长度：" + a);
            if(s == 0)
            {
                int DiamondCount = int.Parse(stringArr[1]);
                diamondCount = DiamondCount;
                bestScoreArr[0] = int.Parse(stringArr[2]);
                bestScoreArr[1] = int.Parse(stringArr[3]);
                bestScoreArr[2] = int.Parse(stringArr[4]);
            }
            if(s == 1)
            {
                int aaa = 0;
                selectSkin = int.Parse(stringArr[1]);
                Debug.Log("selectSkin:"+ selectSkin);
                for(int aa = 2; aa < stringArr.Length; aa++)
                {
                    if (stringArr[aa] == "true" || stringArr[aa] == "true      " || stringArr[aa].Contains("true"))
                    {
                        bool IsTrue = true;
                        skinUnlocked[aaa] = IsTrue;
                    }
                    else if(stringArr[aa].Contains("false"))
                    {
                        bool IsTrue = false;
                        skinUnlocked[aaa] = IsTrue;
                    }
                    
                    Debug.Log(aaa + ":"+skinUnlocked[aaa] + "," +stringArr[aa]);
                    aaa++;
                }
            }
            Debug.Log("执行次数:" + s++);
        }
        Save();
        Debug.Log("数据更新成功");
    }
}
