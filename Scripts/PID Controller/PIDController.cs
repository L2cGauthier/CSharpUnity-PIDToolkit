using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class PIDController
{
    public PIDControllerState state;
    public PIDControllerParameters parameters;
    private PIDGraphHolder pidGraphHolder = null;

    private PIDLogOptions logOptions;
    private PIDOptions pidOptions;
    private PIDTimeOptions timeOptions;

    private const double SMALLEST_REALTIME_DELTATIME = 0.001;

    public PIDController(PIDControllerParameters parameters)
    {
        this.parameters = parameters;
        this.state = new PIDControllerState();

        this.logOptions = parameters.logOptions;
        this.pidOptions = parameters.pidOptions;
        this.timeOptions = parameters.timeOptions;

        this.state.realTimeOfLastPIDUpdate = Time.time;

#if UNITY_EDITOR
        if (logOptions != PIDLogOptions.NONE)
        {
            GameObject graphHolder = new GameObject("PID Graph holder");
            this.pidGraphHolder = graphHolder.AddComponent<PIDGraphHolder>();
            this.pidGraphHolder.LogOptions = logOptions;
        }
#endif
    }

    public PIDControllerState UpdateState(double measure, double target)
    {
        // Update time tracking variables
        switch (timeOptions)
        {
            case PIDTimeOptions.UPDATE:
                if (Time.deltaTime <= float.Epsilon) { return state; } // FIXME:
                state.timeSinceLastPIDUpdate = Time.deltaTime;
                state.realTimeOfLastPIDUpdate = Time.time;
                break;

            case PIDTimeOptions.FIXED_UPDATE:
                if (Time.fixedDeltaTime <= float.Epsilon) { return state; } // FIXME:
                state.timeSinceLastPIDUpdate = Time.fixedDeltaTime;
                state.realTimeOfLastPIDUpdate = Time.time;
                break;

            case PIDTimeOptions.REAL_TIME:
                double time = Time.time;
                state.timeSinceLastPIDUpdate = Math.Max(time - state.realTimeOfLastPIDUpdate, SMALLEST_REALTIME_DELTATIME);
                state.realTimeOfLastPIDUpdate = time;
                break;
                
            default:
                throw new System.Exception("[PIDController] Unhandled PIDTimeOptions passed");
        }
        
        // Update measure-related variables
        state.deltaMeasure = (measure - state.previousMeasure);
        state.previousMeasure = measure;

        // Update error-related variables
        state.errorDerivative = ((target - measure) - state.error) / state.timeSinceLastPIDUpdate;
        if (parameters.pidOptions.HasFlag(PIDOptions.USE_TRAPEZOIDAL_INTEGRATION))
        {
            state.errorIntegral += (state.error + (target - measure)) * 0.5 * state.timeSinceLastPIDUpdate;
        }
        else
        {
            state.errorIntegral += (target - measure) * state.timeSinceLastPIDUpdate;
        }

        state.error = target - measure;
        
        // P-Term
        if (parameters.pidOptions.HasFlag(PIDOptions.USE_PROPORTIONAL_ON_MEASUREMENT))
        {
            state.pTerm = -(parameters.pGain * state.deltaMeasure);
        }
        else
        {
            state.pTerm = parameters.pGain * state.error;
        }

        // I-Term
        state.iTerm = parameters.iGain * state.errorIntegral;

        // D-Term
        if (parameters.pidOptions.HasFlag(PIDOptions.USE_DERIVATIVE_ON_MEASUREMENT))
        {
            state.dTerm = -(parameters.dGain * state.deltaMeasure / state.timeSinceLastPIDUpdate);
        } 
        else
        {
            state.dTerm = parameters.dGain * state.errorDerivative;
        }

        // Output
        state.pidOutput = state.pTerm + state.iTerm + state.dTerm;

        // Update Debug visualization
#if UNITY_EDITOR
        if (this.parameters.logOptions != PIDLogOptions.NONE)
        {
            this.pidGraphHolder.UpdateCurves(state);
        }
#endif

        return state;
    }
}
