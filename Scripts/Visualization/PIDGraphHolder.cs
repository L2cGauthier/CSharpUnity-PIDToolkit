namespace PIDToolkit
{
    using UnityEngine;

    public class PIDGraphHolder : MonoBehaviour
    {
        private PIDLogOptions logOptions = PIDLogOptions.NONE;
        public PIDLogOptions LogOptions { get { return logOptions; } set { logOptions = value; } }

        private EditorGraph pTermCurve = null;
        private EditorGraph iTermCurve = null;
        private EditorGraph dTermCurve = null;
        private EditorGraph pidOutputTCurve = null;

        private bool isInitialized = false;

        private void Initialize()
        {
            if (LogOptions.HasFlag(PIDLogOptions.P_TERM))
            {
                pTermCurve = gameObject.AddComponent<EditorGraph>();
                pTermCurve.Initialize("P-term");
            }
            if (LogOptions.HasFlag(PIDLogOptions.I_TERM))
            {
                iTermCurve = gameObject.AddComponent<EditorGraph>();
                iTermCurve.Initialize("I-term");
            }
            if (LogOptions.HasFlag(PIDLogOptions.D_TERM))
            {
                dTermCurve = gameObject.AddComponent<EditorGraph>();
                dTermCurve.Initialize("D-term");
            }
            if (LogOptions.HasFlag(PIDLogOptions.PID_OUTPUT))
            {
                pidOutputTCurve = gameObject.AddComponent<EditorGraph>();
                pidOutputTCurve.Initialize("PID Output");
            }

            isInitialized = true;
        }

        private void Start()
        {
            if (!isInitialized) { Initialize(); }
        }

        public void UpdateCurves(PIDControllerState state)
        {
            if (!isInitialized) { Initialize(); }
            
            double sampleTime = state.timeSinceLastPIDUpdate;

            if (LogOptions.HasFlag(PIDLogOptions.P_TERM))
            {
                pTermCurve.AddDataPoint(sampleTime, state.pTerm);
            }
            if (LogOptions.HasFlag(PIDLogOptions.I_TERM))
            {
                iTermCurve.AddDataPoint(sampleTime, state.iTerm);
            }
            if (LogOptions.HasFlag(PIDLogOptions.D_TERM))
            {
                dTermCurve.AddDataPoint(sampleTime, state.dTerm);
            }
            if (LogOptions.HasFlag(PIDLogOptions.PID_OUTPUT))
            {
                pidOutputTCurve.AddDataPoint(sampleTime, state.pidOutput);
            }
        }

        private void OnDestroy()
        {
            pTermCurve = null;
            iTermCurve = null;
            dTermCurve = null;
            pidOutputTCurve = null;
        }
    }
}