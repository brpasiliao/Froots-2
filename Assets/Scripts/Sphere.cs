using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere : MonoBehaviour {
    void Start() {
        Vector3 force = new Vector3(2, 0, 0);
        GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
    }
}
