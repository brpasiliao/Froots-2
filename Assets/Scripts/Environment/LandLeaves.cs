using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandLeaves : MonoBehaviour {
    [SerializeField] GameObject hidden;

    public void Break() {
        hidden.SetActive(true);
        gameObject.SetActive(false);
    }
}
