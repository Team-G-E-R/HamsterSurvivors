using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GameLogic : MonoBehaviour
{
    private static GameLogic instance;

    public static GameLogic Instance { get { return instance; }}

    private void Awake()
    {
        if (FindObjectsOfType(typeof(GameLogic)).Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    

}
