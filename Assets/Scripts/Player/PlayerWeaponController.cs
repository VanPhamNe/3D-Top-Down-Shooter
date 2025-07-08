using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    private Player player;
    [SerializeField] private Weapon currentWeapon; // Vu khi hien tai ng choi
    private bool weaponReady;
    private bool isShooting;
    [SerializeField] private WeaponData defaultWeaponData; // Du lieu mac dinh cua vu khi, dung de khoi tao vu khi dau tien trong kho
    //[SerializeField] private LayerMask whatIsAlly;
    [Header("Bullet")]
    [SerializeField] private GameObject bulletPrefab; // Prefab cua dan
    [SerializeField] private float bulletSpeed ; // Toc do dan
    //[SerializeField] private Transform gunPoint; // Vi tri dan duoc tao
    [SerializeField] private float bulletImpactForce = 100f; // Luc tac dong cua dan khi cham vao vat the

    [SerializeField] private Transform weaponHolder; // Vi tri dung de xoay huong sung
    [SerializeField] private Transform aim; //diem ma sung nham toi thuong la tro chuot raycast

    private const float REFERENCE_BULLET_SPEED = 20f; // TOC DO DAN mac dinh dua vao mass cua vien dan

    [Header("Inventory")]
    [SerializeField] private List<Weapon> weaponSlot; // Kho vu khi cua nguoi choi
    [SerializeField] private int maxSlot = 2; // So luong vu khi toi da trong kho

    [SerializeField] private GameObject weaponPickupPrefabs; // Prefab cua vu khi pickup, dung de tao vu khi moi khi nguoi choi nhat duoc vu khi moi
    private void Start()
    {
        player = GetComponent<Player>(); // Lay tham chieu den Player script
        InputEvents();
        currentWeapon.bulletInMagazines = currentWeapon.totalBullet; // Dat so luong dan ban dau bang so luong toi da
        Invoke(nameof(EquipStartingWeapon), 0.1f); // Cho thoi gian de kho vu khi duoc khoi tao truoc khi trang bi vu khi
    }
    private void Update()
    {
        if (player.heath.isDead) //kiem tra xem nhan vat da chet chua
        {
            return; //neu da chet thi khong thuc hien tiep
        }
        if (isShooting) // Neu nguoi choi dang bam nut ban
        {
            Shoot(); // Thuc hien ban
        }
        //if (Input.GetKeyDown(KeyCode.T))
        //{
        //    currentWeapon.ToggleBurst();
        //}
    }

    private void InputEvents()
    {
        PlayerControls playerControls = player.controls; // Lay tham chieu den PlayerControls script
        player.controls.Character.Fire.performed += ctx => isShooting=true; // Dang ky su kien khi nguoi choi bam nut Fire
        player.controls.Character.Fire.canceled += ctx => isShooting = false; // Dang ky su kien khi nguoi choi tha nut Fire
        player.controls.Character.EquipSlot1.performed += ctx => EquipWeapon(0); // Dang ky su kien khi nguoi choi bam nut EquipWeapon
        player.controls.Character.EquipSlot2.performed += ctx => EquipWeapon(1); // Dang ky su kien khi nguoi choi bam nut EquipWeapon
        player.controls.Character.EquipSlot3.performed += ctx => EquipWeapon(2); // Dang ky su kien khi nguoi choi bam nut EquipWeapon
        player.controls.Character.Drop.performed += ctx => DropWeapon(); // Dang ky su kien khi nguoi choi bam nut Drop
        player.controls.Character.Reload.performed += ctx =>
        {
            if (currentWeapon.canReload()&& IsWeaponReady())
            {
                Reload();
            }
        }; // Dang ky su kien khi nguoi choi bam nut PickupWeapon
        player.controls.Character.ToggleWeaponMode.performed += ctx => currentWeapon.ToggleBurst(); // Dang ky su kien khi nguoi choi bam nut ToggleWeaponMode
    }
    #region Reload,Equip,Drop,Pickup
    private void Reload()
    {
        SetWeaponReady(false); // Dat trang thai vu khi la khong san sang
        player.weaponVisual.PlayReloadAnimation(); // Choi animation nap dan
    }

    private void EquipWeapon(int i)
    {
        if(i >= weaponSlot.Count) // Kiem tra xem chi so vu khi co hop le khong
        {
            Debug.Log("Weapon slot index out of range!"); // In ra thong bao neu chi so vu khi khong hop le
            return; // Thoat khoi ham
        }
        if (currentWeapon == weaponSlot[i])
        {
            Debug.Log("Weapon is already equipped!");
            return;
        }
        SetWeaponReady(false); // Dat trang thai vu khi la khong san sang
        currentWeapon = weaponSlot[i]; // Dat vu khi hien tai bang vu khi trong kho
        //player.weaponVisual.SwitchGunOff(); // Tat model vu khi hien tai
        player.weaponVisual.PlayerEquipWeaponAnimation(); // Choi animation khi nguoi choi chon vu khi
        CameraManager.Instance.ChangeCameraDistance(currentWeapon.cameraDistance); // Thay doi khoang cach camera toi vu khi moi
    }
    private void DropWeapon()
    {
        if (HasOneWeapon())
        {
            return; // Neu kho vu khi chi con 1 vu khi thi khong the vut vu khi
        }
        CreateWeaponOnGround();
        weaponSlot.Remove(currentWeapon); // Nem vu khi hien tai
        EquipWeapon(0); // Chon vu khi dau tien trong kho lam vu khi hien tai
    }

    private void CreateWeaponOnGround()
    {
        GameObject dropWeapon = ObjectPooling.Instance.GetObject(weaponPickupPrefabs,transform); // Lay prefab vu khi pickup tu ObjectPooling
        dropWeapon.GetComponent<PickupWeapon>().SetupPickupWeapon(currentWeapon, transform); // Dat vu khi pickup moi voi vu khi hien tai va vi tri cua nguoi choi
    }

    public void PickupWeapon(Weapon newweapon)
    {
        if(HasWeaponInSlot(newweapon.weaponType) != null) //Kiem tra xem kho vu khi da co vu khi cung loai chua
        {
            HasWeaponInSlot(newweapon.weaponType).totalBullet += newweapon.totalBullet; // Neu da co, cong them so luong dan toi da vao vu khi da co
            return;
        }
        // Neu kho vu khi da day va loai vu khi moi khac voi loai vu khi hien tai
        if (weaponSlot.Count >= maxSlot && newweapon.weaponType != currentWeapon.weaponType) 
        {
            //Debug.Log("Inventory full!"); // In ra thong bao
            int weaponIndex = weaponSlot.IndexOf(currentWeapon); // Tim chi so cua vu khi hien tai trong kho
            player.weaponVisual.SwitchGunOff(); // Tat model vu khi hien tai
            weaponSlot[weaponIndex] = newweapon; // Thay the vu khi hien tai bang vu khi moi
            EquipWeapon(weaponIndex); // Chon vu khi moi lam vu khi hien tai
            return; // Thoat khoi ham
        }
        
        weaponSlot.Add(newweapon); // Them vu khi moi vao kho vu khi
        player.weaponVisual.SwitchOnBackupWeaponModels(); // Bat model vu khi backup

    }
    private void EquipStartingWeapon()
    {
       
        weaponSlot[0] = new Weapon(defaultWeaponData); // Dat vu khi dau tien trong kho la Pistol
        EquipWeapon(0); // Chon vu khi dau tien trong kho lam vu khi hien tai
    }
    public bool HasOneWeapon() // Kiem tra xem nguoi choi co 1 vu khi hay khong
    {
        return weaponSlot.Count <= 1; // Tra ve true neu kho vu khi chi co 1 vu khi
    }
    public void SetWeaponReady(bool ready) => weaponReady = ready; // Dat trang thai san sang cua vu khi
    public bool IsWeaponReady() => weaponReady; // Lay trang thai san sang cua vu khi
    #endregion
    private void Shoot()
    {
        if (IsWeaponReady() == false)
        {
            //Debug.Log("Weapon is not ready!");
            return; // Neu vu khi khong san sang thi khong ban duoc
        }
        if (currentWeapon.CanShoot() == false) // Kiem tra xem co du dan de ban khong
        {
            Debug.Log("Busy or No bullet!"); // In ra thong bao neu khong co dan
            return; // Thoat khoi ham
        }
        player.weaponVisual.PlayFireAnimation(); // Choi animation ban sung
        if (currentWeapon.shootType == ShootType.Single) // Kiem tra loai ban
        {
            isShooting = false; // Neu loai ban la Single thi tat nut ban sau khi ban xong
        }
        if (currentWeapon.BurstActive() == true) {
            StartCoroutine(BurstFire()); // Bat dau dot ban neu loai ban la BurstFire
            return;
        }
      
        FireSingleBullet();
    }
    private IEnumerator BurstFire()
    {
        SetWeaponReady(false); // Dat trang thai vu khi la khong san sang
        for (int i = 1; i <= currentWeapon.bulletPerShot; i++) { 
            FireSingleBullet(); // Ban tung vien dan trong dot ban
            yield return new WaitForSeconds(currentWeapon.burstFireDelay); // Cho thoi gian giua cac vien dan
            if(i >= currentWeapon.bulletPerShot)
            {
                SetWeaponReady(true); // Dat trang thai vu khi la san sang sau khi ban xong dot ban
            }
        }
    }
    private void FireSingleBullet()
    {
        currentWeapon.bulletInMagazines--; // Giam so luong dan trong hop dan di 1 khi ban thanh cong
        GameObject newBullet = ObjectPooling.Instance.GetObject(bulletPrefab,GetGunPoint()); // Lay dan tu ObjectPooling
        //Instantiate(bulletPrefab, gunPoint.position, Quaternion.LookRotation(gunPoint.forward)); // Tao dan moi tai vi tri gunPoint
        //newBullet.transform.position = GetGunPoint().position; // Dat vi tri cua dan moi bang vi tri cua gunPoint
        newBullet.transform.rotation = Quaternion.LookRotation(GetGunPoint().forward); // Dat huong cua dan moi theo huong dan bay toi
        Rigidbody rbnewBullet = newBullet.GetComponent<Rigidbody>(); // Lay Rigidbody cua dan moi
        Bullet bulletScript= newBullet.GetComponent<Bullet>(); // Lay script Bullet cua dan moi
        bulletScript.BulletSetup(currentWeapon.bulletDamage,currentWeapon.gunDistance,bulletImpactForce);
        Vector3 bulletDir = currentWeapon.ApplySpread(BulletDirection()); // Lay huong dan bay toi va ap dung phat tan neu co
        rbnewBullet.velocity = bulletDir * bulletSpeed; // Dat van toc cho dan theo huong dan
        rbnewBullet.mass = REFERENCE_BULLET_SPEED / bulletSpeed; // Dat mass cua dan dua vao toc do dan de dam bao dan se bay voi toc do chinh xac
       
    }

    public Vector3 BulletDirection() {
        Transform aim = player.playerAim.Aim(); // Lay vi tri aim chuot nham toi
        Vector3 direction = (aim.position - GetGunPoint().position).normalized; // Tinh toan huong dan se bay toi
        if (player.playerAim.CanAimPrecision()==false && player.playerAim.Target() == null) //neu ko phai aim chinh xac va ko co muc tieu thi duong dan bay ngang
        {
            direction.y = 0; // Neu aim chinh xac, chi lay phan x, z
        }
        return direction; // Tra ve huong dan
    }

    private void OnDrawGizmos()
    {
        
        //Gizmos.DrawLine(weaponHolder.position, weaponHolder.position + weaponHolder.forward * 25);
        //Gizmos.color = Color.red; // Set mau cho gizmos
        //Gizmos.DrawLine(gunPoint.position, gunPoint.position + BulletDirection() * 25); // Ve duong thang tu vi tri sung toi vi tri dan se bay toi
        if (weaponHolder != null  && aim != null && player != null && player.playerAim != null)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawLine(weaponHolder.position, weaponHolder.position + weaponHolder.forward * 25);

            Gizmos.color = Color.red;
            Gizmos.DrawLine(GetGunPoint().position, GetGunPoint().position + BulletDirection() * 25);
        }
    }
    #region Pulic Get Methods
    public Transform GetGunPoint() //Cho phep cac script khac truy cap vi tri xuat dan cua sung

    {
        return player.weaponVisual.GetCurrentWeaponModel().gunPoint; // Tra ve vi tri xuat dan
    }
    public Weapon GetCurrentWeapon() //Cho phep cac script khac truy cap vu khi hien tai
    {
        return currentWeapon; // Tra ve vu khi hien tai
    }
    public Weapon BackupWeapon() //Tra ve vu khi backup
    {
        foreach(Weapon weapon in weaponSlot) //Duyet qua tat ca vu khi trong kho
        {
            if (weapon != currentWeapon) 
            {
                return weapon; // Tra ve vu khi khac voi vu khi hien tai
            }
        }
        return null; // Neu khong co vu khi khac thi tra ve null
    }
    public Weapon HasWeaponInSlot(WeaponType weaponType)
    {
        foreach (Weapon weapon in weaponSlot) // Duyet qua tat ca vu khi trong kho
        {
            if (weapon.weaponType == weaponType) // Neu vu khi co loai bang voi loai can kiem tra
            {
                return weapon; // Tra ve true neu co
            }
        }
        return null; // Tra ve false neu khong co
    }
    #endregion
}
