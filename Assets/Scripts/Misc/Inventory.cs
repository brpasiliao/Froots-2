using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Inventory {
    static List<Acorn> acorns = new List<Acorn>();
    public static int applewoods = 0;

    public static bool HasAcorns() {
        return acorns.Count > 0;
    }

    public static void AddAcorn(Acorn newAcorn) {
        acorns.Add(newAcorn);
    }

    public static Acorn TakeAcorn() {
        Acorn takenAcorn = acorns[0];
        acorns.RemoveAt(0);
        return takenAcorn;
    }

    public static string GetAcornCountString() {
        return acorns.Count.ToString();
    }

    public static string GetApplewoodCountString() {
        return applewoods.ToString();
    }
}
