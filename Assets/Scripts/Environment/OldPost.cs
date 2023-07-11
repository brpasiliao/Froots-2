using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldPost : MonoBehaviour, IGrabbable {
    public static StrawbertBehavior strawbert;

    void Awake() {
        strawbert = GameObject.FindWithTag("Player").GetComponent<StrawbertBehavior>();
    } 

    public void Grab() {
        gameObject.SetActive(false);
        strawbert.grasso.EndGrasso();
    }
}
