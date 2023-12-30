using System.Collections;
using UnityEngine;


public class FirstPersonController : MonoBehaviour
{
    public bool CanMove { get; private set; } = true;
    private bool IsSprinting => canSprint && Input.GetKey(sprintKey);
    private bool ShouldJump => Input.GetKeyDown(jumpKey) && characterController.isGrounded;
    private bool ShouldCrouch => Input.GetKeyDown(crouchKey) && !duringCrouchAnimation && characterController.isGrounded;

    [Header("Functional Options")]
    [SerializeField] private bool canSprint = true;
    [SerializeField] private bool canJump = true;
    [SerializeField] private bool canCrouch = true;
    [SerializeField] private bool canUseHeadbob = true;
    [SerializeField] private bool WillSlideOnSlopes = true; 
    [SerializeField] private bool canZoom = true;
    [SerializeField] private bool canInteract = true;
    [SerializeField] private bool useFootsteps = true;



    [Header("Controls")]
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode crouchKey = KeyCode.LeftControl;
    [SerializeField] private KeyCode zoomKey = KeyCode.Mouse1;
    [SerializeField] private KeyCode interactKey = KeyCode.Mouse0;

    [Header("Movement Parameters")]
    [SerializeField] private float walkSpeed = 8.0f;
    [SerializeField] private float sprintSpeed = 13.0f;
    [SerializeField] private float crouchSpeed = 4.0f;
    [SerializeField] private float slopeSpeed = 8f;

    [Header ("Animation")]
    private bool isWalking;
    private bool isSprinting;
    //private bool isCrouched;
    public bool isIdle;

    private Animator playerAnimator;



    [Header("Look Parameter")]
    [SerializeField, Range(1, 10)] private float lookSpeedX = 2.0f;
    [SerializeField, Range(1, 10)] private float lookSpeedY = 2.0f;
    [SerializeField, Range(1, 180)] private float upperLookLimit = 80.0f;
    [SerializeField, Range(1, 180)] private float lowerLookLimit = 80.0f;

    [Header("Jumping Parameters")]
    [SerializeField] private float jumpForce = 8.0f;
    [SerializeField] private float gravity = 30.0f;

    [Header("Crouch Parameters")]
    [SerializeField] private float crouchHeight = 0.5f;
    [SerializeField] private float standingHeight = 2f;
    [SerializeField] private float timeToCrouch = 0.25f;
    [SerializeField] private Vector3 crouchingCenter = new Vector3(0,0.5f,0);
    [SerializeField] private Vector3 standingCenter = new Vector3(0,0,0);
    private bool isCrouching;
    private bool duringCrouchAnimation;

    [Header("Headbob Parameters")]
    [SerializeField] private float walkBobSpeed = 14f;
    [SerializeField] private float walkBobAmount = 0.05f;
    [SerializeField] private float sprintBobSpeed = 18f;
    [SerializeField] private float sprintBobAmount = 0.11f;
    [SerializeField] private float crouchBobSpeed = 8f;
    [SerializeField] private float crouchBobAmount = 0.025f;
    private float defaultYPos = 0;
    private float timer;

    [Header("Zoom Parameters")]
    [SerializeField] private float timeToZoom = 0.3f;
    [SerializeField] private float zoomFOV = 30f;
    private float defaultFOV;
    private Coroutine zoomRoutine;

    [Header("Footstep Parameters")]
    [SerializeField] private float baseStepSpeed = 0.5f;
    [SerializeField] private float crouchStepMultiplier = 1.5f;
    [SerializeField] private float sprintStepMultiplier = 0.6f;
    [SerializeField] private AudioSource footstepAudioSource = default;
    [SerializeField] private AudioClip[] woodClips = default;
    [SerializeField] private AudioClip[] grassClips = default;
    private float footstepTimer = 0;
    private float GetCurrentOffset => isCrouching ? baseStepSpeed * crouchStepMultiplier : IsSprinting ? baseStepSpeed * sprintStepMultiplier : baseStepSpeed;
    private bool isPlayingFootstep = false; // Add this variable to track whether a footstep sound is currently playing 


    // SLIDING PARAMETERS

    private Vector3 hitPointNormal;

    private bool IsSliding
    {
        get
        {
            if(characterController.isGrounded && Physics.Raycast(transform.position, Vector3.down, out RaycastHit slopeHit, 2f))
            {
                hitPointNormal = slopeHit.normal;
                return Vector3.Angle(hitPointNormal, Vector3.up) > characterController.slopeLimit;
            }
            else
            {
                return false;
            }
        }
    }

