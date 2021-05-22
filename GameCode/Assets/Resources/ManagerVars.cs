using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Security.Permissions;
using UnityEngine;

//[CreateAssetMenu(menuName = "CreatManagerVarsContainer")]
public class ManagerVars : ScriptableObject
{
    public static ManagerVars GetManagerVars()
    {
        return Resources.Load<ManagerVars>("ManagerVarsContainer");
    }
    public List<Sprite> bgThemeSpriteList = new List<Sprite>();
    public List<Sprite> platformThemeSpriteList = new List<Sprite>();
    public List<Sprite> skinSpriteList = new List<Sprite>();
    public List<Sprite> characterSkinSpriteList = new List<Sprite>();
    public GameObject skinChooseItemPre;
    public List<string> skinNameList = new List<string>();
    /// <summary>
    /// 皮肤价格
    /// </summary>
    public List<int> skinPrice = new List<int>();
    public GameObject normalPlatformPre;
    public GameObject characterPre;
    public List<GameObject> commonPlatformGroup = new List<GameObject>();
    public List<GameObject> grassPlatformGroup = new List<GameObject>();
    public List<GameObject> winterPlatformGroup = new List<GameObject>();
    public GameObject spikePlatformLeft;
    public GameObject spikePlatformRight;
    public GameObject deathEffect;
    public GameObject diamondPre;
    public float nextXPos = 0.56f, nextYPos = 0.65f;

    public AudioClip jumpClip, fallClip, hitClip, diamondClip, buttonClip;
    public Sprite musicOn, musicOff;
}
