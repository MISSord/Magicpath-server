using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using Random = UnityEngine.Random;


public enum PlatformGroupType 
{
    Grass,
    Winter,
}
public class PlatformSpawner : MonoBehaviour
{
    public Vector3 startSpawnPos;
    private ManagerVars vars;

    private Vector3 platformSpawnPosition;
    private int spawnPlatformCount;
    /// <summary>
    /// 是否朝左边生成，否则反之
    /// </summary>
    private bool isLeftSpawn = false;
    /// <summary>
    /// 选择的平台图
    /// </summary>
    private Sprite selectPlatformSprite;
    /// <summary>
    /// 组合平台的类型
    /// </summary>
    private PlatformGroupType groupType;
    /// <summary>
    /// 钉子生产平台的方向是否是左边，反之右边。
    /// </summary>
    private bool spikeSpawnLeft = false;
    /// <summary>
    /// 钉子方向平台的位置
    /// </summary>
    private Vector3 spikeDirPlatformPos;
    /// <summary>
    /// 生产钉子平台之后的需要在钉子方向生产的平台数量
    /// </summary>
    private int afterSpawnSpikeSpawnCount;

    private bool isSpawnSpike;
    /// <summary>
    /// 里程碑数
    /// </summary>
    public int milestoneCount = 10;

    public float fallTime;

    public float minFallTime;

    public float multiple;

    private void Awake()
    {
        EventCenter.AddListener(EventDefine.DecidePath, DecidePath);
        vars = ManagerVars.GetManagerVars();
    }

    public void Start()
    {
        RandomPlatformTheme();
        platformSpawnPosition = startSpawnPos;

        for (int i = 0; i < 5; i++)
        {
            spawnPlatformCount = 5;
            DecidePath();
        }
        //生产人物
        GameObject goes = Instantiate(vars.characterPre);
        goes.transform.position = new Vector3(0, 0.7f, 0);
    }

    private void Update()
    {
        if(GameManager.Instance.IsGameStarted && GameManager.Instance.IsGameOver == false)
        {
            UpdateFallTime();
        }
    }
    /// <summary>
    /// 更新平台掉落时间
    /// </summary>
    private void UpdateFallTime()
    {
        if(GameManager.Instance.GetGameScore() > milestoneCount)
        {
            milestoneCount *= 2;
            fallTime *= multiple;
            if(fallTime < minFallTime)
            {
                fallTime = minFallTime;
            }
        }
    }
    private void RandomPlatformTheme()
    {
        int ran = Random.Range(0, vars.platformThemeSpriteList.Count);
        selectPlatformSprite = vars.platformThemeSpriteList[ran];

        if(ran == 3)
        {
            groupType = PlatformGroupType.Winter;
        }
        else
        {
            groupType = PlatformGroupType.Grass;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    
    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.DecidePath, DecidePath);
    }

    private void DecidePath()
    {
        if (isSpawnSpike)
        {
            AfterSpawnSpike();
            return;
        }
        if(spawnPlatformCount >= 0)
        {
            SpawnPlatform();
            spawnPlatformCount--;
        }
        else
        {
            //旋转方向
            isLeftSpawn = !isLeftSpawn ;
            spawnPlatformCount = Random.Range(1, 4);
            SpawnPlatform();
        }
    }
    /// <summary>
    /// 生产平台
    /// </summary>
    private void SpawnPlatform()
    {
        int ranObstacleDir = Random.Range(0, 2);
        //生产单个平台
        if(spawnPlatformCount >= 1)
        {
           SpawnNormalPlatform(ranObstacleDir);
        }
        //生成组合平台
        else if(spawnPlatformCount == 0)
        {
            int ran = Random.Range(0, 3);
            //生产通用组合平台
            if(ran == 0)
            {
                SpawnCommonPlatformGroup(ranObstacleDir);
            }
            //生产主题组合平台
            else if(ran == 1)
            {
                switch (groupType)
                {
                    case PlatformGroupType.Grass: 
                        SpawnGrassPlatformGroup(ranObstacleDir);
                        break;
                    case PlatformGroupType.Winter:
                        SpawnWinterPlatformGroup(ranObstacleDir);
                        break;
                    default:
                        break;
                }
            }
            //生产钉子平台
            else
            {
                int value = -1;
                if (isLeftSpawn)
                {
                    value = 0;//生产右边的钉子
                }
                else
                {
                    value = 1;//生产左边的钉子
                }
                SpawnSpikePlatform(value);

                isSpawnSpike = true;
                afterSpawnSpikeSpawnCount = 4;

                if (spikeSpawnLeft)//钉子在左边
                {
                    spikeDirPlatformPos = new Vector3(platformSpawnPosition.x - 1.65f,
                        platformSpawnPosition.y + vars.nextYPos, 0);
                }
                else
                {
                    spikeDirPlatformPos = new Vector3(platformSpawnPosition.x + 1.65f,
                        platformSpawnPosition.y + vars.nextYPos, 0);
                }
            }
        }
        int ranSpawnDiamond = Random.Range(0, 8);
        if(ranSpawnDiamond >= 6 && GameManager.Instance.PlayerIsMove)
        {
            GameObject go = ObjectPool.Instance.GetDiamond();
            go.transform.position = new Vector3(platformSpawnPosition.x,
                platformSpawnPosition.y + 0.5f, 0);
            go.SetActive(true);
        }
        if (isLeftSpawn)
        {
            platformSpawnPosition = new Vector3(platformSpawnPosition.x - vars.nextXPos, 
                platformSpawnPosition.y + vars.nextYPos, 0);
        }
        else
        {
            platformSpawnPosition = new Vector3(platformSpawnPosition.x + vars.nextXPos,
                platformSpawnPosition.y + vars.nextYPos, 0);
        }
    }

