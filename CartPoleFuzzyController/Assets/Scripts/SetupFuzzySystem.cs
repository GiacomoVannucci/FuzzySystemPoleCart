using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupFuzzySystem : MonoBehaviour {

    public float previousAngle;
    public float angleInput;
    public float angularVelocityInput;
    public float positionInput;
    public float forceOutput;

    public Rigidbody poleRigidBody;
    public Rigidbody cartRigidBody;

    public Transform poleTransform;

    //Angle fuzzy classes
    FuzzyClass nlA;
    FuzzyClass nmA;
    FuzzyClass nsA;
    FuzzyClass zeA;
    FuzzyClass psA;
    FuzzyClass pmA;
    FuzzyClass plA;

    //Angular velocity fuzzy classes
    FuzzyClass nlV;
    FuzzyClass nmV;
    FuzzyClass nsV;
    FuzzyClass zeV;
    FuzzyClass psV;
    FuzzyClass pmV;
    FuzzyClass plV;

    //position fuzzy classes
    FuzzyClass nlP;
    FuzzyClass nsP;
    FuzzyClass zeP;
    FuzzyClass psP;
    FuzzyClass plP;

    //Force fuzzy classes
    FuzzyClass nlF;
    FuzzyClass nmF;
    FuzzyClass nsF;
    FuzzyClass zeF;
    FuzzyClass psF;
    FuzzyClass pmF;
    FuzzyClass plF;

    //Fuzzy dominions
    FuzzyDominion angle;
    FuzzyDominion angularVelocity;
    FuzzyDominion force;
    FuzzyDominion position;

    //Rules
    List<FuzzyRule> rules = new List<FuzzyRule>();


    private void Start() { 

        //Fuzzy classes initialization
        nlA = new FuzzyClass("NL", -70f, -70f, -40f);
        nmA = new FuzzyClass("NL", -60f, -37.5f, -15f);
        nsA = new FuzzyClass("NL", -25f, -12.5f, 0f);
        zeA = new FuzzyClass("NL", -20f, 0f, 20f);
        psA = new FuzzyClass("NL", 0f, 12.5f, 25f);
        pmA = new FuzzyClass("NL", 15f, 37.5f, 60f);
        plA = new FuzzyClass("NL", 40f, 70f, 70f);

        nlV = new FuzzyClass("NL", -100f, -100f, -50f);
        nmV = new FuzzyClass("NL", -70f, -50f, -20f);
        nsV = new FuzzyClass("NL", -40f, -20f, 0f);
        zeV = new FuzzyClass("NL", -20f, 0f, 20f);
        psV = new FuzzyClass("NL", 0f, 20f, 40f);
        pmV = new FuzzyClass("NL", 20f, 50f, 70f);
        plV = new FuzzyClass("NL", 50f, 100f, 100f);

        nlP = new FuzzyClass("pos", -2.4f, -2.4f, -1f);
        nsP = new FuzzyClass("pos", -0.7f, -0.5f, -0.3f);
        zeP = new FuzzyClass("pos", -0.3f, 0f, -0.3f);
        psP = new FuzzyClass("pos", 0.3f, 0.5f, 0.7f);
        plP = new FuzzyClass("pos", 1f, 2.4f, 2.4f);

        nlF = new FuzzyClass("NL", -25, -25f, -20f);
        nmF = new FuzzyClass("NL", -25f, -20f, -12f);
        nsF = new FuzzyClass("NL", -20f, -10f, 0f);
        zeF = new FuzzyClass("NL", -4f, 0f, 4f);
        psF = new FuzzyClass("NL", 0f, 10f, 20f);
        pmF = new FuzzyClass("NL", 12f, 20f, 25f);
        plF = new FuzzyClass("NL", 20f, 25f, 25f);

        List<FuzzyClass> list;

        list = new List<FuzzyClass>() { nlA, nmA, nsA, zeA, psA, pmA, plA };
        angle = new FuzzyDominion(list);

        list = new List<FuzzyClass>() { nlV, nmV, nsV, zeV, psV, pmV, plV };
        angularVelocity = new FuzzyDominion(list);

        list = new List<FuzzyClass>() { nlF, nmF, nsF, zeF, psF, pmF, plF };
        force = new FuzzyDominion(list);

        list = new List<FuzzyClass>() { nlP, nsP, zeP, psP, plP };
        position = new FuzzyDominion(list);

        rules.Add(new FuzzyRule(new List<FuzzyClass>() { nlP }, nsF));
        rules.Add(new FuzzyRule(new List<FuzzyClass>() { plP }, psF));
        rules.Add(new FuzzyRule(new List<FuzzyClass>() { zeA, nlV }, plF));
        rules.Add(new FuzzyRule(new List<FuzzyClass>() { zeA, nmV }, pmF));
        rules.Add(new FuzzyRule(new List<FuzzyClass>() { psA, nsV }, nsF));
        rules.Add(new FuzzyRule(new List<FuzzyClass>() { zeA, nsV }, psF));
        rules.Add(new FuzzyRule(new List<FuzzyClass>() { plA, zeV }, nlF));
        rules.Add(new FuzzyRule(new List<FuzzyClass>() { pmA, zeV }, nmF));
        rules.Add(new FuzzyRule(new List<FuzzyClass>() { psA, zeV }, nsF));
        rules.Add(new FuzzyRule(new List<FuzzyClass>() { zeA, zeV }, zeF));
        rules.Add(new FuzzyRule(new List<FuzzyClass>() { nsA, zeV }, psF));
        rules.Add(new FuzzyRule(new List<FuzzyClass>() { nmA, zeV }, pmF));
        rules.Add(new FuzzyRule(new List<FuzzyClass>() { nlA, zeV }, plF));
        rules.Add(new FuzzyRule(new List<FuzzyClass>() { zeA, psV }, nsF));
        rules.Add(new FuzzyRule(new List<FuzzyClass>() { nsA, psV }, psF));
        rules.Add(new FuzzyRule(new List<FuzzyClass>() { zeA, pmV }, nmF));
        rules.Add(new FuzzyRule(new List<FuzzyClass>() { zeA, nlV }, nlF));  

        if (poleTransform.eulerAngles.z > 90) {
            previousAngle = poleTransform.eulerAngles.z - 360;
        } else {
            previousAngle = poleTransform.eulerAngles.z;
        }

        StartCoroutine(UpdateSystem());
    }

    IEnumerator UpdateSystem() {

       while (true) {

            yield return new WaitForSeconds(0.02f);

            if(poleTransform.eulerAngles.z > 180) {
                angleInput = poleTransform.eulerAngles.z - 360;
            } else {
                angleInput = poleTransform.eulerAngles.z;
            }
            angularVelocityInput = poleRigidBody.angularVelocity.z; 
            previousAngle = angleInput;
            positionInput = cartRigidBody.transform.position.x;

            //Numerator and denominator of the weighted average
            float num = 0;
            float den = 0;

            //Fuzzyfication of the inputs
            angle.CheckActivations(angleInput);
            angularVelocity.CheckActivations(angularVelocityInput);
            position.CheckActivations(positionInput);

            //Compute the fits of each rule
            for (int i = 0; i < rules.Count; i++) {
                rules[i].ComputeRule();
                num += rules[i].Fit * rules[i].Action;
                den += rules[i].Fit;
            }

            //Finally applying the force to the system
            forceOutput = num / den;

            cartRigidBody.AddForce(new Vector3(forceOutput, 0, 0), ForceMode.Impulse);          
        }
    }
}
