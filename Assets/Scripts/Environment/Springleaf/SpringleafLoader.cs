using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringleafLoader : MonoBehaviour {
    [SerializeField] Springleaf springleaf;

    bool acornAssigned = false;
    public bool acornLoaded { get; set; } = false;

    string assignedAcorn = "Assigned acorn to springleaf!";
    string reloadedAcorn = "Reloaded on springleaf!";
    string noAcorns = "Not enough acorns!";

    public void TryLoadAcorn() {
        if (!acornAssigned) {
            TryAssignAcorn();
        } else {
            ReloadAcorn();
        }
    }

    void TryAssignAcorn() {
        if (Inventory.HasAcorns()) {
            AssignAcorn();
            ReloadAcorn();
        } else {
            EventBroker.CallSendFeedback(noAcorns);
        }
    }

    void AssignAcorn() {
        acornAssigned = true;

        springleaf.acorn = Inventory.TakeAcorn();
        springleaf.acorn.AssignToSpringleaf(springleaf);

        EventBroker.CallSendFeedback(assignedAcorn);
        EventBroker.CallAcornCount();
    }

    public void ReloadAcorn() {
        acornLoaded = true;
        springleaf.acorn.Reload();

        EventBroker.CallSendFeedback(reloadedAcorn);
    }
}
