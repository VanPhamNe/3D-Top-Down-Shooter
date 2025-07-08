using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject bulletImpactVFX;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private MeshRenderer mesh;
    [SerializeField] private BoxCollider cd; // Collider cua dan
    [SerializeField] private TrailRenderer trailRenderer;
    private float impactForce; // Luc tac dong khi dan cham vao vat the
    private Vector3 startPos;
    private float flyDistance; // Khoang cach bay cua dan
    private bool bulletDisable;
    public int bulletDamage;
    //private LayerMask allyMask; // LayerMask de kiem tra dan co cham vao dong minh hay khong
    protected virtual void Awake()
    {
        cd = GetComponent<BoxCollider>(); // Lay collider cua dan
        rb = GetComponent<Rigidbody>(); // Lay Rigidbody cua dan
        mesh = GetComponent<MeshRenderer>(); // Lay MeshRenderer cua dan
        trailRenderer = GetComponent<TrailRenderer>(); // Lay TrailRenderer cua dan
    }
    protected virtual void Update()
    {
        TrailVisualUpdate();
        //Khac phuc loi khi dan bay ra ngoai ko cham vat the thi tu dong tro ve ObjectPooling
        CheckBulletDisable();
        CheckBulletReturnPool();

    }

    protected void CheckBulletReturnPool()
    {
        if (trailRenderer.time < 0)
        {
            ReturnToPool();

        }
    }

    protected void ReturnToPool()
    {
        ObjectPooling.Instance.ReturnObject(gameObject,0); // Tra ve dan vao hang doi cua ObjectPooling
    }

    protected void CheckBulletDisable()
    {
        if (Vector3.Distance(startPos, transform.position) > flyDistance && !bulletDisable) // Neu khoang cach bay cua dan da vuot qua khoang cach toi da
        {
            cd.enabled = false; // Tat collider cua dan de khong bi cham vao vat the
            mesh.enabled = false; // Tat mesh cua dan de khong hien thi
            bulletDisable = true; // Dan da bi tat
            trailRenderer.Clear();
            trailRenderer.time = 0; // Dat thoi gian de lai duong dan ve 0 de khong hien thi duong dan bay nua
        }
    }

    protected void TrailVisualUpdate()
    {
        if (Vector3.Distance(startPos, transform.position) > flyDistance - 1.5f)
        {
            trailRenderer.time -= 2 * Time.deltaTime;
        }
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        /*rb.constraints = RigidbodyConstraints.FreezeAll;*/ // Dung dan lai khi cham vao vat the
        //if(FriendlyFireEnable() == false)
        //{
        //    //neu dan trung cung enemy thi return
        //    if ((allyMask.value &  (1 << collision.gameObject.layer))>0) // Kiem tra xem dan co cham vao dong minh khong
        //    {
        //        ObjectPooling.Instance.ReturnObject(gameObject, 10); // Tra ve dan vao hang doi cua ObjectPooling
        //        return;
        //    }

        //}
        EnemyShield shield = collision.gameObject.GetComponent<EnemyShield>();

        IDamagable damagable = collision.gameObject.GetComponent<IDamagable>(); // Lay IDamagable neu co
        damagable?.TakeDamage(bulletDamage); // Goi ham TakeDamage cua IDamagable neu co, de thuc hien viec bi thuong
        //if (shield != null)
        //{
        //    shield.ReduceDurability(); // Goi ham ReduceDurability cua EnemyShield de giam do ben cua shield
        //    //rb.velocity = Vector3.zero;
        //    //rb.angularVelocity = Vector3.zero;
        //    CreateImpactVFX();
        //    ReturnToPool();
        //    return;
        //}
        DamageBulletToEnemy(collision);
        CreateImpactVFX();
        ReturnToPool(); // Tra ve dan vao hang doi cua ObjectPooling
    }

    private void DamageBulletToEnemy(Collision collision)
    {
        Enemy enemy = collision.gameObject.GetComponentInParent<Enemy>(); // Lay Enemy neu co

        if (enemy != null)
        {
            Vector3 force = rb.velocity.normalized * impactForce; // Tinh toan luc tac dong dua tren van toc cua dan va luc tac dong
            Rigidbody hitRigibody = collision.collider.attachedRigidbody; // Lay Rigidbody cua vat the bi cham
            enemy.HitImpact(force, collision.contacts[0].point, hitRigibody); // Goi ham HitImpact de Enemy bi tac dong luc
        }
    }

    public void BulletSetup(int bulletdame,float flyDistance=100,float impactForce = 100)
    {
        this.impactForce = impactForce; // Luu luc tac dong cua dan
        this.bulletDamage = bulletdame; // Luu luong sat thuong cua dan
        bulletDisable = false; // Dat lai trang thai dan chua bi tat
        cd.enabled = true;
        mesh.enabled = true; // Bat lai mesh cua dan de hien thi
        trailRenderer.Clear();
        trailRenderer.time = 0.25f; // Thoi gian dan de lai duong dan
        startPos = transform.position; // Luu lai vi tri bat dau bay cua dan
        this.flyDistance = flyDistance + .5f; // Luu khoang cach bay cua dan

    }

    protected void CreateImpactVFX()
    {
       
            
            GameObject newImpact = ObjectPooling.Instance.GetObject(bulletImpactVFX,transform); // Lay hieu ung cham tu ObjectPooling
            
            ObjectPooling.Instance.ReturnObject(newImpact,1f); // Tra ve hieu ung cham vao hang doi cua ObjectPooling
        
    }

}
