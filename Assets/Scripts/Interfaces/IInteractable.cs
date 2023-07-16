using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable {
    void GetApproached();
    void GetDeparted();

    void DoPrimary();
    void DoSecondary();
}
