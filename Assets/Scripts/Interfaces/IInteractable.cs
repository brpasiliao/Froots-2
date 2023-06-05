using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable {
    SpriteRenderer sr { get; set; }
    void PerformInteraction();

}
