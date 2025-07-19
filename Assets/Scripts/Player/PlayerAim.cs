using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    private Player player;
    private PlayerControls controls;
    private Vector2 aimInput; //gia tri toa do chuot
    private bool hasHit = false;
    [Header("Aim Info")]
    [SerializeField] private Transform aim; //vi tri aim ma nhan vat nham toi
    [SerializeField] private bool AimPrecision; //nham chinh xac
    [SerializeField] private bool isLockingTarget; //kiem tra xem co dang lock target hay khong

    [Header("Aim Settings")]
    [SerializeField] private LineRenderer aimLineRenderer; //line renderer de ve tia laser huong sung

    [Header("Camera Info")]
    [SerializeField] private Transform cameraTarget; //vi tri ma nhan vat dang nhin den
    [Range(0.5f, 1)]
    [SerializeField] private float minCameraDistance; // khoang cach toi thieu cua camera
    [Range(1,3f)]
    [SerializeField] private float maxCameraDistance; // khoang cach toi da cua camera
    [Range(3,5)]
    [SerializeField] private float aimSensitivity = 5f; // do nhay cua aim
    [SerializeField] private LayerMask aimLayerMask;
   
    private RaycastHit lastKnownHit; //luu lai vi tri cuoi cung ma chuot cham vao vat the
    private void Start()
    {
        player = GetComponent<Player>();
        InputEvents();
    }
    private void Update()
    {
        if (player.heath.isDead)
        {
            return;
        }
        if(player.controlsEnabled == false) // neu controls khong duoc kich hoat thi khong cho aim
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            AimPrecision = !AimPrecision;
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            isLockingTarget = !isLockingTarget;
        }
        UpdateAimLaser(); //Cap nhat tia huong laser 
        UpdateAimPosition(); //cap nhat vi tri diem aim

        //aim.position = new Vector3(aim.position.x, transform.position.y + 1, aim.position.z); //dat vi tri y cua aim bang 1 de khong bi cham dat
        UpdateCameraPosition(); // cap nhat vi tri camera theo vi tri aim
    }

    private void UpdateCameraPosition()
    {
        cameraTarget.position = Vector3.Lerp(cameraTarget.position, DesiredCameraPosition(), aimSensitivity * Time.deltaTime); //dung Lerp de lam muot qua trinh aim
    }

    private void UpdateAimPosition()
    {
        Transform target = Target(); //xem con tro chuot co tro vao target nao khong (Enemy)
        if (target != null && isLockingTarget) // neu target khong null
        {
            if (target.GetComponent<Renderer>() != null) //kiem tra xem target co Renderer khong
            {
                aim.position = target.GetComponent<Renderer>().bounds.center; //cap nhat vi tri aim theo Renderer bounds center cua target

            }
            else
            {
                aim.position = target.position; //cap nhat vi tri aim theo target
            }
            return;
        }
        aim.position = GetMouseHitInfo().point; //cap nhat vi tri aim theo vi tri chuot
        if (!AimPrecision)
        {
            aim.position = new Vector3(aim.position.x, transform.position.y + 1, aim.position.z); //dat vi tri y cua aim bang 1 de khong bi cham dat

        }
    }
    private void UpdateAimLaser()
    {
        aimLineRenderer.enabled = player.weaponController.IsWeaponReady();
        if (!aimLineRenderer.enabled) // neu vu khi khong san sang thi khong ve tia laser
            return;
        WeaponModel weaponModel = player.weaponVisual.GetCurrentWeaponModel(); //lay weapon model hien tai
        weaponModel.transform.LookAt(aim.position); // quay weapon model ve huong aim
        weaponModel.gunPoint.LookAt(aim.position); // quay gun point ve huong aim
        float tipLenght = 0.5f; //do dai tip cua laser
        float gunDistance = player.weaponController.GetCurrentWeapon().gunDistance;
        Transform gunPoint =player.weaponController.GetGunPoint(); //lay vi tri xuat dan
        Vector3 laserDirection = player.weaponController.BulletDirection(); //lay huong dan cua dan se bay toi
       
        aimLineRenderer.SetPosition(0, gunPoint.position); //dat vi tri bat dau cua line renderer
        Vector3 endPoint = gunPoint.position + laserDirection * gunDistance;
        if(Physics.Raycast(gunPoint.position, laserDirection, out RaycastHit hitInfo, gunDistance)) //kiem tra xem co cham vao vat the khong
        {
            endPoint = hitInfo.point; // neu co cham vao vat the thi cap nhat vi tri ket thuc cua line renderer
            tipLenght = 0;
        }
        aimLineRenderer.SetPosition(1, endPoint); //dat vi tri ket thuc cua line renderer
        aimLineRenderer.SetPosition(2, endPoint + laserDirection * tipLenght); //dat vi tri tip cua line renderer
    }
    public bool CanAimPrecision() //kiem tra xem co the aim chinh xac hay khong
    {
        if (AimPrecision)
            return true;
        return false;
    }
    private void InputEvents()
    {
        controls = player.controls;
        controls.Character.Aim.performed += ctx => aimInput = ctx.ReadValue<Vector2>();
        controls.Character.Aim.canceled += ctx => aimInput = Vector2.zero;
    }
    public Transform Aim() => aim; //tra ve vi tri aim cua nhan vat
    public RaycastHit GetMouseHitInfo()
    {
        Ray ray = Camera.main.ScreenPointToRay(aimInput); //ban ray tu camera  theo huong chuot
        if (Physics.Raycast(ray, out var hit, Mathf.Infinity,aimLayerMask)) // neu ma ray do va cham 1 vat the tra ve toa do diem hit point
        {
            lastKnownHit = hit; // luu lai vi tri cham cuoi cung
            hasHit = true;
            return hit; // Tra ve vi tri va cham voi mat phang
        }
        if (hasHit) return lastKnownHit;
        return default;
    }
    private Vector3 DesiredCameraPosition() {
        float actualMaxCameraDistance;
        bool moveDownards = player.movement.moveInput.y < .5f;
        if(moveDownards) // neu di chuyen lui muc tieu
        {
            actualMaxCameraDistance = minCameraDistance; // khoang cach toi da cua camera
        }
        else // neu di chuyen toi
        {
            actualMaxCameraDistance = maxCameraDistance; // khoang cach toi thieu cua camera
        }
        Vector3 desiredCameraPosition = GetMouseHitInfo().point; // lay diem ma chuot dang nham toi
        Vector3 aimDirection = (desiredCameraPosition - transform.position).normalized; //tinh toan huong aim tu vi tri nhan vat toi vi tri chuot
        float distance = Vector3.Distance(transform.position, desiredCameraPosition); // tinh toan khoang cach that su+ tu vi tri nhan vat toi vi tri chuot
        float clampedDistance = Mathf.Clamp(distance, minCameraDistance, maxCameraDistance); //dung Clamp de gioi han khoang cach toi da va toi thieu
        desiredCameraPosition = transform.position + aimDirection * clampedDistance; //tinh toan vi tri aim moi

        desiredCameraPosition.y = transform.position.y + 1;

        return desiredCameraPosition;
    }
    public Transform Target()
    {
        Transform target = null;
        //if(GetMouseHitInfo().transform.GetComponent<Target>() != null)
        //{
        //    target = GetMouseHitInfo().transform;

        //}
        RaycastHit hit = GetMouseHitInfo(); 

        if (hit.transform != null) 
        {
            if (hit.transform.GetComponent<Target>() != null)
            {
                target = hit.transform;
            }
        }
        return target;
    }
}