    [Header("Interaction")]
    [SerializeField] private Vector3 interactionRayPoint = default;
    [SerializeField] private float interactionDistance = default;
    [SerializeField] private LayerMask interactionLayer = default;
    private Interactable currentInteractable;
 


    private Camera playerCamera;
    private CharacterController characterController;

    private Vector3 moveDirection;
    private Vector2 currentInput;

    private float rotationX = 0;

    public static FirstPersonController instance;

    void Awake()
    {
        instance = this;

        playerCamera = GetComponentInChildren<Camera>();
        characterController = GetComponent<CharacterController>();
        defaultYPos = playerCamera.transform.localPosition.y;
        defaultFOV = playerCamera.fieldOfView;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playerAnimator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
    }

    void Update()
    {
        if (CanMove)
        {
            HandleMovementInput();
            HandleMouseLook();

            if(canJump)
                HandleJump();
            if (canCrouch)
                HandleCrouch();
            if(canUseHeadbob)
                HandleHeadbob();
            if (canZoom)
                HandleZoom();
            if (useFootsteps)
                Handle_Footsteps();
            if (canInteract)
            {
                HandleInteractionCheck();
                HandleInteractionInput();
            }

            ApplyFinalMovements();

          
            UpdateMovementFlags();

            UpdateAnimator();

        }
    }

    private void UpdateMovementFlags()
    {
        isWalking = !isSprinting && !isCrouching && currentInput.magnitude > 0;
        isSprinting = IsSprinting && currentInput.magnitude > 0;
        isCrouching = isCrouching && currentInput.magnitude >= 0;
        isIdle = !isWalking && !isSprinting && !isCrouching;
    }

    private void UpdateAnimator()
    {
        // Set the animator bool parameters based on the movement state
        playerAnimator.SetBool("isWalking", isWalking);
        playerAnimator.SetBool("isSprinting", isSprinting);
        playerAnimator.SetBool("isCrouching", isCrouching);
        playerAnimator.SetBool("isIdle", isIdle);
    }


    private void HandleMovementInput()
    {
        currentInput = new Vector2((isCrouching ? crouchSpeed :IsSprinting ? sprintSpeed : walkSpeed) * Input.GetAxis("Vertical"), (isCrouching ? crouchSpeed : IsSprinting ? sprintSpeed : walkSpeed) * Input.GetAxis("Horizontal"));

        float moveDirectionY = moveDirection.y;
        moveDirection = (transform.TransformDirection(Vector3.forward) * currentInput.x) + (transform.TransformDirection(Vector3.right) * currentInput.y);
        moveDirection.y = moveDirectionY;
    }

    private void HandleMouseLook()
    {
        rotationX -= Input.GetAxis("Mouse Y") * lookSpeedY;
        rotationX = Mathf.Clamp(rotationX, -upperLookLimit, lowerLookLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeedX, 0);
    }

    private void HandleJump()
    {
        if (ShouldJump)
            moveDirection.y = jumpForce;
    }

    private void HandleCrouch()
    {
        if (ShouldCrouch)
            StartCoroutine(CrouchStand());
                
    }

    private void HandleHeadbob()
    {
        if (!characterController.isGrounded) return;

        if (Mathf.Abs(moveDirection.x) > 0.1f || Mathf.Abs(moveDirection.z) > 0.1f)
            {
            timer += Time.deltaTime * (isCrouching ? crouchBobSpeed : IsSprinting ? sprintBobSpeed : walkBobSpeed);
            playerCamera.transform.localPosition = new Vector3(
                playerCamera.transform.localPosition.x,
                defaultYPos + Mathf.Sin(timer) * (isCrouching ? crouchBobAmount : IsSprinting ? sprintBobAmount : walkBobAmount),
                playerCamera.transform.localPosition.z);
        }
    }

    private void HandleZoom()
    {
        if (Input.GetKeyDown(zoomKey))
        {
            if(zoomRoutine != null)
            {
                StopCoroutine(zoomRoutine);
                zoomRoutine = null;
            }
            zoomRoutine = StartCoroutine(ToggleZoom(true));
        }
        if (Input.GetKeyUp(zoomKey))
        {
            if (zoomRoutine != null)
            {
                StopCoroutine(zoomRoutine);
                zoomRoutine = null;
            }

            zoomRoutine = StartCoroutine(ToggleZoom(false));
        }
    }

