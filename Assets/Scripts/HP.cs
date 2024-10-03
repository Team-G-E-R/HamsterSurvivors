using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Hp : MonoBehaviour
{
    [SerializeField] private bool isPlayer;
    [SerializeField] protected float maxHealth;
    [SerializeField] GameObject floatingTextPrefab;
    private RectTransform textCanvasTransform;
  
    protected float currentHealth;

    public bool IsDeath => currentHealth <= 0;

    

    private void Awake()
    {
        currentHealth = maxHealth;    
    }

    protected virtual void Start()
    {
        textCanvasTransform = FindAnyObjectByType<CanvasPosSingleton>().canvasTransform;
        Debug.Log("finded transform " + $"{textCanvasTransform == null}");
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
            Death();
        }
    }

    protected void ShowFloatingText(float value, Color textColor)
    {
        if (floatingTextPrefab != null)
        {
            GameObject floatingText = Instantiate(floatingTextPrefab, transform.position, Quaternion.identity, textCanvasTransform);
            floatingText.transform.SetParent(textCanvasTransform, true);
            TMP_Text textMesh = floatingText.GetComponent<TMP_Text>();

            if (textMesh != null)
            {
                textMesh.text = value.ToString();  // Присваиваем значение
                textMesh.color = textColor;        // Устанавливаем цвет текста
            }

            // Запускаем анимацию и уничтожаем объект после завершения
            Animator textAnimator = floatingText.GetComponent<Animator>();
            if (textAnimator != null)
            {
                Destroy(floatingText, textAnimator.GetCurrentAnimatorStateInfo(0).length);
            }
            else
            {
                Destroy(floatingText, 2f);  // Уничтожить через 1 секунду, если нет анимации
            }
        }
    }

    public virtual void Death()
    {
        Destroy(gameObject);
    }

}