    /// <summary>
    /// 生产普通的平台
    /// </summary>
    private void SpawnNormalPlatform(int ranObstacleDir)
    {
        GameObject go = ObjectPool.Instance.GetNormalPlatform();
        go.transform.position = platformSpawnPosition;
        go.GetComponent<PlatformScript>().Init(selectPlatformSprite, fallTime, ranObstacleDir);
        go.SetActive(true);
    }
    /// <summary>
    /// 生产通用组合平台
    /// </summary>
    private void SpawnCommonPlatformGroup(int ranObstacleDir)
    {

        GameObject go = ObjectPool.Instance.GetCommonPlatformGroup();
        go.transform.position = platformSpawnPosition;
        go.GetComponent<PlatformScript>().Init(selectPlatformSprite, fallTime, ranObstacleDir);
        go.SetActive(true);
    }

    /// <summary>
    /// 生产草地组合平台
    /// </summary>
    private void SpawnGrassPlatformGroup(int ranObstacleDir)
    {
        GameObject go = ObjectPool.Instance.GetGrassPlatformGroup();
        go.transform.position = platformSpawnPosition;
        go.GetComponent<PlatformScript>().Init(selectPlatformSprite, fallTime,ranObstacleDir);
        go.SetActive(true);
    }

    /// <summary>
    /// 生产冬季组合平台
    /// </summary>
    private void SpawnWinterPlatformGroup(int ranObstacleDir)
    {
        GameObject go = ObjectPool.Instance.GetWinterPlatformGroup();
        go.transform.position = platformSpawnPosition;
        go.GetComponent<PlatformScript>().Init(selectPlatformSprite, fallTime, ranObstacleDir);
        go.SetActive(true);
    }
    /// <summary>
    /// 生产钉子的组合平台
    /// </summary>
    /// <param name="dir"></param>
    private void SpawnSpikePlatform(int dir)
    {
        GameObject temp = null;
        if(dir == 0)
        {
            spikeSpawnLeft = false;
            temp = ObjectPool.Instance.GetRightSpikePlatform();
        }
        else
        {
            spikeSpawnLeft = true;
            temp = ObjectPool.Instance.GetLeftSpikePlatform();
        }
            temp.transform.position = platformSpawnPosition;
            temp.GetComponent<PlatformScript>().Init(selectPlatformSprite, fallTime, dir);
            temp.SetActive(true);   
    }
    /// <summary>
    /// 生产钉子平台之后需要生产的平台
    /// 包括钉子方向，原来的方向
    /// </summary>
    private void AfterSpawnSpike()
    {
        if(afterSpawnSpikeSpawnCount > 0)
        {
            afterSpawnSpikeSpawnCount--;
            for(int i = 0; i < 2; i++)
            {
                GameObject temp = ObjectPool.Instance.GetNormalPlatform();
                if(i == 0)//生产原来方向的平台
                {
                    temp.transform.position = platformSpawnPosition;
                    //如果钉子在左边
                    if (spikeSpawnLeft)
                    {
                        platformSpawnPosition = new Vector3(platformSpawnPosition.x + vars.nextXPos,
                            platformSpawnPosition.y + vars.nextYPos, 0);
                    }
                    else
                    {
                        platformSpawnPosition = new Vector3(platformSpawnPosition.x - vars.nextXPos,
                            platformSpawnPosition.y + vars.nextYPos, 0);
                    }
                }
                else//钉子方向的平台
                {
                    temp.transform.position = spikeDirPlatformPos;
                    if (spikeSpawnLeft)
                    {
                        spikeDirPlatformPos = new Vector3(spikeDirPlatformPos.x - vars.nextXPos,
                            spikeDirPlatformPos.y + vars.nextYPos, 0);
                    }
                    else
                    {
                        spikeDirPlatformPos = new Vector3(spikeDirPlatformPos.x + vars.nextXPos,
                            spikeDirPlatformPos.y + vars.nextYPos, 0);
                    }
                }
                temp.GetComponent<PlatformScript>().Init(selectPlatformSprite, fallTime, 1);
                temp.SetActive(true);
            }
        }
        else
        {
            isSpawnSpike = false;
            DecidePath();
        }
    }
}
