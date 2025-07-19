using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsController : MonoBehaviour
{
    public static ControlsController Instance;
    public PlayerControls controls { get; private set; }
    private Player player;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        // Initialize controls or settings if needed
        controls = GameManager.instance.player.controls;
        player = GameManager.instance.player;
        SwitchToCharacterControls(); // Start with character controls enabled
    }
    public void SwitchToCharacterControls()
    {
        // Switch to character controls
        controls.UI.Disable();
        controls.Character.Enable();
        player.SetControlsEnableTo(true);

    }
    public void SwitchToUIControls()
    {
        // Switch to UI controls
        controls.Character.Disable();
        controls.UI.Enable();
        player.SetControlsEnableTo(false);
    }
}
