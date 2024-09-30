using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathMenu : MonoBehaviour
{
    public GameObject deathWindowUI;
    public static DeathMenu instance;

    private void Awake()
    {
        instance = this;
    }

    public void TriggerDeathMenu()
    {
        Time.timeScale = 0;
        deathWindowUI.SetActive(true);
        
    }

    public void RevivePlayer()
    {
        PlayerController.instance.ResetPlayerHealth();
        deathWindowUI.SetActive(false);
        Time.timeScale = 1;
    }

    public void EndLevel()
    {

    }
}
