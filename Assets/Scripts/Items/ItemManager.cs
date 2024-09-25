using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;



public class ItemManager : MonoBehaviour
{
    public Weapon[] weaponSlots = new Weapon[4];
    public PassiveItem[] passiveItemSlots = new PassiveItem[4];
    public Item[] itemSlots;
    private Image[] weaponSlotImages;
    private Image[] passiveItamesSlotImages;
    [SerializeField] private GameObject weaponContainerUi;
    [SerializeField] private GameObject passiveItemContainerUi;
    [SerializeField] private GameObject weaponContainerOb;

    private void Awake()
    {
         weaponSlotImages = weaponContainerUi.GetComponentsInChildren<Image>();
         passiveItamesSlotImages = passiveItemContainerUi.GetComponentsInChildren<Image>();
        

        itemSlots = new Item[weaponSlots.Length + passiveItemSlots.Length];  
    }

    private void Start()
    {
        
    }

    void Update()
    {
        for (int i = 0; i < weaponSlots.Length; i++)
        {
            if (weaponSlots[i] != null)
            {
                weaponSlots[i].TryShoot();
            }
        }
    }

    public void EquipItem(Item new_item)
    {
        Item item = Instantiate(new_item, transform.position, Quaternion.identity);
        item.transform.SetParent(weaponContainerOb.transform, true);

        if (item is Weapon && GetFreeSlotIndex(weaponSlots) != null)
        {
            int freeWeaponSlot = (int)GetFreeSlotIndex(weaponSlots);
            weaponSlots[freeWeaponSlot] = (Weapon)item;
            itemSlots[(int)GetFreeSlotIndex(itemSlots)] = item;
            weaponSlotImages[freeWeaponSlot].sprite = item.itemSptite;
        }
        else if (item is PassiveItem && GetFreeSlotIndex(passiveItemSlots) != null)
        {
            int frePassiveSlot = (int)GetFreeSlotIndex(passiveItemSlots);
            passiveItemSlots[frePassiveSlot] = (PassiveItem)item;
            itemSlots[(int)GetFreeSlotIndex(itemSlots)] = item;
            passiveItamesSlotImages[frePassiveSlot].sprite = item.itemSptite;
        }
        else
        {
            return;
        }
    }


    private int? GetFreeSlotIndex (Item[] slots)
    {
        for(int i = 0; i<slots.Length; i++)
        {
            if (slots[i] == null)
            {
                return i;
            }
        }
        return null;
    }


    public bool ItemSlotsFull()
    {
        foreach(Item item in itemSlots)
        {
            if(item == null)
            {
                return false;
            }
        }
        return true;
    }

    public bool WeaponSlotsFull()
    {
        foreach(Item item in weaponSlots)
        {
            if(item == null)
            {
                return false;
            }
        }
        return true;
    }

    public bool PassiveItemsSlotsFull()
    {
        foreach (Item item in passiveItemSlots)
        {
            if (item == null)
            {
                return false;
            }
        }
        return true;
    }



}