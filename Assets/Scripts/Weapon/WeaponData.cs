using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponData", menuName = "Weapon Data")]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public WeaponType weaponType;
    public ShootType shootType; //Loai ban
    public float fireRate;
    public int bulletPerShot; //So luong dan ban trong mot dot ban

    [Header("Spread")]
    public float baseSpread; //Khoang phat tan co ban cua dan
    public float maxSpread; //Khoang phat tan toi da cua dan
    public float spreadIncreaseRate = .15f; // Toc do tang khoang phat tan

    [Header("Info")]
    [Range(1,3)]
    public float reloadSpeed = 1; //Thoi gian nap dan
    [Range(1, 3)]
    public float equipSpeed = 1; //Thoi gian trang bi vu khi
    [Range(4, 25)]
    public float gunDistance = 4; //Khoang cach toi da cua sung
    [Range(4, 8)]
    public float cameraDistance = 5; //Khoang cach toi da cua camera

    [Header("Burst Fire")]
    public int burstModeBulletPerShot;
    public float burstModeFireRate; // Toc do ban trong che do dot ban
    public float burstFireDelay = .1f; //Khoang thoi gian giua cac vien dan trong dot ban
    public bool burstModeAvaible;
    public bool burstModeActive;

    [Header("Magazines Details")]
    public int bulletInMagazines; //So luong dan
    public int magazineCapacity; //Dung tich cua hop dan
    public int totalBullet; //So luong dan toi da

    [Header("bullet info")]
    public int bulletDamage; // Luong sat thuong cua dan

    [Header("UI")]
    public Sprite weaponIcon; //Hinh anh cua vu khi
}
