using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrawbertGrasso : MonoBehaviour {
    [SerializeField] StrawbertBehavior strawbert;
    [SerializeField] GameObject target;
    [SerializeField] GameObject flower;
    [SerializeField] public float rotationSpeed;

    public bool canGrasso { get; set; } = true;

    string currentGrassoAim = "UseGrassoV1";
    float xInput = 0;
    float yInput = 1f;

    void Update() {
        // if (canGrasso && Input.GetButtonDown("Grasso")) {
        //     StartCoroutine("UseGrasso");
        // }

        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            currentGrassoAim = "UseGrassoV1";
            EventBroker.CallSendFeedback("Hold right trigger + left stick aim");
        } else if (Input.GetKeyDown(KeyCode.Alpha2)) {
            currentGrassoAim = "UseGrassoV2";
            EventBroker.CallSendFeedback("Hold right trigger + right stick aim");
        } else if (Input.GetKeyDown(KeyCode.Alpha3)) {
            currentGrassoAim = "UseGrassoV3";
            EventBroker.CallSendFeedback("Tap X + right stick aim");
        }

        if (canGrasso && PressedButtonToAim()) {
            StartCoroutine("UseGrasso");
        }
    }

    IEnumerator UseGrasso() {
        SwingGrasso();
        
        yield return 0;
        while (!PressedButtonToShoot()) {
            AimGrassoJoystick();

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
        if (currentGrassoAim.Equals("UseGrassoV1")) {
            SetJoystickInput("Horizontal", "Vertical");
        } else {
            SetJoystickInput("Right Horizontal", "Right Vertical");
        }

        float angle = Mathf.Atan2(yInput, xInput) * Mathf.Rad2Deg;
        target.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle-90));
    }

    private void AimGrassoMouse() {
        Vector3 mousePos = Input.mousePosition;
        Vector3 targetPos = Camera.main.WorldToScreenPoint(target.transform.position);
        mousePos.x = mousePos.x - targetPos.x;
        mousePos.y = mousePos.y - targetPos.y;
        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        target.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle-90));
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

    bool PressedButtonToAim() {
        if ((currentGrassoAim.Equals("UseGrassoV1") && 
                Input.GetButtonDown("Right Trigger")) ||
                (currentGrassoAim.Equals("UseGrassoV2") &&
                Input.GetButtonDown("Right Trigger")) ||
                (currentGrassoAim.Equals("UseGrassoV3") &&
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
            xInput = xInputRaw;
            yInput = yInputRaw;
        }
    }

    bool PressedButtonToShoot() {
        if ((currentGrassoAim.Equals("UseGrassoV1") && 
                Input.GetButtonUp("Right Trigger")) ||
                (currentGrassoAim.Equals("UseGrassoV2") &&
                Input.GetButtonUp("Right Trigger")) ||
                (currentGrassoAim.Equals("UseGrassoV3") &&
                Input.GetButtonDown("Grasso"))
           ) {
            return true;
           }
        
        return false;
    }
}
