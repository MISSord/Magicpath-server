using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System;
using UnityEngine.SceneManagement;
using static UnityEngine.Object;

public class LoginPanel : MonoBehaviour
{
    public Button LoginButton;
    public Button RegisteredButton;
    public Button TrueRegisterButtn;
    public Button TurnBack;
    public Text IDText;
    public Text PassWordText;

    public Text IDTextOne;
    public Text PassWordOne;
    public Text PassWordTwo;

    public string id;
    public string PassWord;

    public string ID;
    public string FirstPassWord;
    public string CheckPassWord;

    public GameObject LoginPart;
    public GameObject RegisteredPart;

    public GameObject LoginPanels;
    public GameObject GameMangerOne;
    public GameObject TipPanel;

    public Button IsSingle;

    public string Text;

    private void Awake()
    {
        EventCenter.AddListener(EventDefine.Login, LoginButtonClick);
        EventCenter.AddListener(EventDefine.Registered, RegisteredButtonClick);
        EventCenter.AddListener(EventDefine.ShowLoginPart, ShowLoginPart);
        EventCenter.AddListener(EventDefine.ShowRegisteredPart, ShowRegisteredPart);
        EventCenter.AddListener(EventDefine.DonShowLoginPart, DonShowLoginPanel);
        EventCenter.AddListener(EventDefine.ShowMainPanelOne, ShowMainPanel);
        GameMangerOne = GameObject.Find("GameManager");
        TipPanel = GameObject.Find("TipsPanel");
        Init();
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.Registered, RegisteredButtonClick);
        EventCenter.RemoveListener(EventDefine.Login, LoginButtonClick);
        EventCenter.RemoveListener(EventDefine.ShowLoginPart, ShowLoginPart);
        EventCenter.RemoveListener(EventDefine.ShowRegisteredPart, ShowRegisteredPart);
        EventCenter.RemoveListener(EventDefine.DonShowLoginPart, DonShowLoginPanel);
        EventCenter.RemoveListener(EventDefine.ShowMainPanelOne, ShowMainPanel);
    }

    private void Start()
    {
        if (GameData.IsAgainGame)
        {
            gameObject.SetActive(false);
        }
        LoginPart.SetActive(true);
        RegisteredPart.SetActive(false);
    }

    // Start is called before the first frame update
    public void Init()
    {
        LoginButton = transform.Find("LoginPart/Login").GetComponent<Button>();
        RegisteredButton = transform.Find("LoginPart/Registered").GetComponent<Button>();
        TrueRegisterButtn = transform.Find("RegisteredPart/Registered").GetComponent<Button>();
        TurnBack = transform.Find("RegisteredPart/TurnBack").GetComponent<Button>();
        IsSingle = transform.Find("LoginPart/IsSingle").GetComponent<Button>();

        IDText = transform.Find("LoginPart/ID/Text").GetComponent<Text>();
        PassWordText = transform.Find("LoginPart/PassWord/Text").GetComponent<Text>();
        IDTextOne = transform.Find("RegisteredPart/ID/Text").GetComponent<Text>();
        PassWordOne = transform.Find("RegisteredPart/PassWord/Text").GetComponent<Text>();
        PassWordTwo = transform.Find("RegisteredPart/PassWordTwo/Text").GetComponent<Text>();

        LoginPart = transform.Find("LoginPart").gameObject;
        RegisteredPart = transform.Find("RegisteredPart").gameObject;
        LoginPanels = transform.gameObject;

        LoginButton.onClick.AddListener(LoginButtonClick);
        RegisteredButton.onClick.AddListener(ShowRegisteredPart);
        TrueRegisterButtn.onClick.AddListener(RegisteredButtonClick);
        TurnBack.onClick.AddListener(ShowLoginPart);
        IsSingle.onClick.AddListener(SingleGame);
    }

    private void LoginButtonClick()
    {
        Debug.Log("点击成功");
        id = IDText.text;
        PassWord = PassWordText.text;
        string Text;

        if (id != "" && PassWord != "")
        {
            bool isSixOne = CheckNumble(id);
            if (isSixOne)
            {
                GameManager.Instance.SetID(id);   
            }
            else
            {
                Text = "ID只能是六位数";
                EventCenter.Broadcast<String>(EventDefine.ShowTipsPanel,Text);
                return;
            }
            bool isSixTwo = CheckNumble(PassWord);
            if (isSixTwo)
            {
                GameManager.Instance.SetPassWord(PassWord);
            }
            else
            {
                Text = "密码只能是六位数";
                EventCenter.Broadcast<String>(EventDefine.ShowTipsPanel, Text);
                return;
            }
            if(isSixOne && isSixTwo)
            {
                Debug.Log("设置ID和密码成功");
                EventCenter.Broadcast<int>(EventDefine.SendMessage,1);
                
                return;
            }
        }
        else if(id == "" || id == null)
        {
            Text = "ID不能为空";
            EventCenter.Broadcast<String>(EventDefine.ShowTipsPanel, Text);
            return;
        }
        else if(PassWord == "" || PassWord == null)
        {
            Text = "密码不能为空";
            EventCenter.Broadcast<String>(EventDefine.ShowTipsPanel, Text);
            return;
        }
    }
    IEnumerator LoadAsync()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync("GamePart", LoadSceneMode.Single);
        SceneManager.LoadSceneAsync("Main",LoadSceneMode.Additive);
        while(operation.isDone == false)
        {
            float progress = Mathf.Clamp01(operation.progress/0.9f);
            Debug.Log(progress);
            if(progress >= 0.9)
            {
                gameObject.SetActive(false);
                UnityEngine.Object.DontDestroyOnLoad(GameMangerOne);
                UnityEngine.Object.DontDestroyOnLoad(TipPanel);
            }
            yield return null;
        }
        SceneManager.UnloadSceneAsync("Login");
    }

    private void OpenScence()
    {
        throw new NotImplementedException();
    }

    private bool CheckNumble(string Message)
    {
        int a = int.Parse(Message);
        int i = a.ToString().Length;
        if(i != 6)
        {
            return false;
        }
        return true;
    }

    private void ShowRegisteredPart()
    {
        LoginPart.SetActive(false);
        RegisteredPart.SetActive(true);
    }

    private void ShowLoginPart()
    {
        LoginPart.SetActive(true);
        RegisteredPart.SetActive(false);
    }

    private void RegisteredButtonClick()
    {
        string Text;
        ID = IDTextOne.text;
        FirstPassWord = PassWordOne.text;
        CheckPassWord = PassWordTwo.text;

        if (ID == null || ID == "")
        {
            Text = "ID不能为空";
            EventCenter.Broadcast<String>(EventDefine.ShowTipsPanel, Text);
            return;
        }
        bool isSixOne = CheckNumble(ID);
        if (!isSixOne)
        {
            Text = "ID只能是六位数";
            EventCenter.Broadcast<String>(EventDefine.ShowTipsPanel, Text);
            return;
        }
        if (FirstPassWord == "" || FirstPassWord == null)
        {
            Text = "密码不能为空";
            EventCenter.Broadcast<String>(EventDefine.ShowTipsPanel, Text);
            return;
        }
        bool isSixTwo = CheckNumble(FirstPassWord);
        if (!isSixTwo)
        {
            Text = "密码只能是六位数";
            EventCenter.Broadcast<String>(EventDefine.ShowTipsPanel, Text);
            return;
        }
        if (CheckPassWord == "" || CheckPassWord == null)
        {
            Text = "确认密码不能为空";
            EventCenter.Broadcast<String>(EventDefine.ShowTipsPanel, Text);
            return;
        }
        else if(FirstPassWord != CheckPassWord)
        {
            Text = "密码和确认密码需要一致";
            EventCenter.Broadcast<String>(EventDefine.ShowTipsPanel, Text);
            return;
        }

        string str = ID + "," + FirstPassWord;
        GameManager.Instance.SetTemporary(str);
        Debug.Log(str);
        EventCenter.Broadcast<int>(EventDefine.SendMessage,2);
        
    }

    private void DonShowLoginPanel()
    {
        StartCoroutine(Disapper());
    }

    IEnumerator Disapper()
    {
        yield return new WaitForSeconds(2.0f);
        LoginPanels.SetActive(false);
    }
    private void ShowLoginPanel()
    {
        LoginPanels.SetActive(false);
    }

    private void ShowMainPanel()
    {
        StartCoroutine(LoadAsync());
    }

    private void SingleGame()
    {
        EventCenter.Broadcast(EventDefine.SingleGame);
    }
}
