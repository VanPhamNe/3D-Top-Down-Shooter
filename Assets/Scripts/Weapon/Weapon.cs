using UnityEngine;

public enum WeaponType
{
    Pistol,
    Revolver,
    Rifle,
    Shotgun,
    Sniper
}
public enum ShootType
{
    Single, //Ban don
    Auto //Ban tu dong
}
[System.Serializable] //Lam class hien thi tren inspector

public class Weapon
{
    public WeaponType weaponType; //Loai vu khi
    public int bulletDamage; //Luong sat thuong cua dan
    [Header("Magazines Details")]
    public int bulletInMagazines; //So luong dan
    public int magazineCapacity; //Dung tich cua hop dan
    public int totalBullet; //So luong dan toi da

    
    public float reloadSpeed  { get; private set; } //Thoi gian nap dan
    public float equipSpeed { get; private set; } //Thoi gian trang bi vu khi
    public float gunDistance { get; private set; } //Khoang cach toi da cua sung
    public float cameraDistance { get; private set; } //Khoang cach toi da cua camera

    [Header("Shoot Details")]
    public ShootType shootType; //Loai ban
    public float fireRate = 1f; // Toc do ban 1 bullet/1s
    private float lastFireTime; //Thoi gian ban cuoi cung
    public int bulletPerShot { get; private set; } //So luong dan ban trong mot dot ban
    public float defaultfireRate; // Toc do ban mac dinh, dung de reset toc do ban khi tat che do dot ban

    [Header("Spread")]
    private float baseSpread; //Khoang phat tan co ban cua dan
    private float currentSpread; //Khoang phat tan cua dan
    private float maxSpread; //Khoang phat tan toi da cua dan
    private float spreadIncreaseRate= .15f; // Toc do tang khoang phat tan
    private float lastSpreadUpdateTime; //Thoi gian cap nhat khoang phat tan cuoi cung
    private float spreadCooldown=1; // Khoang thoi gian giua cac lan cap nhat khoang phat tan

    [Header("Burst Fire")]
    private int burstModeBulletPerShot;
    private float burstModeFireRate; // Toc do ban trong che do dot ban
    public float burstFireDelay { get; private set; } //Khoang thoi gian giua cac vien dan trong dot ban
    private bool burstModeAvaible;
    public bool burstModeActive;

    

    public WeaponData weaponData { get; private set; } //Luu tru WeaponData lien quan den vu khi nay
    public Weapon(WeaponData weaponData)
    {
        this.fireRate = weaponData.fireRate; //Lay toc do ban tu WeaponData
        this.weaponType = weaponData.weaponType;

        this.shootType = weaponData.shootType; //Lay loai ban tu WeaponData
        this.bulletPerShot = weaponData.bulletPerShot; //Lay so luong dan ban trong mot dot ban tu WeaponData

        this.baseSpread = weaponData.baseSpread; //Lay khoang phat tan co ban tu WeaponData
        this.maxSpread = weaponData.maxSpread; //Lay khoang phat tan toi da tu WeaponData
        this.spreadIncreaseRate = weaponData.spreadIncreaseRate; //Lay toc do tang khoang phat tan tu WeaponData

        this.reloadSpeed = weaponData.reloadSpeed; //Lay thoi gian nap dan tu WeaponData
        this.equipSpeed = weaponData.equipSpeed; //Lay thoi gian trang bi vu khi tu WeaponData
        this.gunDistance = weaponData.gunDistance; //Lay khoang cach toi da cua sung tu WeaponData
        this.cameraDistance = weaponData.cameraDistance; //Lay khoang cach toi da cua camera tu WeaponData

        this.bulletInMagazines = weaponData.bulletInMagazines; //Lay so luong dan trong hop dan tu WeaponData
        this.magazineCapacity = weaponData.magazineCapacity; //Lay dung tich hop dan tu WeaponData
        this.totalBullet = weaponData.totalBullet; //Lay so luong dan toi da tu WeaponData
        this.bulletDamage = weaponData.bulletDamage; //Lay luong sat thuong cua dan tu WeaponData

        this.burstModeAvaible = weaponData.burstModeAvaible; //Kiem tra xem che do dot ban co duoc kich hoat hay khong
        this.burstModeBulletPerShot = weaponData.burstModeBulletPerShot; //Lay so luong dan ban trong che do dot ban tu WeaponData
        this.burstModeFireRate = weaponData.burstModeFireRate; //Lay toc do ban trong che do dot ban tu WeaponData
        this.burstFireDelay = weaponData.burstFireDelay; //Lay khoang thoi gian giua cac vien dan trong che do dot ban tu WeaponData
        this.burstModeActive = weaponData.burstModeActive; //Kiem tra xem che do dot ban co dang hoat dong hay khong
        defaultfireRate = fireRate;

        this.weaponData = weaponData;

    }
    public bool CanShoot() => HaveEnoughBullet() && ReadyToFire(); //Kiem tra xem co du dan de ban va da den thoi diem ban tiep theo hay chua

