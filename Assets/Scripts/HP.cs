using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Hp : MonoBehaviour
{
    [SerializeField] private bool isPlayer;
    [SerializeField] protected float maxHealth;
   

    protected float currentHealth;

    public bool IsDeath => currentHealth <= 0;

    

    private void Awake()
    {
        currentHealth = maxHealth;    
    }


    public virtual void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (IsDeath && !isPlayer)
        {
            Death();
        }

        if (isPlayer && IsDeath)
        {
            //Death();
        }
    }

    

    public virtual void Death()
    {
        Destroy(gameObject);
    }

}
