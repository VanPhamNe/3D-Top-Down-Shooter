using UnityEngine;
public class PlayerMove : MonoBehaviour
{
    private Player player;
    private PlayerControls controls;
    private CharacterController characterController;
    public Vector2 moveInput { get; private set; } //read only, chi doc duoc gia tri, khong the gan gia tri moi
    private Animator animator; 

    [Header("Movement")]
    public Vector3 moveDirection;
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed;
    [SerializeField] private float turnSpeed; //toc do quay nhan vat
    private float speed;
    private float verticalVelocity;
    private bool isRunning;

    private AudioSource walkSFX;
    private AudioSource runSFX;
    

    private void Start()
    {
        player = GetComponent<Player>(); //lay tham chieu den Player script  
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    
        speed = walkSpeed; //mac dinh toc do di chuyen la toc do di bo
    
        walkSFX = player.soundSFX.walkSFX; //lay tham chieu den walkSFX trong Player_SoundSFX script
        runSFX = player.soundSFX.runSFX; //lay tham chieu den runSFX trong Player_SoundSFX script
        InputEvents();
     
    }
    private void Update()
    {
        if(player.heath.isDead) //kiem tra xem nhan vat da chet chua
        {
            return; //neu da chet thi khong thuc hien tiep
        }
        ApplyMovement();
        AimMouse();
        AnimatorController();
    }
    private void AnimatorController()
    {
        float xVelocity = Vector3.Dot(moveDirection.normalized, transform.right); //tinh toan van toc theo truc x
        float zVelocity = Vector3.Dot(moveDirection.normalized, transform.forward); //tinh toan van toc theo truc z
        animator.SetFloat("xVelocity", xVelocity,.1f,Time.deltaTime); //cap nhat bien XVelocity trong Animator
        animator.SetFloat("zVelocity", zVelocity,.1f,Time.deltaTime); //cap nhat bien ZVelocity trong Animator
        bool playRunAnimation = isRunning && moveInput.magnitude > 0; //kiem tra xem co dang chay va co input chon huong hay khong
        animator.SetBool("isRun", playRunAnimation); //cap nhat bien isRunning trong Animator
    }
    private void AimMouse()
    {
      
         Vector3 lookingDir = player.playerAim.GetMouseHitInfo().point - transform.position;
         lookingDir.y = 0; // chi lay phan x, z
         lookingDir.Normalize(); // chuan hoa vector
         //transform.forward = lookingDir; // quay nhan vat ve huong chuot
         Quaternion rotation = Quaternion.LookRotation(lookingDir); //tao quaternion tu huong chuot
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, turnSpeed * Time.deltaTime); //dung Slerp de quay nhan vat tu tu ve huong chuot

    }

    private void ApplyMovement()
    {
        moveDirection = new Vector3(moveInput.x, 0, moveInput.y);
        ApplyGravity();
        if (moveDirection.magnitude > 0)
        {
            PlayFootstepsSound();
            characterController.Move(moveDirection * Time.deltaTime * speed);
        }
        else
        {
            StopFootstepSound(); // dung am thanh chan khi khong di chuyen
        }
    }

    private void PlayFootstepsSound()
    {
        if (isRunning)
        {
            if (runSFX.isPlaying == false)
            {
                runSFX.Play();
            }
            else
            {
                if (walkSFX.isPlaying == false)
                {
                    walkSFX.Play();
                }
            }
        }
    }
    private void StopFootstepSound()
    {
        walkSFX.Stop();
        runSFX.Stop();
    }

    private void ApplyGravity()
    {
        if(!characterController.isGrounded)
        {
            verticalVelocity = verticalVelocity - 9.81f * Time.deltaTime; //-9.81f is the gravity value in Unity
            moveDirection.y = verticalVelocity;
        }
        else
        {
            verticalVelocity = -.5f;
           
        }
        
    }
    private void InputEvents()
    {
        controls = player.controls;
        controls.Character.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Character.Movement.canceled += ctx =>
        {
            StopFootstepSound(); // dung am thanh chan khi khong di chuyen
            moveInput = Vector2.zero; //khi nhan phim roi thi set lai input ve 0
          
        };
  
        controls.Character.Run.performed += ctx =>
        {
            speed = runSpeed; //thay doi toc do di chuyen khi bam phim
            isRunning = true;
        };
        controls.Character.Run.canceled += ctx =>
        {
            speed = walkSpeed;
            isRunning = false;
        };
    }


}
