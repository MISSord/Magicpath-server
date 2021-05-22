using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG;

public class TipsPanel : MonoBehaviour
{
    public Text Tips;
    public GameObject TipsPanels;

    public void Awake()
    {
        EventCenter.AddListener<string>(EventDefine.ShowTipsPanel, ShowTipsPanel);
    }

    public void OnDestroy()
    {
        EventCenter.RemoveListener<string>(EventDefine.ShowTipsPanel, ShowTipsPanel);
    }
    // Start is called before the first frame update
    void Start()
    {
        Tips = transform.GetChild(0).GetComponent<Text>();
        TipsPanels = transform.gameObject;
        TipsPanels.SetActive(false);
    }

    public void ShowTipsPanel(string TipsPart)
    {
        Tips.text = TipsPart;
        TipsPanels.SetActive(true);
        StartCoroutine(DisapperThis());
    }

    IEnumerator DisapperThis()
    {
        yield return new WaitForSeconds(2.0f);
        TipsPanels.SetActive(false);
    }
}
