using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "StatUpgradeData", menuName = "Game/Upgrades level data")]
public class UpgradeValues : ScriptableObject
{
    [Header("Upgrades Values")]
    public float playerMaxHealth = 10;
    public float playerHealthRegen = 0.1f;
    public float weaponDamageModifier = 10;
    public float playerMoveSpeedModifier = 5;
    public float playerArmor = 5;
    public float activeItemsCooldown = 5;
    public float experienceGainModifier = 5;
    public float experienceTakingRange = 2;
}

