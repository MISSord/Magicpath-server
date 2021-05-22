using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random =  UnityEngine.Random;

public class BgTheme : MonoBehaviour
{
    public static BgTheme bgTheme;
    private SpriteRenderer m_SpriteRenderer;
    private ManagerVars vars;
    public int ranValue;
    private void Awake()
    {
        vars = ManagerVars.GetManagerVars();
        bgTheme = this;
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        ranValue = Random.Range(0, vars.bgThemeSpriteList.Count);
        m_SpriteRenderer.sprite = vars.bgThemeSpriteList[ranValue];
    }

    public int GetranValue()
    {
        return ranValue;
    }
}
