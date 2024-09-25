using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Hp : MonoBehaviour
{
    [SerializeField] private bool isPlayer;
    [SerializeField] protected float maxHealth;
    [SerializeField] private GameObject _expShardPrefab;

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
            Instantiate(_expShardPrefab, transform.position, Quaternion.identity);
            Death();
        }
        /*if (isPlayer && IsDeath)
        {
            SceneManager.LoadScene(0);
        }*/
    }

    public virtual void Death()
    {
        Destroy(gameObject);
    }

}
