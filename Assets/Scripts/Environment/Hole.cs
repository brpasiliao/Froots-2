using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour {
    [SerializeField] GameObject hidden;

    public void Plug() {
        gameObject.SetActive(false);
        hidden.SetActive(true);
    }
}
