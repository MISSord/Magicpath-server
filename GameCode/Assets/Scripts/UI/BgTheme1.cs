using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BgTheme1 : MonoBehaviour
{
    private SpriteRenderer m_SpriteRenderer;
    private Image image;
    private ManagerVars vars;
    private void Start()
    {
        vars = ManagerVars.GetManagerVars();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();

        int ranValue1 = BgTheme.bgTheme.GetranValue();
        image = transform.GetComponent<Image>();
        image.sprite = vars.bgThemeSpriteList[ranValue1];

    }
}
