using System.IO;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using YG;
using UnityEngine.Events;

public class StatUpgradeManager : MonoBehaviour
{
    public StatUpgradeData[] statUpgrades;       // Массив всех доступных улучшений
    [SerializeField] private TMP_Text currentGoldText;
    [SerializeField] private UpgradeValues upgradeValues;
    public int currentGold { get; private set; }
    private PlayerData currentPlayerData = new PlayerData();
    public UnityEvent refreshShop = new UnityEvent();


    private static StatUpgradeManager instance;

    public static StatUpgradeManager Instance { get { return instance; } }

    private void Awake()
    {
        if (FindObjectsOfType(typeof(GameLogic)).Length > 1)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    private void Start()
    {
        // Загрузка данных улучшений из сохранений JSON
        LoadUpgrades();
        currentGold = YandexGame.savesData.gold;

        if (currentGoldText != null)
        {
            currentGoldText.text = currentGold.ToString();
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            
            PlayerPrefs.DeleteAll();
            YandexGame.ResetSaveProgress();
        }
    }

    public void AddGold(int gold)
    {
        currentGold += gold;
        if (currentGoldText != null)
        {
            currentGoldText.text = currentGold.ToString();
        }
        refreshShop.Invoke();

        YandexGame.savesData.gold = currentGold;
        YandexGame.SaveProgress();
    }

    public void RemoveGold(int gold)
    {
        if (currentGold >= gold && currentGoldText != null)
        {
            currentGold -= gold;
            currentGoldText.text = currentGold.ToString();
        }
        YandexGame.savesData.gold = currentGold;
        YandexGame.SaveProgress();
        
    }

    public void UpgradeStat(int index)
    {
        if (index < 0 || index >= statUpgrades.Length) return;

        StatUpgradeData upgrade = statUpgrades[index];
        if (upgrade.currentLevel < upgrade.maxLevel)
        {
            RemoveGold(GetUpgradeCost(upgrade));
            upgrade.currentLevel++;
            ApplyUpgrade(upgrade);
            SaveUpgrades();
        }

        YandexGame.savesData.gold = currentGold;
        YandexGame.SaveProgress();

    }

    public int TryGetUpgradeCost(int index)
    {
        if (index < 0 || index >= statUpgrades.Length) return 0;

        StatUpgradeData upgrade = statUpgrades[index];
        double actualUpgradeCost = upgrade.baseUpgradeCost * Mathf.Pow(upgrade.upgradeCostMulti, upgrade.currentLevel);

        if (upgrade.currentLevel < upgrade.maxLevel)
        {
            return (int)actualUpgradeCost;
        }

        return 0;
    }

    private int GetUpgradeCost(StatUpgradeData upgrade)
    {
        return (int)(upgrade.baseUpgradeCost * Mathf.Pow(upgrade.upgradeCostMulti, upgrade.currentLevel));
    }

    private void ApplyUpgrade(StatUpgradeData upgrade)
    {
        PlayerData upgradedData = LoadPlayerStats();
        switch (upgrade.statName)
        {
            case "Health":
                Debug.Log("UpgradeHealth");
                upgradedData.playerMaxHealth += upgradeValues.playerMaxHealth;
                break;
            case "MovementSpeed":
                upgradedData.playerMoveSpeedModifier += upgradeValues.playerMoveSpeedModifier;
                break;
            case "WeaponDamageMulti":
                upgradedData.weaponDamageModifier += upgradeValues.weaponDamageModifier;
                break;
            case "activeItemsCooldown":
                upgradedData.activeItemsCooldown += upgradeValues.activeItemsCooldown;
                break;
            case "experienceGainModifier":
                upgradedData.experienceGainModifier += upgradeValues.experienceGainModifier;
                break;
            case "playerArmor":
                upgradedData.playerArmor += upgradeValues.playerArmor;
                break;
            case "playerHealthRegen":
                upgradedData.playerHealthRegen += upgradeValues.playerHealthRegen;
                break;
            case "experienceTakingRange":
                upgradedData.experienceTakingRange += upgradeValues.experienceTakingRange;
                break;

        }
        SavePlayerStats(upgradedData);
    }

    // Загрузка уровней улучшений из JSON-файла
    private void LoadUpgrades()
    {

        if (YandexGame.savesData.upgradeLevels != "")
        {
            string json = YandexGame.savesData.upgradeLevels;
            StatUpgradeSaveWrapper savedData = JsonUtility.FromJson<StatUpgradeSaveWrapper>(json);

            foreach (var savedUpgrade in savedData.upgrades)
            {
                foreach (var upgrade in statUpgrades)
                {
                    if (upgrade.statName == savedUpgrade.statName)
                    {
                        upgrade.currentLevel = savedUpgrade.currentLevel;
                        break;
                    }
                }
            }

            Debug.Log("Upgrade levels loaded from JSON.");
        }
        else
        {
            foreach (var upgrade in statUpgrades)
            {
                upgrade.currentLevel = 0;
            }
            Debug.Log("No save file found, using default upgrade levels.");
        }
    }

    // Сохранение уровней улучшений в JSON-файл
    private void SaveUpgrades()
    {
        List<StatUpgradeSaveData> saveDataList = new List<StatUpgradeSaveData>();

        foreach (var upgrade in statUpgrades)
        {
            StatUpgradeSaveData saveData = new StatUpgradeSaveData
            {
                statName = upgrade.statName,
                currentLevel = upgrade.currentLevel
            };
            saveDataList.Add(saveData);
        }

        StatUpgradeSaveWrapper saveWrapper = new StatUpgradeSaveWrapper
        {
            upgrades = saveDataList
        };

        string json = JsonUtility.ToJson(saveWrapper, true);
        //File.WriteAllText(upgradeSavePath, json);
        YandexGame.savesData.upgradeLevels=json;

        Debug.Log("Upgrade levels saved to JSON.");
    }

    // Метод для получения индекса апгрейда по данным
    public int GetUpgradeIndex(StatUpgradeData data)
    {
        for (int i = 0; i < statUpgrades.Length; i++)
        {
            if (statUpgrades[i] == data) return i;
        }
        return -1;
    }

    public void SavePlayerStats(PlayerData data)
    {
        string savesJson = JsonUtility.ToJson(data);
        YandexGame.savesData.globalStats = savesJson;
        YandexGame.SaveProgress();
        Debug.Log("Player stats save succeed");
    }

    public PlayerData LoadPlayerStats()
    {
        if (YandexGame.savesData.globalStats != "")
        {
            string savesJson = YandexGame.savesData.globalStats;
            currentPlayerData = JsonUtility.FromJson<PlayerData>(savesJson);
            Debug.Log("Player stats loaded");
            return currentPlayerData;
        }
        else
        {
            Debug.LogWarning("New save file created.");
            return new PlayerData();
        }
    }

    // Вспомогательные классы для работы с JSON
    [System.Serializable]
    public class StatUpgradeSaveData
    {
        public string statName;
        public int currentLevel;
    }

    [System.Serializable]
    public class StatUpgradeSaveWrapper
    {
        public List<StatUpgradeSaveData> upgrades;
    }
}