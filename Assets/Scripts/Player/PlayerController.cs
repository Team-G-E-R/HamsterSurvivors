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
    [SerializeField] Animator animator;
    [Header("MoveSpeedConst")]
    [SerializeField] private float _moveSpeedConst = 300;
    private Transform enemyPos;
    private float shootTimer;
    private Rigidbody2D rb;

    public TMP_Text hpText;
    public static PlayerController instance;


    private void Awake()
    {
        instance = this;
    }

    protected override void Start()
    {
        base.Start();
        currentHealth = maxHealth;
        UpdateHealthUI();
        rb= GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

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
        animator.SetFloat("moveX", hor);
        animator.SetFloat("moveY", ver);

        Vector2 step = new Vector2(hor, ver).normalized;

        if(step != Vector2.zero )
        {
            rb.velocity = step * Time.deltaTime * _moveSpeedMulti * _moveSpeedConst;
            animator.SetFloat("speed", rb.velocity.sqrMagnitude);
        }
        else
        {
            rb.velocity = Vector2.zero;
        }    
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        UpdateHealthUI();
    }

    public override void Death()
    {
        DeathMenu.instance.TriggerDeathMenu();
    }

    public void UpdateHealthUI()
    {
        hpText.text = ($"{Mathf.RoundToInt(currentHealth)}/{maxHealth}");
    }

    public void ApplyHeal(float healAmount)
    {
        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();
        Debug.Log($"healed {healAmount} health");
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
        if(healthRegen > 0 && currentHealth < maxHealth)
        {
            currentHealth += healthRegen*Time.deltaTime;
            UpdateHealthUI();
        }
    }


    public void ResetPlayerHealth()
    {
        currentHealth = maxHealth;
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
