using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrawbertBehavior : MonoBehaviour {
    public float speed;
    Vector3 destination;

    void Update() {
        destination = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);
        destination = destination.normalized*speed;
        transform.Translate(destination);
    }
}
