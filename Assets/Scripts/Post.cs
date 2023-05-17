using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Post : MonoBehaviour, IGrabbable {
    // [SerializeField] GameObject flower;
    public static StrawbertBehavior strawbert;

    void Awake() {
        strawbert = GameObject.FindWithTag("Player").GetComponent<StrawbertBehavior>();
    } 
    public void Grab() {
        // flower.SetActive(true);
        strawbert.transform.position = transform.position;
        strawbert.EndGrasso();
    }
}
