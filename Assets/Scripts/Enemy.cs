using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Hp
{
    [SerializeField] private float moveSpeed;

    [SerializeField] private bool isDestroyItem;
     private GameObject _expShardPrefab;

    private Transform target;

    private void Start()
    {
        _expShardPrefab = Resources.Load<GameObject>("XpShard");
        target = GameObject.FindWithTag("Player").transform;
        EnemyManager.instance.AddTransformToList(this.gameObject.transform);
    }


    private void Update()
    {
        //Vector3 direction = (target.position - transform.position).normalized;
        //transform.position += direction * moveSpeed * Time.deltaTime;
    }

    public override void Death()
    {
        if(isDestroyItem)
        {
            DropRandomItemOrNthg();          
        }
        else
        {
            Instantiate(_expShardPrefab, transform.position, Quaternion.identity);
            LevelStats.instance.enemiesKilled += 1;
        }
        base.Death();     
    }

    private void DropRandomItemOrNthg()
    {
        int randomNum = Random.Range(0, 100);

        if(randomNum > 95)
        {
            Instantiate(Resources.Load<Consumable>("ConsumableItems/HealItem"), transform.position, Quaternion.identity);
        }
        else if(randomNum > 65)
        {
            Instantiate(Resources.Load<Consumable>("ConsumableItems/Coin"), transform.position, Quaternion.identity);
        }
        else
        {
            return;
        }
    }
}
