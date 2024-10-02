using System;
using Unity.Mathematics;
using UnityEngine;

public class LevelStats : MonoBehaviour
{
    public static LevelStats instance;
    [SerializeField] private LevelStatUI levelStatItemIU;
    [SerializeField] private GameObject upgradeItemContainerUI;

    public int enemiesKilled;
    public int currentLevel;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        Debug.Log(Time.timeSinceLevelLoad.ToString());
    }

    public void GetLevelStats()
    {
        CreateLevelStatItem("Time :", GetLevelPlayTime());
        CreateLevelStatItem("Врагов убито :", enemiesKilled.ToString());
        CreateLevelStatItem("Уровень достигнут :", currentLevel.ToString());
    }

    private string GetLevelPlayTime()
    {
        double time = Math.Round(Time.timeSinceLevelLoad, 2);
        int hTime = (int)time / 60;
        double minSecTime = time % 60;
        string hTimeStr = "";
        string munSecTimeStr = "";

        if (minSecTime<10)
        {
            munSecTimeStr = "0" + minSecTime.ToString();
        }
        else
        {
            munSecTimeStr = minSecTime.ToString();
        }

        if(minSecTime%0.01<0)
        {
            munSecTimeStr = munSecTimeStr.ToString() + "0" ;
        }

        return "0"+hTime.ToString() + ":" + munSecTimeStr.Replace(",", ":");
    }

    private void CreateLevelStatItem(string statName, string statValue)
    {
        LevelStatUI upgradeItem = Instantiate(levelStatItemIU, Vector3.zero, Quaternion.identity, upgradeItemContainerUI.transform);
        upgradeItem.InitializeItem(statName, statValue);
    }
}
