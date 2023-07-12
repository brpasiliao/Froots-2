using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Inventory {
    static List<Acorn> acorns = new List<Acorn>();
    public static int applewoods = 0;

    public static bool HasAcorns() {
        return acorns.Count > 0;
    }

    public static Acorn TakeAcorn() {
        takenAcorn = acorns[0];
        acorns.RemoveAt(0);
        return takenAcorn;
    }
}
