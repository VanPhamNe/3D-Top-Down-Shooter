using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponData", menuName = "Enemy data/Enemy Range Data")]
public class Enemy_RangeWeaponData : ScriptableObject
{
    [Header("Weapon Details")]
    public Enemy_RangeWeaponType weaponType; // Type of ranged weapon used by the enemy
    public float fireRate = 1f; // bullet per second
    public int minBulletPerAttack = 1; // Minimum number of bullets to shoot in a burst
    public int maxBulletPerAttack = 1; // Maximum number of bullets to shoot in a burst

    public float minWeaponCooldown = 2f; // Minimum cooldown time for the weapon
    public float maxWeaponCooldown = 3f; // Maximum cooldown time for the weapon

    [Header("Bullet details")]
    public float bulletSpeed = 20; // Speed of the bullet
    public float weaponSpread = 0.1f; // Do phan tan cua dan
    public int bulletDamage; // Damage dealt by the bullet
    public int GetBulletPerAttack() => Random.Range(minBulletPerAttack, maxBulletPerAttack + 1); //Lay ngau nhien dan ban ra khi tan cong
    public float GetWeaponCooldown()=> Random.Range(minWeaponCooldown, maxWeaponCooldown); //Lay ngau nhien thoi gian hoi cua vu khi
    public Vector3 ApplyWeaponSpread(Vector3 originalDir)
    {
        float randomSpread = Random.Range(-weaponSpread, weaponSpread); //Lay gia tri phat tan ngau nhien trong khoang -weaponSpread den weaponSpread
        Quaternion spreadRotation = Quaternion.Euler(randomSpread, randomSpread / 2, randomSpread); //Tao mot quaternion xoay ngau nhien trong khoang do
        return spreadRotation * originalDir; //Tra ve huong dan da duoc phat tan
    }
}
