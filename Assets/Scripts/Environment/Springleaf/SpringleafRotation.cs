using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public enum Direction {
    up, right, down, left
}

public class SpringleafRotation : MonoBehaviour {
    [SerializeField] Springleaf springleaf;
    [SerializeField] GameObject targetPivot;
    [SerializeField] GameObject target;
    [SerializeField] GameObject flower;
    [SerializeField] Direction direction = Direction.right;

    public PlayerInputActions playerInputActions;
    public Vector2 input;
    private bool onKeyboard;

    private void Awake() {
        playerInputActions = InputManager.inputActions;
        input = new Vector2(0, 1f);
    }

    void Start() {
        SetTargetDistance();
        ChangeDirection(direction);
    }

    public void RotateSpringleaf(bool onKeyboardcheck) {
        if (onKeyboardcheck)
            onKeyboard = true;
        else if(!onKeyboardcheck)
            onKeyboard = false;
        
        StartCoroutine("RotatingAction");
    }

    IEnumerator RotatingAction() {
        springleaf.StallStrawbert(null);
        flower.SetActive(true);

        yield return 0;
        while (!playerInputActions.Player.SecondaryAction.triggered) {
            RotateTarget();
            yield return null;
        }

        flower.SetActive(false);
        springleaf.UnstallStrawbert();
        // springleaf.animator.SetAnimatorInt("Direction", (int)direction);
    }

    public void RotateTarget() {
        if (onKeyboard) {
            Vector3 mousePos = playerInputActions.Player.SecondaryMovement.ReadValue<Vector2>();
            Vector3 targetPos = targetPivot.transform.position;
            Vector3 targetScreenPos = Camera.main.WorldToScreenPoint(targetPos);
            input = new Vector2(mousePos.x - targetScreenPos.x, mousePos.y - targetScreenPos.y);
            input.Normalize();

        } else {    //Joystick Controls
            Vector2 inputVector = playerInputActions.Player.Movement.ReadValue<Vector2>();
            
            if (inputVector != Vector2.zero) {
                input = inputVector;
            }
        }
        
        float angle = Mathf.Atan2(input.y, input.x) * Mathf.Rad2Deg * -1;
        Quaternion eulerAngle = Quaternion.Euler(new Vector3(0, angle, 0));
        targetPivot.transform.rotation = eulerAngle;
    }
    
    void ChangeDirection(Direction newDirection) {
        if (newDirection == Direction.up) {
            targetPivot.transform.eulerAngles = new Vector3(0, 90, 0);
        } else if (newDirection == Direction.right) {
            targetPivot.transform.eulerAngles = new Vector3(0, 0, 0);
        } else if (newDirection == Direction.down) {
            targetPivot.transform.eulerAngles = new Vector3(0, -90, 0);
        } else {
            targetPivot.transform.eulerAngles = new Vector3(0, 180, 0);
        }

        springleaf.animator.SetAnimatorInt("Direction", (int)newDirection);
    }

    void SetTargetDistance() {
        float distance = SpringleafSingleton.instance.targetDistance;
        targetPivot.transform.localScale = new Vector3(distance, 1, 1);
        target.transform.localScale = new Vector3(0.05f, 0.15f, 0.15f);
    }

    public void SetTargetActive(bool setting) {
        targetPivot.SetActive(setting);
    }
}