    #region Reload
    public bool HaveEnoughBullet()
    {
        return bulletInMagazines > 0; //Kiem tra xem con dan trong hop dan hay khong 
    }
    public bool canReload()
    {
        if(bulletInMagazines == magazineCapacity) //Neu so luong dan trong hop dan da day
        {
            return false; //Khong the nap dan
        }
        if (totalBullet > 0)
        {
            //Neu con dan trong kho
            return true; 
        }
        return false;
    }
    public void ReloadBullet()
    {
        int bulletToReload = magazineCapacity;
        if(bulletToReload > totalBullet) //Neu so luong dan trong kho it hon dung tich hop dan
        {
            bulletToReload = totalBullet; //Giam so luong dan trong kho

        }
        totalBullet -= bulletToReload; //Giam so luong dan trong kho
        bulletInMagazines = bulletToReload; //Dat so luong dan trong hop dan bang so luong dan nap vao
        if(totalBullet < 0) //Neu so luong dan trong kho nho hon 0
        {
            totalBullet = 0; //Dat so luong dan trong kho bang 0
        }
    }
    #endregion
    #region Spread
    public Vector3 ApplySpread(Vector3 originalDir)
    {
        UpdateSpread(); // Cap nhat khoang phat tan neu can thiet
        float randomValue=Random.Range(-currentSpread, currentSpread); //Lay gia tri ngau nhien trong khoang -spreadAmount den spreadAmount
        Quaternion spreadRotation = Quaternion.Euler(randomValue, randomValue/2, randomValue); //Tao mot quaternion xoay ngau nhien trong khoang do
        return spreadRotation * originalDir; //Tra ve huong dan da duoc phat tan
    }
    private void UpdateSpread()
    {
        
        if(Time.time > lastSpreadUpdateTime + spreadCooldown) //Neu da den thoi diem cap nhat khoang phat tan tiep theo
        {
            currentSpread = baseSpread; //Dat khoang phat tan ve gia tri co ban
        }
        else
        {
            IncreaseSpread(); //Neu chua den thoi diem cap nhat khoang phat tan tiep theo, tang khoang phat tan
        }
        lastSpreadUpdateTime = Time.time; //Cap nhat thoi gian cap nhat khoang phat tan cuoi cung
    }
    private void IncreaseSpread()
    {
        currentSpread = Mathf.Clamp(currentSpread + spreadIncreaseRate, baseSpread, maxSpread); // gioi han gia tri khoang phat tan trong khoang tu baseSpread den maxSpread
    }
    #endregion
    #region Burst
    public bool BurstActive()
    {
        if(weaponType == WeaponType.Shotgun)
        {
            burstFireDelay = 0;
            return true;
        }
        return burstModeActive;
    }
    public void ToggleBurst()
    {
        if(burstModeAvaible == false)
        {
            return;
        }
        burstModeActive = !burstModeActive;
        if (burstModeActive)
        {
            bulletPerShot = burstModeBulletPerShot;
            fireRate = burstModeFireRate;
        }
        else
        {
            bulletPerShot = 1;
            fireRate = defaultfireRate;
        }

    }
    #endregion
    private bool ReadyToFire()
    {
       if(Time.time > lastFireTime+1 / fireRate) //gia su bat dau time 25s (25> 25+1/1)
        {
            lastFireTime = Time.time; //Cap nhat thoi gian ban cuoi cung  
            return true; //Tra ve true neu da den thoi diem ban tiep theo
        }
        return false;
    }
}
