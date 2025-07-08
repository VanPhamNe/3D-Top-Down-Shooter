using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform playerBody;
    public PlayerControls controls { get; private set; }
    public PlayerAim playerAim { get; private set; } //read only khong thay doi gia tri
    public PlayerMove movement { get; private set; } //read only khong thay doi gia tri
    public PlayerWeaponController weaponController { get; private set; } //read only khong thay doi gia tri
    public PlayerWeaponVisual weaponVisual { get; private set; } //read only khong thay doi gia tri
    public PlayerInteraction interaction { get; private set; } //read only khong thay doi gia tri
    public PlayerHeath heath { get; private set; } //read only khong thay doi gia tri
    public Ragdoll ragdoll { get; private set; } //read only khong thay doi gia tri
    public Animator animator { get; private set; } //read only khong thay doi gia tri
    private void Awake()
    {
        controls = new PlayerControls();
       playerAim = GetComponent<PlayerAim>();
        movement = GetComponent<PlayerMove>();
        weaponController = GetComponent<PlayerWeaponController>();
        weaponVisual = GetComponent<PlayerWeaponVisual>();
        interaction = GetComponent<PlayerInteraction>();
        heath = GetComponent<PlayerHeath>();
        ragdoll = GetComponent<Ragdoll>();
        animator = GetComponentInChildren<Animator>();
    }
    private void OnEnable()
    {
        controls.Enable();
    }
    private void OnDisable()
    {
        controls.Disable();
    }

}
