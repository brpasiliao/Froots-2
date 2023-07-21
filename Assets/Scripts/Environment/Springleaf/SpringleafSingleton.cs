using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringleafSingleton : MonoBehaviour {
    public static SpringleafSingleton instance { get; private set; }

    public float targetDistance;
    public float animationSpeed;

    private void Awake() { 
        if (instance != null && instance != this) { 
            Destroy(this); 
        } else { 
            instance = this; 
        }
    }
}
