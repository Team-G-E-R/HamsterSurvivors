using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelStatUI : MonoBehaviour
{
    [SerializeField] private TMP_Text stat_Name;
    [SerializeField] private TMP_Text stat_value;

    public void InitializeItem(string name, string value)
    {
        stat_Name.text = name;
        stat_value.text = value;
    }
}
