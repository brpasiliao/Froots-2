using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrawbertGrasso : MonoBehaviour {
    [SerializeField] StrawbertBehavior strawbert;
    [SerializeField] GameObject target;
    [SerializeField] GameObject flower;
    [SerializeField] public float rotationSpeed;

    public bool canGrasso { get; set; } = true;

    void Update() {
        if (canGrasso && Input.GetButtonDown("Fire1")) {
            StartCoroutine("UseGrasso");
        }
    }

    IEnumerator UseGrasso() {
        SwingGrasso();
        
        yield return 0;
        while (!Input.GetButtonDown("Fire1")) {
            AimGrasso();

            if (Input.GetButtonDown("Fire3")) {
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

    private void AimGrasso() {
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
}
