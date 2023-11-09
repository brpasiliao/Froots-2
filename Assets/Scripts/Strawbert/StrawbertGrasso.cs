using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrawbertGrasso : MonoBehaviour {
    [SerializeField] StrawbertBehavior strawbert;
    [SerializeField] GameObject target;
    [SerializeField] public float rotationSpeed;

    public bool canGrasso { get; set; } = true;

    string currentGrassoAim = "GrassoAimV1";
    public Vector2 input;

    void Awake() {
        input = new Vector2(0, 1f);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            currentGrassoAim = "GrassoAimV1";
            EventBroker.CallSendFeedback("Hold right trigger + left stick aim");
        } else if (Input.GetKeyDown(KeyCode.Alpha2)) {
            currentGrassoAim = "GrassoAimV2";
            EventBroker.CallSendFeedback("Hold right trigger + right stick aim");
        } else if (Input.GetKeyDown(KeyCode.Alpha3)) {
            currentGrassoAim = "GrassoAimV3";
            EventBroker.CallSendFeedback("Tap X + right stick aim");
        } else if (Input.GetKeyDown(KeyCode.Alpha4)) {
            currentGrassoAim = "Mouse";
            EventBroker.CallSendFeedback("Move mouse");
        }

        if (canGrasso && PressedButtonToAim()) {
            StartCoroutine("UseGrasso");
        }
    }

    IEnumerator UseGrasso() {
        SwingGrasso();
        
        yield return 0;
        while (!PressedButtonToShoot()) {
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
        strawbert.flower.SetObjectActive(true);
        strawbert.animator.SetAnimatorBool("Swinging", true);
    }

    private void AimGrassoJoystick() {
        if (currentGrassoAim.Equals("GrassoAimV1")) {
            SetJoystickInput("Horizontal", "Vertical");
        } else {
            SetJoystickInput("Right Horizontal", "Right Vertical");
        }

        float angle = Mathf.Atan2(input.y, input.x) * Mathf.Rad2Deg * -1;
        target.transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));
    }

    private void AimGrassoMouse() {
        Vector3 mousePos = Input.mousePosition;
        Vector3 targetPos = Camera.main.WorldToScreenPoint(target.transform.position);
        input = new Vector2(mousePos.x - targetPos.x, mousePos.y - targetPos.y);
        input.Normalize();
        float angle = Mathf.Atan2(input.y, input.x) * Mathf.Rad2Deg * -1;
        target.transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));
    }

    private void ShootGrasso() {
        strawbert.movement.canMove = false;
        strawbert.movement.StopMovement();
        strawbert.flower.StartShoot();
    }

    public void EndGrasso() {
        StopCoroutine("UseGrasso");
        target.SetActive(false);

        canGrasso = true;
        strawbert.movement.canMove = true;
        strawbert.animator.EndGrassoAnimation();
    }

    bool PressedButtonToAim() {
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
    }

    bool PressedButtonToShoot() {
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
    }

    void SetJoystickInput(string xAxis, string yAxis) {
        float xInputRaw = Input.GetAxisRaw(xAxis);
        float yInputRaw = Input.GetAxisRaw(yAxis);

        if (xInputRaw != 0 || yInputRaw != 0) {
            input = new Vector2(xInputRaw, yInputRaw);
        }
    }
}
