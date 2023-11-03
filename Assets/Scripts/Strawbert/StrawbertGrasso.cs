using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class StrawbertGrasso : MonoBehaviour {
    [SerializeField] StrawbertBehavior strawbert;
    [SerializeField] GameObject target;
    [SerializeField] public float rotationSpeed;

    public PlayerInputActions playerInputActions;
    private bool mouseControl;

    public bool canGrasso { get; set; } = true;

    float xInput = 0;
    float yInput = 1f;

    private void Awake()
    {
        playerInputActions = InputManager.inputActions;
    }

    private void Start()
    {
        InputManager.ChangeGrassoControls(playerInputActions.GrassoAimV1);
    }

    void Update() {

        if (playerInputActions.Player.Option1.triggered) {
            EventBroker.CallSendFeedback("Hold right trigger + left stick aim");
            InputManager.ChangeGrassoControls(playerInputActions.GrassoAimV1);
        } else if (playerInputActions.Player.Option2.triggered) {
            EventBroker.CallSendFeedback("Hold right trigger + right stick aim");
            InputManager.ChangeGrassoControls(playerInputActions.GrassoAimV2);
        } else if (playerInputActions.Player.Option3.triggered) {
            EventBroker.CallSendFeedback("Tap X + right stick aim");
            InputManager.ChangeGrassoControls(playerInputActions.GrassoAimV3);
        } else if (playerInputActions.Player.Option4.triggered) {
            EventBroker.CallSendFeedback("Move mouse");
        }

        if (canGrasso && (PressedButtonToAim())){
            StartCoroutine("UseGrasso");
        }
    }

    IEnumerator UseGrasso() {
        SwingGrasso();
        
        yield return 0;
        while (!PressedButtonToShoot()) { 
            
            if (Keyboard.current.spaceKey.isPressed) {
                mouseControl = true;
            }

            AimGrassoMouse(mouseControl);
            AimGrassoJoystick(mouseControl);

            if (playerInputActions.Player.SecondaryAction.triggered) {
                mouseControl = false;
                EndGrasso();
            }
            yield return null;
        }

        mouseControl = false;
        ShootGrasso();
    }

    private void SwingGrasso() {
        canGrasso = false;
        target.SetActive(true);
        strawbert.flower.SetObjectActive(true);
        strawbert.animator.SetAnimatorBool("Swinging", true);
    }

    private void AimGrassoJoystick(bool mouseInUse) {

        if (mouseInUse)
        {
            return;
        }
        else
        {
            SetJoystickInput();

            float angle = Mathf.Atan2(yInput, xInput) * Mathf.Rad2Deg;
            target.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }

    private void AimGrassoMouse(bool mouseInUse) {
        if (!mouseInUse)
        {
            return;
        }
        else
        {
            Vector3 mousePos = playerInputActions.Player.SecondaryMovement.ReadValue<Vector2>();
            Vector3 targetPos = Camera.main.WorldToScreenPoint(target.transform.position);
            mousePos.x = mousePos.x - targetPos.x;
            mousePos.y = mousePos.y - targetPos.y;
            float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
            target.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }

    private void ShootGrasso() {
        strawbert.movement.canMove = false;
        strawbert.movement.StopMovement();
        strawbert.animator.SetAnimatorBool("Shooting", true);
        strawbert.flower.Shoot();
        // EndGrasso();
    }

    public void EndGrasso() {
        StopCoroutine("UseGrasso");
        target.SetActive(false);

        canGrasso = true;
        strawbert.movement.canMove = true;
        strawbert.animator.EndGrassoAnimation();
    }

    bool PressedButtonToAim()
    {
        if ((playerInputActions.GrassoAimV1.enabled &&
                 playerInputActions.GrassoAimV1.Grasso.WasPressedThisFrame()) ||
                 (playerInputActions.GrassoAimV2.enabled &&
                 playerInputActions.GrassoAimV2.Grasso.WasPressedThisFrame()) ||
                 (playerInputActions.GrassoAimV3.enabled &&
                 playerInputActions.GrassoAimV3.Grasso.triggered)
            )
        {
            return true;
        }

        return false;
    }


    bool PressedButtonToShoot()
    {
        if ((playerInputActions.GrassoAimV1.enabled &&
                playerInputActions.GrassoAimV1.Grasso.WasReleasedThisFrame()) ||
                (playerInputActions.GrassoAimV2.enabled &&
                playerInputActions.GrassoAimV2.Grasso.WasReleasedThisFrame()) ||
                (playerInputActions.GrassoAimV3.enabled &&
                playerInputActions.GrassoAimV3.Grasso.triggered)
           )
        {
            return true;
        }

        return false;
    }


    void SetJoystickInput()
    {
        Vector2 inputVector;


        if (playerInputActions.GrassoAimV1.enabled)
        {
            inputVector = playerInputActions.Player.Movement.ReadValue<Vector2>();
        }
        else if (playerInputActions.GrassoAimV2.enabled)
        {
            inputVector = playerInputActions.GrassoAimV2.GrassoDirection.ReadValue<Vector2>();
        }
        else if (playerInputActions.GrassoAimV3.enabled)
        {
            inputVector = playerInputActions.GrassoAimV3.GrassoDirection.ReadValue<Vector2>();
        }
        else
        {
            inputVector = Vector2.zero;
            Debug.Log("No input available");
        }

        float xInputRaw = inputVector.x; //Input.GetAxisRaw(xAxis);
        float yInputRaw = inputVector.y; //Input.GetAxisRaw(yAxis);

        if (xInputRaw != 0 || yInputRaw != 0) {
            xInput = xInputRaw;
            yInput = yInputRaw;
        }
    }
}
