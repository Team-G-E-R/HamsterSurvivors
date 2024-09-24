using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour, IStatSubject
{
    public float ActiveItemsCooldown;
    public float ActiveItemsProjSpeed;

    private float _playerMaxHealth=100;
    public float PlayerMaxHealth 
    {
        get => _playerMaxHealth;
        set
        {
            _playerMaxHealth = value;
            NotifyHealthChanged();
        } 
    }

    private float _playerHealthRegen;
    public float PlayerHealthRegen
    {
        get => _playerHealthRegen;
        set
        {
            _playerHealthRegen = value;
            NotifyRegenChanged();
        }
    }

    private float _playerMoveSpeedModifier = 1;
    public float PlayerMoveSpeedModifier
    {
        get => PlayerMoveSpeedModifier;
        set
        {
            PlayerMoveSpeedModifier = value;
            NotifySpeedChanged();
        }
    }

    public float PlayerArmor;   
    public float PlayerExpirienseModifire;
    public static PlayerStats instance;
    public PlayerController playerController;

    private List<WeaponStatsObserver> weaponObservers = new List<WeaponStatsObserver>();
    private List<PlayerStatsObserver> playerObservers = new List<PlayerStatsObserver>();

    private void Awake()
    {
        instance = this;
    }

    /* //base format for this game
     public void ModifyPlayerHealtch(float health)
     {
         PlayerMaxHealth += health;
         ApplyPlayerHealch();
     }

     private void ApplyPlayerHealch()
     {
         //custom function for appling definite effect

     }*/


    public void RegisterPlayerObserver(PlayerStatsObserver playerObserver)
    {
        playerObservers.Add(playerObserver);
        
    }

    public void UnregisterPlayerObserver(PlayerStatsObserver playerObserver)
    {
        playerObservers.Remove(playerObserver);
        
    }

    public void RegisterWeaponObserver(WeaponStatsObserver weaponObserver)
    {
        weaponObservers.Add(weaponObserver);
    }
    public void UnregisterWeaponObserver(WeaponStatsObserver weaponObserver)
    {
        weaponObservers.Remove(weaponObserver);
    }

    public void NotifyDamageChanged()
    {
        foreach (var observer in weaponObservers)
        {
            observer.OnDamageChanged(10);
        }
    }

    public void NotifyHealthChanged()
    {
        foreach (var observer in playerObservers)
        {
            observer.OnMaxHealthChanged(PlayerMaxHealth);
        }
    }

    public void NotifySpeedChanged()
    {
        foreach (var observer in playerObservers)
        {
            observer.OnSpeedChanged(PlayerMoveSpeedModifier);
        }
    }

    public void NotifyRegenChanged()
    {
        foreach(var observer in playerObservers)
        {
            observer.OnRegenChanged(PlayerHealthRegen);
        }
    }
}
