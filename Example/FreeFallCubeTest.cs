using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FreeFallCubeTest : MonoBehaviour
{
    /*
    How to use: put a cube in a new scene, add this script to the cube, press play.

    In this test, we use a PID controller to slow the fall of free falling cube:
        - The process variable is the cube's vertical velocity
        - The target is 0, we are trying to stop the cube fall
    
    We could use Newton's second law of motion to determine exactly the amount of force to compensate the action of gravity.

    There are 2 main cases where you might consider a PID controller instead:
        - If you cannot model your process, or do not have perfect knowledge of the simulated process
        - If your system might be subjected to external / impredictable forces (wind, collisions...)

    The downside is that tuning the P, I and D gains might require some fiddling.
    */


    private PIDController gravityCompensator;
    private Rigidbody cubeRigidbody;

    [SerializeField] private double pGain = 30.0;
    [SerializeField] private double iGain = 20.0;
    [SerializeField] private double dGain = 0.1;

    private void Start()
    {
        cubeRigidbody = this.GetComponent<Rigidbody>();
        gravityCompensator = new PIDController(new PIDControllerParameters(pGain, iGain, dGain, PIDTimeOptions.FIXED_UPDATE, PIDOptions.USE_DERIVATIVE_ON_MEASUREMENT | PIDOptions.USE_TRAPEZOIDAL_INTEGRATION, PIDLogOptions.ALL));
    }

    private void Update()
    {
        gravityCompensator.parameters.pGain = pGain;
        gravityCompensator.parameters.iGain = iGain;
        gravityCompensator.parameters.dGain = dGain;

        cubeRigidbody.WakeUp();
    }

    private void FixedUpdate() 
    {
        double targetVerticalSpeed = 0.0;
        double currentVerticalVelocity = cubeRigidbody.velocity.y;
        double pidOutput = gravityCompensator.UpdateState(currentVerticalVelocity, targetVerticalSpeed).pidOutput;

        cubeRigidbody.AddForce((float) pidOutput * Vector3.up, ForceMode.Force);
    }
}