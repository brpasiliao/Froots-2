using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrawbertGrasso : MonoBehaviour {
    [SerializeField] StrawbertBehavior strawbert;
    [SerializeField] GameObject target;
    [SerializeField] GameObject flower;
    [SerializeField] public float rotationSpeed;

    public PlayerInputActions playerInputActions;

    public bool canGrasso { get; set; } = true;

    string currentGrassoAim = "GrassoAimV1";
    float xInput = 0;
    float yInput = 1f;

    private void Awake()
    {
        playerInputActions = InputManager.inputActions;
        //playerInputActions = new PlayerInputActions();
    }

    void Update() {
        // if (canGrasso && Input.GetButtonDown("Grasso")) {
        //     StartCoroutine("UseGrasso");
        // }

        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            currentGrassoAim = "GrassoAimV1";
            EventBroker.CallSendFeedback("Hold right trigger + left stick aim");
            //InputManager.ToggleActionMap(playerInputActions.GrassoAimV1);
            InputManager.ChangeGrassoControls(playerInputActions.GrassoAimV1);
        } else if (Input.GetKeyDown(KeyCode.Alpha2)) {
            currentGrassoAim = "GrassoAimV2";
            EventBroker.CallSendFeedback("Hold right trigger + right stick aim");
            //InputManager.ToggleActionMap(playerInputActions.GrassoAimV2);
            InputManager.ChangeGrassoControls(playerInputActions.GrassoAimV2);
        } else if (Input.GetKeyDown(KeyCode.Alpha3)) {
            currentGrassoAim = "GrassoAimV3";
            EventBroker.CallSendFeedback("Tap X + right stick aim");
            //InputManager.ToggleActionMap(playerInputActions.GrassoAimV3);
            InputManager.ChangeGrassoControls(playerInputActions.GrassoAimV3);
        } else if (Input.GetKeyDown(KeyCode.Alpha4)) {
            currentGrassoAim = "Mouse";
            EventBroker.CallSendFeedback("Move mouse");
        }

        if (canGrasso && (PressedButtonToAim())){  //|| playerInputActions.Player.Grasso.triggered)) {
            StartCoroutine("UseGrasso");
        }
    }

    IEnumerator UseGrasso() {
        SwingGrasso();
        
        yield return 0;
        while (!PressedButtonToShoot()) {  //&& !playerInputActions.Player.Grasso.triggered) {
            if (currentGrassoAim.Equals("Mouse")) {
                AimGrassoMouse();
            } else {
                AimGrassoJoystick();
            }

            if (Input.GetButtonDown("Cancel")) {
                EndGrasso();
            }

            yield return null;
        }

        ShootGrasso();
    }

    private void SwingGrasso() {
        canGrasso = false;
        target.SetActive(true);
        flower.SetActive(true);
        strawbert.animator.SetAnimatorBool("Swinging", true);
    }

    private void AimGrassoJoystick() {
        if (currentGrassoAim.Equals("GrassoAimV1")) {
            SetJoystickInput("Horizontal", "Vertical");
        } else {
            SetJoystickInput("Right Horizontal", "Right Vertical");
        }

        float angle = Mathf.Atan2(yInput, xInput) * Mathf.Rad2Deg;
        target.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private void AimGrassoMouse() {
        Vector3 mousePos = Input.mousePosition;
        Vector3 targetPos = Camera.main.WorldToScreenPoint(target.transform.position);
        mousePos.x = mousePos.x - targetPos.x;
        mousePos.y = mousePos.y - targetPos.y;
        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        target.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private void ShootGrasso() {
        strawbert.movement.canMove = false;
        strawbert.movement.StopMovement();
        strawbert.animator.SetAnimatorBool("Shooting", true);
    }

    public void EndGrasso() {
        StopCoroutine("UseGrasso");
        target.SetActive(false);

        canGrasso = true;
        strawbert.movement.canMove = true;
        strawbert.animator.EndGrassoAnimation();
    }

    /*bool PressedButtonToAim() {
        if ((currentGrassoAim.Equals("GrassoAimV1") && 
                Input.GetButtonDown("Right Trigger")) ||
                (currentGrassoAim.Equals("GrassoAimV2") &&
                Input.GetButtonDown("Right Trigger")) ||
                (currentGrassoAim.Equals("GrassoAimV3") &&
                Input.GetButtonDown("Grasso")) ||
                (currentGrassoAim.Equals("Mouse") &&
                Input.GetButtonDown("Grasso"))
           ) {
            return true;
           }
        
        return false;
    }*/

    bool PressedButtonToAim()
    {
        if ((playerInputActions.GrassoAimV1.enabled &&
                 playerInputActions.GrassoAimV1.Grasso.WasPressedThisFrame()) ||
                 (playerInputActions.GrassoAimV2.enabled &&
                 playerInputActions.GrassoAimV2.Grasso.WasPressedThisFrame()) ||
                 (playerInputActions.GrassoAimV3.enabled &&
                 playerInputActions.GrassoAimV3.Grasso.triggered) ||
                 (currentGrassoAim.Equals("Mouse") &&
                 Input.GetButtonDown("Grasso"))
            )
        {
            return true;
        }

        return false;
    }

    /*bool PressedButtonToShoot() {
        if ((currentGrassoAim.Equals("GrassoAimV1") && 
                Input.GetButtonUp("Right Trigger")) ||
                (currentGrassoAim.Equals("GrassoAimV2") &&
                Input.GetButtonUp("Right Trigger")) ||
                (currentGrassoAim.Equals("GrassoAimV3") &&
                Input.GetButtonDown("Grasso")) ||
                (currentGrassoAim.Equals("Mouse") &&
                Input.GetButtonDown("Grasso"))
           ) {
            return true;
           }
        
        return false;
    }*/

    bool PressedButtonToShoot()
    {
        if ((playerInputActions.GrassoAimV1.enabled &&
                playerInputActions.GrassoAimV1.Grasso.WasReleasedThisFrame()) ||
                (playerInputActions.GrassoAimV2.enabled &&
                playerInputActions.GrassoAimV2.Grasso.WasReleasedThisFrame()) ||
                (playerInputActions.GrassoAimV3.enabled &&
                playerInputActions.GrassoAimV3.Grasso.triggered) ||
                (currentGrassoAim.Equals("Mouse") &&
                Input.GetButtonDown("Grasso"))
           )
        {
            return true;
        }

        return false;
    }


    void SetJoystickInput(string xAxis, string yAxis) {
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
