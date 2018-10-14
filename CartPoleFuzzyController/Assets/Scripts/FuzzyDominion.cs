using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuzzyDominion : MonoBehaviour {

    public List<FuzzyClass> classes;

    public List<FuzzyClass> activatedClasses;

    public FuzzyDominion(List<FuzzyClass> c) {

        classes = c;
    }

    public void CheckActivations(float input) {

        for (int i = 0; i < classes.Count; i++) {
            classes[i].ComputeFit(input);
        }
    }
}
