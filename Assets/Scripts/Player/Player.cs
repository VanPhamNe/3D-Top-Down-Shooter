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
    public Player_SoundSFX soundSFX { get; private set; } //read only khong thay doi gia tri
    public bool controlsEnabled { get; private set; } //kiem soat xem controls co duoc kich hoat hay khong
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
        soundSFX = GetComponent<Player_SoundSFX>();
    }
    private void OnEnable()
    {
        controls.Enable();
        controls.Character.UIPause.performed += ctx => UI.Instance.PauseSwitch(); //Dang ky su kien khi nhan nut UIPause
    }
    private void OnDisable()
    {
        controls.Disable();
    }
    public void SetControlsEnableTo(bool enable)
    {
        controlsEnabled = enable; //cap nhat gia tri controlsEnabled
    }

}
