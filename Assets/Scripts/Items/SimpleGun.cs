using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class SimpleGun : Weapon
{

    protected override void Start()
    {
        base.Start();

        // ƒобавл€ем предопределенные модификации по уровн€м
        AddModification(new Modification("+2 базового урона", w => this.currentDamage += 10f));
        AddModification(new Modification("+1 к скорости снар€да", w => this.currentProjectileSize += 1f));
        AddModification(new Modification("”меньшение перезар€дки на 10%", w => this.currentCooldown *= 0.9f));
        AddModification(new Modification("+3 базового урона", w => this.currentDamage += 3f));
        AddModification(new Modification("”величение скорости снар€да на 20%", w => this.currentProjSpeed *= 3f));
        AddModification(new Modification("”меньшение перезар€дки на 15%", w => this.currentCooldown *= 0.85f));
    }

    protected override void Shoot()
    {
        
        EnemyManager.instance.FindClosiestEnemy();
        enemyPos = EnemyManager.instance._closietsEnemy;
        if (enemyPos == null)
        {
            return;
        }
        Vector3 shootDirection = enemyPos.position - playerPos.position;
        shootDirection.z = 0;

        GameObject projectile = Instantiate(projectilePrefab, playerPos.position, playerPos.rotation);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.velocity = shootDirection.normalized * baseProjSpeed;
        projectile.GetComponent<Projectile>().damage = currentDamage;
    }
}