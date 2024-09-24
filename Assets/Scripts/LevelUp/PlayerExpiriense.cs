using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System;
using Unity.Mathematics;
using static UnityEditor.Progress;

public class PlayerExpiriense : MonoBehaviour
{
    private int _currentLevel;
    private int _currentExp;
    private int _expToNewLvl = 10;
    private float _vacuumSpeed = 6f;
    private List<GameObject> LevelUpItems = new List<GameObject>();

    [SerializeField] private float epxTakingRange;
    [SerializeField] private LayerMask expLayer;
    [SerializeField] private Slider _expSlider;
    [SerializeField] private TMP_Text _expText;
    [SerializeField] private TMP_Text _levelText;
    [SerializeField] GameObject LevelUpPanel;
    [SerializeField] GameObject UpdateGridLayout;

    [SerializeField] private ItemManager _itemManager;

    public UnlockManager UnlockManager;

    private void Start()
    {
        UpdateLevelUi();
    }

    private void Update()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, epxTakingRange, expLayer);

        if (hitColliders.Length > 0)
        {
            foreach (Collider2D col in hitColliders)
            {
                col.transform.position = Vector3.MoveTowards(col.transform.position, transform.position, _vacuumSpeed * Time.deltaTime);
                if (Vector2.Distance(col.transform.position, transform.position) < 0.5f)
                {
                    ConsumeExpChard(col.gameObject);
                }
            }
        }
    }


    private void ConsumeExpChard(GameObject expChard)
    {
        _currentExp++;
        Destroy(expChard);

        UpdateLevelState();
    }

    private void UpdateLevelState()
    {
        _currentExp++;
        if (_currentExp >= _expToNewLvl)
        {
            LevelUp();
            _currentLevel++;
            _currentExp = 0;
            _expToNewLvl = _expToNewLvl + (int)(_expToNewLvl * 0.2);
        }

        UpdateLevelUi();
    }

    private void UpdateLevelUi()
    {
        _expSlider.value = (float)(_currentExp / (float)_expToNewLvl);
        _expText.text = ($"{_currentExp} / {_expToNewLvl}");
        _levelText.text = _currentLevel.ToString();
    }

    private void LevelUp()
    {
        LevelUpPanel.SetActive(true);

        List<Item> itemsToLevelUp = new List<Item>();


        foreach (Item item in _itemManager.itemSlots)
        {
            if (item == null)
            {
                continue;
            }
            else if (!item.ReachMaxLevel())
            {
                itemsToLevelUp.Add(item);
            }
        }

        List<Item> possableNewItems = ÑheckForAddingNewItems();
        if (itemsToLevelUp.Count == 0 && possableNewItems.Count == 0)
        {
            return; 
        }

        ChooseItemsToLevelUp(itemsToLevelUp, possableNewItems);
        Time.timeScale = 0f;
    }

    private void ChooseItemsToLevelUp(List<Item> itemsToLevelUp, List<Item> possableNewItems)
    {
        List<Item> choosenItemToLevelUp = new List<Item>();
        

        if(possableNewItems.Count <= 3 && possableNewItems.Count != 0)
        {
            choosenItemToLevelUp.AddRange(possableNewItems);
        }
        else if(possableNewItems.Count >3)
        {
            choosenItemToLevelUp.AddRange(ChooseRandomItems(3, possableNewItems));
        }
        
        if (itemsToLevelUp.Count <= 3 && itemsToLevelUp.Count != 0)
        {
            choosenItemToLevelUp.AddRange(itemsToLevelUp);
        }
        else if(itemsToLevelUp.Count >3)
        {
            choosenItemToLevelUp.AddRange(ChooseRandomItems(3, itemsToLevelUp));            
        }

        ShowLevelUpItems(choosenItemToLevelUp);
    }

  

    private void ShowLevelUpItems(List<Item> itemsToLevelUp)
    {
        List<Item> updatedItems = ChooseRandomItems(3, itemsToLevelUp);
        foreach (Item item in updatedItems)
        {
            if( _itemManager.itemSlots.Contains(item))
            {
                CreateUpdateItemUI(item);
            }
            else
            {
                CreateNewitemUI(item);
            }   
        }
    }

    private void CreateUpdateItemUI(Item item)
    {
        UpdatedItem updateItem = Instantiate(Resources.Load<UpdatedItem>("LevelUp/UpdatedSkill"), Vector3.zero, Quaternion.identity);
        updateItem.transform.SetParent(UpdateGridLayout.transform);
         
        updateItem.InitializeItem(item.itemSptite, item.GetNextModificationText(), (item.GetItemCurrentLevel()+1).ToString() + " óðîâåíü");
        LevelUpItems.Add(updateItem.gameObject);

        Button button = updateItem.GetComponentInChildren<Button>();
        button.onClick.AddListener(() => item.ActivateNextModification());
        button.onClick.AddListener(() => CloseLevelUpPanel());
    }

    private void CreateNewitemUI(Item item)
    {
        NewItem newItem = Instantiate(Resources.Load<NewItem>("LevelUp/NewItem"), Vector3.zero, Quaternion.identity);
        newItem.transform.SetParent(UpdateGridLayout.transform);

        newItem.InitializeItem(item.itemSptite, item.itemName, item.itemDescription);
        LevelUpItems.Add(newItem.gameObject);

        Button button = newItem.GetComponentInChildren<Button>();
        button.onClick.AddListener(() => _itemManager.EquipItem(item));
        button.onClick.AddListener(() => CloseLevelUpPanel());
    }

    private List<Item> ÑheckForAddingNewItems()
    {
        List<Item> NewItems = new List<Item>();
        if(_itemManager.ItemSlotsFull())
        {
            return NewItems;
        }

        List<Item> possableNewItems = UnlockManager.GetNewUnlockedItems(_itemManager.itemSlots);

        if (!_itemManager.WeaponSlotsFull())
        {
            foreach (Item item in possableNewItems)
            {
                if (item is Weapon)
                {
                    NewItems.Add(item);
                }
            }
        }

        if (!_itemManager.PassiveItemsSlotsFull())
        {
            foreach (Item item in possableNewItems)
            {
                if (item is PassiveItem)
                {
                    NewItems.Add(item);
                }
            }
        }


        return NewItems;
    }


    private List<Item> ChooseRandomItems(int randomItemsCount, List<Item> items)
    {
        List<Item> randomItems = new List<Item>();
        for (int i = 0; i < randomItemsCount; i++)
        {
            Item choosenItem = items[UnityEngine.Random.Range(0, items.Count)];
            randomItems.Add(choosenItem);
            items.Remove(choosenItem);
        }
        return randomItems;
    }

    private void CloseLevelUpPanel()
    {
        foreach(GameObject item in LevelUpItems)
        {
            Destroy(item);
        }
        LevelUpItems.Clear();
        LevelUpPanel.SetActive(false);
        Time.timeScale = 1.0f;
    }
}
