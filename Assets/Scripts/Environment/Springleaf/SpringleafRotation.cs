using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction {
    up, right, down, left
}

public class SpringleafRotation : MonoBehaviour {
    [SerializeField] Springleaf springleaf;
    [SerializeField] GameObject target;
    [SerializeField] GameObject flower;
    [SerializeField] Direction direction = Direction.right;

    void Start() {
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
        Vector3 targetPos = Camera.main.WorldToScreenPoint(target.transform.position);
        mousePos.x = mousePos.x - targetPos.x;
        mousePos.y = mousePos.y - targetPos.y;
        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        target.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle-90));
    }
    
    void ChangeDirection(Direction newDirection) {
        if (newDirection == Direction.up) {
            target.transform.eulerAngles = new Vector3(0, 0, 90);
        } else if (newDirection == Direction.right) {
            target.transform.eulerAngles = new Vector3(0, 0, 0);
        } else if (newDirection == Direction.down) {
            target.transform.eulerAngles = new Vector3(0, 0, -90);
        } else {
            target.transform.eulerAngles = new Vector3(0, 0, 180);
        }

        springleaf.animator.SetAnimatorInt("Direction", (int)newDirection);
    }

    public void SetTargetActive(bool setting) {
        target.SetActive(setting);
    }
}
