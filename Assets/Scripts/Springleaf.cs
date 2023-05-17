using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Springleaf : MonoBehaviour, IInteractable, ITaggable, IGrabbable {
    [SerializeField] Animator animator;
    [SerializeField] GameObject target;
    [SerializeField] GameObject flower;
    [SerializeField] int direction;     // 1=up, 2=right, 3=down, 4=left
    public static StrawbertBehavior strawbert;
    public Pit pit;

    public bool IsTagged { get; set; }

    public static event Action onSpringleafTag;

    void Awake() {
        strawbert = GameObject.FindWithTag("Player").GetComponent<StrawbertBehavior>();
        pit = transform.parent.GetComponent<Pit>();
        IsTagged = false;
    } 

    void Start() {
        animator.SetInteger("Direction", direction);

        if (direction == 1) 
            target.transform.eulerAngles = new Vector3(0, 0, 180);
        else if (direction == 2)
            target.transform.eulerAngles = new Vector3(0, 0, 90);
        else if (direction == 3)
            target.transform.eulerAngles = new Vector3(0, 0, 0);
        else
            target.transform.eulerAngles = new Vector3(0, 0, -90);
    }

    public void PerformInteraction() {
        if (!IsTagged) GetTagged();
        else {
            if (Inventory.acorns.Count > 0) Launch();
            else Debug.Log("not enough acorns!");
        }
    }

    void Launch() {
        if (!animator.GetBool("Launching"))
            animator.SetBool("Launching", true);
    }

    void EndLaunch() {
        animator.SetBool("Launching", false);
    }

    public void GetTagged() {
        Debug.Log("tagged!");
        IsTagged = true;
        Inventory.springleaves.Add(this);
        onSpringleafTag?.Invoke();
    }

    public void Grab() {
        StartCoroutine("ChangeDirection");
    }

    IEnumerator ChangeDirection() {
        flower.SetActive(true);
        target.SetActive(true);

        while (!Input.GetButtonDown("Fire1")) {
            if (Input.GetKeyDown(KeyCode.O)) {
                target.transform.Rotate(new Vector3(0, 0, 90));
                direction--;
                if (direction < 1) direction = 4;
            } else if (Input.GetKeyDown(KeyCode.P)) {
                target.transform.Rotate(new Vector3(0, 0, -90));
                direction++;
                if (direction > 4) direction = 1;
            }

            if (Input.GetButtonDown("Fire3")) {
                target.SetActive(false);
                StopCoroutine("ChangeDirection");
            }

            yield return null;
        }

        target.SetActive(false);
        flower.SetActive(false);
        animator.SetInteger("Direction", direction);
        strawbert.EndGrasso();
    }
}
