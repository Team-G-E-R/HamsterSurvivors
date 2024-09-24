using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : Hp, PlayerStatsObserver
{
    [SerializeField] private float _moveSpeedMulti = 1;
    [SerializeField] private float shootInterval;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float shootSpeed;
    [SerializeField] private float shootDelay;
    [SerializeField] private float healthRegen;
    [Header("MoveSpeedConst")]
    [SerializeField] private float _moveSpeedConst = 300;
    
    private Rigidbody2D rb;

    public TMP_Text hpText;

    private void Start()
    {
        UpdateHealthUI();
        currentHealth = maxHealth / 2;
        rb= GetComponent<Rigidbody2D>();
    }

    private Transform enemyPos;
    private float shootTimer;


    private void Update()
    {
        
        HpRegen();
        if(Input.GetKeyDown(KeyCode.F))
        {
            this.TakeDamage(10);
        }
        
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        float hor = Input.GetAxisRaw("Horizontal");
        float ver = Input.GetAxisRaw("Vertical");
        Vector2 step = new Vector2(hor, ver).normalized;

        if(step != Vector2.zero )
        {
            rb.velocity = step * Time.deltaTime * _moveSpeedMulti * _moveSpeedConst;
        }
        else
        {
            Debug.Log("Zero");
            rb.velocity = Vector2.zero;
        }
        
        Debug.Log(step + " Velocity");
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        UpdateHealthUI();
    }

    public void UpdateHealthUI()
    {
        hpText.text = ($"{Mathf.RoundToInt(currentHealth)}/{maxHealth}");
    }


    public void OnMaxHealthChanged(float newHealth)
    {
        float oldMaxHealth = maxHealth;
        maxHealth = newHealth;
        currentHealth += newHealth - oldMaxHealth;

        Debug.Log("Player health updated: " + maxHealth);
        UpdateHealthUI();
    }


    private void HpRegen()
    {
        if(healthRegen > 0)
        {
            currentHealth += healthRegen*Time.deltaTime;
            UpdateHealthUI();
        }
    }

    private void OnEnable()
    {
        
        if (PlayerStats.instance != null)
        {
            PlayerStats.instance.RegisterPlayerObserver(this);
        }
    }

    private void OnDisable()
    {
        if (PlayerStats.instance != null)
        {
            PlayerStats.instance.UnregisterPlayerObserver(this);
        }
    }

    public void OnSpeedChanged(float newSpeed)
    {
        _moveSpeedMulti = newSpeed;
    }

    public void OnRegenChanged(float newRegen)
    {
        healthRegen = newRegen;
    }
}


