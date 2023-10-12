using System.Collections;
using System.Collections.Generic;
// using System.Numerics;
using UnityEngine;
// using Vector3 = UnityEngine.Vector3;

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

    void Start() {
        SetTargetDistance();
        ChangeDirection(direction);
    }

    public void RotateSpringleaf() {
        StartCoroutine("RotatingAction");
    }

    IEnumerator RotatingAction() {
        springleaf.StallStrawbert(null);
        flower.SetActive(true);

        yield return 0;
        while (!Input.GetButtonDown("Secondary")) {
            RotateTarget();
            yield return null;
        }

        flower.SetActive(false);
        springleaf.UnstallStrawbert();
        // springleaf.animator.SetAnimatorInt("Direction", (int)direction);
    }

    public void RotateTarget() {
        Vector3 mousePos = Input.mousePosition;
        Vector3 targetPos = targetPivot.transform.position;
        Vector3 targetScreenPos = Camera.main.WorldToScreenPoint(targetPos);
        mousePos.x = mousePos.x - targetScreenPos.x;
        mousePos.y = mousePos.y - targetScreenPos.y;
        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        Quaternion eulerAngle = Quaternion.Euler(new Vector3(0, angle, 0));

        acornPivot.transform.rotation = eulerAngle;
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
        acornPivot.transform.localScale = new Vector3(distance, 1, 1);
        acornAnimation.transform.localScale = new Vector3(1/distance, 1, 1);
        targetPivot.transform.localScale = new Vector3(distance, 1, 1);
        // target.transform.localScale = new Vector3(1/distance, 1, 1);
        target.transform.localScale = new Vector3(0.05f, 0.15f, 0.15f);
    }

    public void SetTargetActive(bool setting) {
        targetPivot.SetActive(setting);
    }
}
