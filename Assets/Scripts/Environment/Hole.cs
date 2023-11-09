using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour {
    [SerializeField] GameObject hidden;

    public void Plug() {
        IHideable hideable = hidden.GetComponent<IHideable>();
        hideable.Reveal();
        gameObject.SetActive(false);
    }
}
