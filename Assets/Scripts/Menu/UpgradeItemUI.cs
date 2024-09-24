using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeItemUI : MonoBehaviour
{
    public TMP_Text statNameText;                // Текстовое поле для названия улучшения
    public Image iconImage;                  // Изображение иконки улучшения
    public TMP_Text levelText;                   // Текстовое поле для отображения уровня

    private StatUpgradeData upgradeData;
    private System.Action onSelected;

    public void Setup(StatUpgradeData data, System.Action onSelectedCallback)
    {
        upgradeData = data;
        onSelected = onSelectedCallback;
        UpdateUI();

        // Добавляем обработчик нажатия на элемент улучшения
        GetComponent<Button>().onClick.AddListener(() => onSelectedCallback.Invoke());
    }

    public void UpdateUI()
    {
        statNameText.text = upgradeData.statName;
        levelText.text = $"Level: {upgradeData.currentLevel}/{upgradeData.maxLevel}";
        iconImage.sprite = upgradeData.icon;
    }
}