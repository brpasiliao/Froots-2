using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Windows;

public enum Direction {
    up, right, down, left
}

public class SpringleafRotation : MonoBehaviour {
    [SerializeField] Springleaf springleaf;
    [SerializeField] GameObject acornPivot;
    [SerializeField] GameObject acornAnimation;
    [SerializeField] GameObject targetPivot;
    [SerializeField] GameObject target;
    [SerializeField] GameObject flower;
    [SerializeField] Direction direction = Direction.right;

    public PlayerInputActions playerInputActions;
    private float xInput;
    private float yInput;
    private bool onKeyboard;

    private void Awake()
    {
        playerInputActions = InputManager.inputActions;
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
        while (!UnityEngine.Input.GetButtonDown("Secondary") && !playerInputActions.Player.SecondaryAction.triggered) {
            RotateTarget();
            yield return null;
        }

        flower.SetActive(false);
        springleaf.UnstallStrawbert();
        // springleaf.animator.SetAnimatorInt("Direction", (int)direction);
    }

    public void RotateTarget() {
        Vector3 mousePos = UnityEngine.Input.mousePosition;
        Vector3 targetPos = targetPivot.transform.position;
        Vector3 targetScreenPos = Camera.main.WorldToScreenPoint(targetPos);
        mousePos.x = mousePos.x - targetScreenPos.x;
        mousePos.y = mousePos.y - targetScreenPos.y;
        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        Quaternion eulerAngle = Quaternion.Euler(new Vector3(0, 0, angle));        

        //Joystick Controls
        Vector2 inputVector = playerInputActions.Player.SecondaryMovement.ReadValue<Vector2>();

        float xInputRaw = inputVector.x; //Input.GetAxisRaw(xAxis);
        float yInputRaw = inputVector.y; //Input.GetAxisRaw(yAxis);



        if (xInputRaw != 0 || yInputRaw != 0)
        {
            xInput = xInputRaw;
            yInput = yInputRaw;
        }

        float joyStickAngle = Mathf.Atan2(yInput, xInput) * Mathf.Rad2Deg;
        Quaternion eulerJoyStickAngle = Quaternion.Euler(new Vector3(0, 0, joyStickAngle));

        if (!onKeyboard)
        {
            acornPivot.transform.rotation = eulerJoyStickAngle;
            targetPivot.transform.rotation = eulerJoyStickAngle;
        }
        else if(onKeyboard)
        {
            acornPivot.transform.rotation = eulerAngle;
            targetPivot.transform.rotation = eulerAngle;
        }
    }
    
    void ChangeDirection(Direction newDirection) {
        if (newDirection == Direction.up) {
            targetPivot.transform.eulerAngles = new Vector3(0, 0, 90);
        } else if (newDirection == Direction.right) {
            targetPivot.transform.eulerAngles = new Vector3(0, 0, 0);
        } else if (newDirection == Direction.down) {
            targetPivot.transform.eulerAngles = new Vector3(0, 0, -90);
        } else {
            targetPivot.transform.eulerAngles = new Vector3(0, 0, 180);
        }

        springleaf.animator.SetAnimatorInt("Direction", (int)newDirection);
    }

    void SetTargetDistance() {
        float distance = SpringleafSingleton.instance.targetDistance;
        acornPivot.transform.localScale = new Vector3(distance, 1, 1);
        acornAnimation.transform.localScale = new Vector3(1/distance, 1, 1);
        targetPivot.transform.localScale = new Vector3(distance, 1, 1);
        target.transform.localScale = new Vector3(1/distance, 1, 1);
    }

    public void SetTargetActive(bool setting) {
        targetPivot.SetActive(setting);
    }
}
