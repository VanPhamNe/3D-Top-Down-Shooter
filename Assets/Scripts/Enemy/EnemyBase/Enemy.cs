using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))] // Yeu cau Enemy phai co NavMeshAgent de di chuyen tren map va cac script con se tu dong tao NavMeshAgent
public class Enemy : MonoBehaviour
{
   public int health = 25; // Suc khoe cua Enemy
    public float turnSpeed; // toc do xoay cua Enemy
    public float aggressiveRange; // khoang cach Enemy co the tan cong toi Player
    public bool inBattleMode { get; private set; }
    public LayerMask whatIsAlly;
    public LayerMask whatIsPlayer; // LayerMask de Enemy co the tan cong toi Player
    [Space]

    [Header("Idle")]
    public float idleTime; // thoi gian cho trang thai Idle

    [Header("Move")]
    public float moveSpeed; // toc do di chuyen cua Enemy
    [SerializeField] private Transform[] patrolPoint; // diem canh bao cua Enemy, Enemy se di chuyen den diem canh bao tiep theo trong danh sach nay
    private Vector3[] patrolPointsPos; // mang chua cac diem canh bao, de Enemy co the di chuyen toi diem canh bao tiep theo
    private int currentPatrolIndex = 0; // chi so cua diem canh bao hien tai
    public float runSpeed = 3; // toc do di chuyen khi Enemy theo doi Player
    private bool manualMovement;
    private bool manualRotation; 

    //[Header("Attack")]
    //public float attackRange; // khoang cach Enemy co the tan cong toi Player
    //public float attackMoveSpeed;
    protected bool isMeleeAttackReady; // bien kiem soat trang thai Enemy co san sang tan cong hay khong


    public NavMeshAgent agent { get; private set; } // NavMeshAgent de Enemy co the di chuyen tren map

    public Transform player { get; private set; } // Player de Enemy co the tan cong toi Player
    public Animator anim { get; private set; } // Animator de Enemy co the chuyen dong
    public EnemyStateMachine stateMachine { get; private set; }
    public EnemyVisual enemyVisual { get; private set; } // Reference to the EnemyVisual component for color setup
    public Ragdoll ragdoll { get; private set; } // Reference to the Enemy_Ragdoll component for ragdoll functionality
    public EnemyHeath enemyHealth { get; private set; } // Reference to the EnemyHeath component for health management

    protected virtual void Awake() {
        stateMachine = new EnemyStateMachine();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        player = GameObject.Find("Player").GetComponent<Transform>(); // Tim kiem Player trong scene
        enemyVisual = GetComponent<EnemyVisual>(); // Initialize the EnemyVisual component
        ragdoll = GetComponent<Ragdoll>(); // Initialize the Enemy_Ragdoll component
        enemyHealth = GetComponent<EnemyHeath>(); // Initialize the EnemyHeath component
    }
    // Start is called before the first frame update
    protected virtual void Start()
    {
        InitizalizePatrolPoints();

    }
    
    // Update is called once per frame
    protected virtual void Update()
    {
        if (ShouldEnterBattleMode())
        {
            EnterBattleMode(); // Enter battle mode if conditions are met
        }
    }
    protected virtual void InializeSpecial()
    {

    }
    #region Patrol logic
    public Vector3 GetPatrolDestination()
    {
        Vector3 destination = patrolPointsPos[currentPatrolIndex]; // Lay vi tri cua diem canh bao hien tai
        currentPatrolIndex++; // Tang chi so diem canh bao hien tai
        if (currentPatrolIndex >= patrolPoint.Length) // Neu chi so diem canh bao vuot qua so luong diem canh bao
        {
            currentPatrolIndex = 0; // Reset chi so ve 0
        }
        return destination; // Tra ve vi tri cua diem canh bao hien tai
    }
    // Ham khoi tao diem canh bao, bo parent cua diem canh bao de Enemy co the di chuyen toi diem canh bao
    private void InitizalizePatrolPoints()
    {
        patrolPointsPos = new Vector3[patrolPoint.Length]; // Khoi tao mang chua cac diem canh bao
        for(int i = 0; i < patrolPoint.Length; i++) // Duyet qua tat ca cac diem canh bao
        {
            patrolPointsPos[i] = patrolPoint[i].position; // Luu vi tri cua diem canh bao vao mang
            patrolPoint[i].gameObject.SetActive(false); // Tat diem canh bao trong scene de tranh mat tai nguyen
        }
    }
    #endregion
    #region AnimationEvents
    public void ActiveManualMovement(bool manualMovement) => this.manualMovement = manualMovement; // Kich hoat hoac tat che do di chuyen thu cong cua Enemy
    public bool ManualMovementActive() => manualMovement; // Kiem tra xem che do di chuyen thu cong cua Enemy co dang hoat dong khong
    public void ActiveManualRotation(bool manualRotation) => this.manualRotation = manualRotation; // Kich hoat hoac tat che do xoay thu cong cua Enemy
    public bool ManualRotationActive() => manualRotation; // Kiem tra xem che do xoay thu cong cua Enemy co dang hoat dong khong
    public void AnimationTrigger() => stateMachine.currentState.AnimationTrigger(); // Kich hoat trang thai hien tai cua Enemy


