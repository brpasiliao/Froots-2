using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrawbertDialogue : MonoBehaviour {
    List<string> acornDialogue = new List<string> {
        "I saw some springleaves around here... ",
        "Maybe I can launch these acorns with them.",
        "Press right click to tag an acorn to use for later",
    };
    List<string> springleafDialogue = new List<string> {
        "There's a springleaf!",
        "Press right click to load an acorn, and again to launch it.",
        "To change its direction, grab it with the grasso.",
        "Then, press QE to rotate the springleaf, and left click to finish.",
    };
    List<string> applewoodDialogue = new List<string> {
        "This is great wood for building houses!",
        "I knew helping the Apples and Pears was a good idea!",
    };

    bool firstAcorn = true;
    bool firstSpringleaf = true;
    bool firstApplewood = true;

    void OnTriggerEnter2D(Collider2D other) {
        if (firstAcorn && other.GetComponent<Acorn>() != null) {
            EventBroker.CallPlayDialogue(acornDialogue);
            firstAcorn = false;
        }
        if (firstSpringleaf && other.GetComponent<Springleaf>() != null) {
            EventBroker.CallPlayDialogue(springleafDialogue);
            firstSpringleaf = false;
        }
        if (firstApplewood && other.GetComponent<Applewood>() != null) {
            EventBroker.CallPlayDialogue(applewoodDialogue);
            firstApplewood = false;
        } 
    }
}
