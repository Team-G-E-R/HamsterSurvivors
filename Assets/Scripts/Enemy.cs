using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Hp
{
    [SerializeField] private float moveSpeed;

    [SerializeField] private bool isDestroyItem;

    private Transform target;

    private void Start()
    {
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
        base.Death();
       
    }

    private void DropRandomItemOrNthg()
    {

    }
}