    public virtual void AbilityTrigger()
    {
        stateMachine.currentState.AbilityTrigger();
    }
    #endregion
    public Quaternion FaceTarget(Vector3 target , float turnSpeed = 0)
    {
        Quaternion targetRotation = Quaternion.LookRotation(target - transform.position); // Lay goc xoay de Enemy quay ve huong cua target
        Vector3 currentEuler = transform.rotation.eulerAngles; // Lay goc xoay hien tai cua Enemy
        if(turnSpeed == 0)
        {
            turnSpeed = this.turnSpeed; // Neu turnSpeed khong duoc truyen vao, su dung turnSpeed mac dinh cua Enemy
        }
        float yRotation = Mathf.LerpAngle(currentEuler.y, targetRotation.eulerAngles.y, turnSpeed * Time.deltaTime); // Di chuyen goc xoay theo thoi gian
        return Quaternion.Euler(currentEuler.x, yRotation, currentEuler.z); // Tra ve goc xoay moi cua Enemy
    }
    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.red; // Set color for the Gizmos
        Gizmos.DrawWireSphere(transform.position, aggressiveRange); // Draw a wire sphere to visualize the aggressive range
       
    }
   
    public virtual void EnterBattleMode()
    {
        inBattleMode = true; // Bat dau che do chien dau
  
    }
    protected bool ShouldEnterBattleMode()
    {
        if (IsPlayerInAggressiveRange() && !inBattleMode) // Neu Enemy trong khoang cach tan cong toi Player va chua bat dau che do chien dau
        {
            EnterBattleMode(); // Bat dau che do chien dau
            return true; // Tra ve true de cho phep Enemy vao che do chien dau
        }
        return false; // Tra ve false neu Enemy khong trong khoang cach tan cong toi Player hoac da bat dau che do chien dau
    }
   
    public virtual void GetHit(int damage)
    {
        enemyHealth.ReduceHealth(damage); // Giam suc khoe cua Enemy khi bi tan cong
        if (enemyHealth.IsDead()) // Kiem tra xem Enemy da chet chua
        {
            Death(); // Neu Enemy da chet, goi ham Death
        }
        EnterBattleMode(); // Bat dau che do chien dau khi Enemy bi tan cong
     
    }
    public virtual void MeleeAttackCheck(Transform[]damagePoints, float attackCheckRadius,GameObject vfx,int dame)
    {
        if (isMeleeAttackReady == false) return; // If not ready to attack, do nothing
        foreach (Transform attackPoint in damagePoints)
        {
            Collider[] detectedHit = Physics.OverlapSphere(attackPoint.position, attackCheckRadius, whatIsPlayer); // Detect player within the attack radius
            for (int i = 0; i < detectedHit.Length; i++)
            {
                IDamagable damage = detectedHit[i].GetComponent<IDamagable>(); // Get the IDamagable component from the detected hit
                if (damage != null)
                {
                    damage.TakeDamage(dame); // Apply damage to the player
                    isMeleeAttackReady = false; // Reset attack readiness after attacking
                    GameObject newAttackVfx = ObjectPooling.Instance.GetObject(vfx, attackPoint); // Get a new attack VFX from the object pool
                    ObjectPooling.Instance.ReturnObject(newAttackVfx, 1f); // Return the VFX to the pool after 1 second
                    return;
                }
            }
        }

    }
    public void EnableAttackCheck(bool active)
    {
        isMeleeAttackReady = active; // Enable or disable the attack check
    }
    public virtual void Death()
    {
       
    }
    public virtual void HitImpact(Vector3 force, Vector3 hitPoint, Rigidbody rb)
    {
        if(enemyHealth.IsDead()) // Kiem tra xem Enemy da chet chua
        {
            StartCoroutine(HitImpactCouroutine(force, hitPoint, rb)); // Bat dau Coroutine de xu ly luc va vi tri khi Enemy bi tan cong

        }
    }
    private IEnumerator HitImpactCouroutine(Vector3 force, Vector3 hitPoint, Rigidbody rb)
    {
        yield return new WaitForSeconds(0.1f); // Delay to allow the hit impact to be processed
        rb.AddForceAtPosition(force, hitPoint, ForceMode.Impulse); // de them luc va vi tri de Enemy bi anh huong khi bi tan cong

    }
    public bool IsPlayerInAggressiveRange() //Su dung doi voi enemyRange
    {
        return Vector3.Distance(transform.position, player.position) < aggressiveRange; // Kiem tra xem Player co trong khoang cach tan cong toi Enemy khong
    }

}
