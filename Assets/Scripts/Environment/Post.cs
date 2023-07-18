using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Post : MonoBehaviour, IGrabbable {
    public static StrawbertBehavior strawbert;

    void Awake() {
        strawbert = GameObject.FindWithTag("Player").GetComponent<StrawbertBehavior>();
    } 

    public void GetGrabbed() {
        strawbert.transform.position = transform.position;
        strawbert.grasso.EndGrasso();
    }
}
