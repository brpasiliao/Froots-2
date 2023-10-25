using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHideable {
    bool isRevealed { get; set; }

    void Reveal();
}
