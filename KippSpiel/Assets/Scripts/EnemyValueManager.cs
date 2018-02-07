using UnityEngine;
using System;

public class EnemyValueManager : MonoBehaviour {

    [Header("ExplosiveEnemy")]
    public int health_ExplosiveEnemy;
    public float reactRange_ExplosiveEnemy;
    public int glowDrop_ExplosiveEnemy;
    public int speed_ExplosiveEnemy;

    public int explosiveDamage_ExplosiveEnemy;
    public float explosionForce_ExplosiveEnemy;
    public float explosionRadius_ExplosiveEnemy;
    public float chargeRange_ExplosiveEnemy;
    public float chargeTime_ExplosiveEnemy;
    public float scaleMultiplier_ExplosiveEnemy;
    public Color voxelColor_ExplosiveEnemy;

    [Header("StaticEnemy")]
    public int health_StaticEnemy;
    public float reactRange_StaticEnemy;
    public int glowDrop_StaticEnemy;
    public int speed_StaticEnemy;

    public float fireRateSec_StaticEnemy;
    public int bulletDamage_StaticEnemy;
    public int bulletSpeed_StaticEnemy;
    public Color voxelColor_StaticEnemy;

    void Start ()
    {
        try
        {
            ExplosionEnemy[] explosiveEnemies = GameObject.FindObjectsOfType<ExplosionEnemy>();

            foreach (ExplosionEnemy enemy in explosiveEnemies)
            {
                enemy.health = health_ExplosiveEnemy;
                enemy.reactRange = reactRange_ExplosiveEnemy;
                enemy.glowDrop = glowDrop_ExplosiveEnemy;
                enemy.speed = speed_ExplosiveEnemy;
                enemy.explosionDamage = explosiveDamage_ExplosiveEnemy;
                enemy.explosionForce = explosionForce_ExplosiveEnemy;
                enemy.explosionRadius = explosionRadius_ExplosiveEnemy;
                enemy.chargeTime = chargeTime_ExplosiveEnemy;
                enemy.chargingRange = chargeRange_ExplosiveEnemy;
                enemy.scaleMultiplier = scaleMultiplier_ExplosiveEnemy;
                enemy.voxelColor = voxelColor_ExplosiveEnemy;
            }
        }   catch (InvalidCastException ex) { }
        
        try
        {
            StaticEnemy[] staticEnemies = GameObject.FindObjectsOfType<StaticEnemy>();

            foreach (StaticEnemy enemy in staticEnemies)
            {
                enemy.health = health_StaticEnemy;
                enemy.reactRange = reactRange_StaticEnemy;
                enemy.glowDrop = glowDrop_StaticEnemy;
                enemy.speed = speed_StaticEnemy;
                enemy.fireRateSec = fireRateSec_StaticEnemy;
                enemy.bulletDamage = bulletDamage_StaticEnemy;
                enemy.bulletSpeed = bulletSpeed_StaticEnemy;
                enemy.voxelColor = voxelColor_StaticEnemy;
            }
        }   catch (InvalidCastException ex) { }
    }
}
