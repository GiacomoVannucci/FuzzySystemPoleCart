using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuzzyClass : MonoBehaviour {

    public string className;
    MembershipFunction membershipFunction;
    private float fit;
    private float action;

    public float Fit { get { return fit; } }
    public float Action { get { return action; } }

    public FuzzyClass(string name, float l, float c, float r) {

        className = name;
        membershipFunction = new MembershipFunction(l, c, r);
        action = c;
    }

    public void ComputeFit(float input) {

        fit = membershipFunction.Triangular(input);
    }
}
