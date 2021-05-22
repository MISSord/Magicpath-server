using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverPanel : MonoBehaviour
{
    public Text txt_Score, txt_BestScore, txt_AddDiamondCount;
    public Button btn_Restart, btn_Rank, btn_Home;
    public Image img_New;

    private void Awake()
    {
        btn_Restart.onClick.AddListener(OnRestartButtonclick);
        btn_Rank.onClick.AddListener(OnRankButtonClick);
        btn_Home.onClick.AddListener(OnHomeButtonClick);
        EventCenter.AddListener(EventDefine.ShowGameOverPanel, Show);
        gameObject.SetActive(false);
        EventCenter.Broadcast(EventDefine.GameStart);
    }


    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.ShowGameOverPanel, Show);
    }

    private void Show()
    {
        if(GameManager.Instance.GetGameScore() > GameManager.Instance.GetBestScore())
        {
            img_New.gameObject.SetActive(true);
            txt_BestScore.text = "最高分 " + GameManager.Instance.GetGameScore();
        }
        else
        {
            img_New.gameObject.SetActive(false);
            txt_BestScore.text = "最高分 " + GameManager.Instance.GetBestScore();
        }
        GameManager.Instance.SaveScore(GameManager.Instance.GetGameScore());
        txt_Score.text = GameManager.Instance.GetGameScore().ToString();
        txt_AddDiamondCount.text = "+" + GameManager.Instance.GetGameDiamond().ToString();
        //更新总的钻石数量
        GameManager.Instance.UpdateDiamond(GameManager.Instance.GetGameDiamond());
        gameObject.SetActive(true);
    }
    /// <summary>
    /// 再来一次按钮点击
    /// </summary>
    private void OnRestartButtonclick()
    {
        EventCenter.Broadcast(EventDefine.PlayClikAudio);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name,LoadSceneMode.Single);
        SceneManager.LoadSceneAsync("Main", LoadSceneMode.Additive);
        EventCenter.Broadcast(EventDefine.GameStart);
        GameData.IsAgainGame = true;
    }
    /// <summary>
    /// 排行榜按钮点击
    /// </summary>
    private void OnRankButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClikAudio);
        EventCenter.Broadcast(EventDefine.ShowRankPanel);
    }
    /// <summary>
    /// 主界面按钮点击
    /// </summary>
    private void OnHomeButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClikAudio);
        EventCenter.Broadcast(EventDefine.GameStart);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name,LoadSceneMode.Single);
        SceneManager.LoadSceneAsync("Main", LoadSceneMode.Additive);
        GameData.IsAgainGame = false;
    }
}