    private void HandleInteractionCheck()
    {
        Ray interactionRay = playerCamera.ViewportPointToRay(interactionRayPoint);

        if (Physics.Raycast(interactionRay, out RaycastHit hit, interactionDistance, interactionLayer))
        {
            Interactable newInteractable = hit.collider.GetComponent<Interactable>();

            if (newInteractable != null && newInteractable != currentInteractable)
            {
                // Lose focus on the current interactable if any
                if (currentInteractable != null)
                    currentInteractable.OnLoseFocus();

                currentInteractable = newInteractable;
                currentInteractable.OnFocus();
            }
        }
        else if (currentInteractable != null)
        {
            // If no interactable object is hit, lose focus on the current interactable
            currentInteractable.OnLoseFocus();
            currentInteractable = null;
        }
    }


    private void HandleInteractionInput()
    {
        if (Input.GetKeyDown(interactKey) && currentInteractable != null && Physics.Raycast(playerCamera.ViewportPointToRay(interactionRayPoint), out RaycastHit hit, interactionDistance, interactionLayer))
            {
                currentInteractable.OnInteract();
                
            }
    }

   

    private void Handle_Footsteps()
    {
        if (!characterController.isGrounded) return;
        if (currentInput == Vector2.zero) return;

        footstepTimer -= Time.deltaTime;

        if (footstepTimer <= 0 && !isPlayingFootstep)
        {
            if (Physics.Raycast(playerCamera.transform.position, Vector3.down, out RaycastHit hit, 3))
            {
                switch (hit.collider.tag)
                {
                    case "Footsteps/Wood":
                        StartCoroutine(PlayFootstep(woodClips[Random.Range(0, woodClips.Length - 1)]));
                        break;
                    case "Footsteps/Grass":
                        StartCoroutine(PlayFootstep(grassClips[Random.Range(0, grassClips.Length - 1)]));
                        break;
                    default:
                        StartCoroutine(PlayFootstep(woodClips[Random.Range(0, woodClips.Length - 1)]));
                        break;
                }
            }
        }
    }

    private IEnumerator PlayFootstep(AudioClip clip)
    {
        isPlayingFootstep = true;
        footstepAudioSource.PlayOneShot(clip);

        // wait for the length of the footstep sound before allowing another footstep
        yield return new WaitForSeconds(clip.length);

        isPlayingFootstep = false;
    }

    private void ApplyFinalMovements()
    {
        if (!characterController.isGrounded)
        
            moveDirection.y -= gravity * Time.deltaTime;

            characterController.Move(moveDirection * Time.deltaTime);

        if (WillSlideOnSlopes && IsSliding)
            moveDirection += new Vector3(hitPointNormal.x, -hitPointNormal.y, hitPointNormal.z) * slopeSpeed;

        if (characterController.velocity.y < - 1 && characterController.isGrounded)
            moveDirection.y = 0;

    }




    private IEnumerator CrouchStand()
    {
        if (isCrouching && Physics.Raycast(playerCamera.transform.position, Vector3.up, 1f))
            yield break;

        duringCrouchAnimation = true;

        float timeElapsed = 0;
        float targetHeight = isCrouching ? standingHeight : crouchHeight;
        float currentHeight = playerCamera.transform.localPosition.y;
        Vector3 targetCenter = isCrouching ? standingCenter : crouchingCenter;
        Vector3 currentCenter = playerCamera.transform.localPosition;

        // Perform crouch action

        while (timeElapsed < timeToCrouch)
        {
            playerCamera.transform.localPosition = new Vector3(
                currentCenter.x,
                Mathf.Lerp(currentHeight, targetHeight, timeElapsed / timeToCrouch),
                currentCenter.z
            );
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        playerCamera.transform.localPosition = new Vector3(
            currentCenter.x,
            targetHeight,
            currentCenter.z
        );

        isCrouching = !isCrouching;

        duringCrouchAnimation = false;
    }


    private IEnumerator ToggleZoom (bool isEnter)
    {
        float targetFOV = isEnter ? zoomFOV : defaultFOV; 
        float startingFOV = playerCamera.fieldOfView;
        float timeElapsed = 0;

        while(timeElapsed < timeToZoom)
        {
            playerCamera.fieldOfView = Mathf.Lerp(startingFOV, targetFOV, timeElapsed / timeToZoom);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        playerCamera.fieldOfView = targetFOV;
        zoomRoutine = null;
    }
}

