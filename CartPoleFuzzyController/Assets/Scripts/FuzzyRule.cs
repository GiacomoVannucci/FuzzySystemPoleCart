using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuzzyRule : MonoBehaviour {

    List<FuzzyClass> inputClasses;
    FuzzyClass outputClass;

    private float fit;
    private float action;
    public float Fit { get { return fit; } }
    public float Action { get{ return action; } }


    public FuzzyRule(List<FuzzyClass> inputClasses, FuzzyClass outputClass) {

        this.inputClasses = inputClasses;
        this.outputClass = outputClass;
        action = outputClass.Action;
    }

    public void ComputeRule() {

        float min = 1;

        for(int i = 0; i < inputClasses.Count; i++) {
            if(inputClasses[i].Fit < min) {
                min = inputClasses[i].Fit;
            }
        }

        fit = min;


    }
}
