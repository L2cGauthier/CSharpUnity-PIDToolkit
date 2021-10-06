[System.Serializable]
public class PIDControllerState
{
    public double timeSinceLastPIDUpdate = 0.0;
    public double realTimeOfLastPIDUpdate = 0.0;

    public double error;
    public double errorIntegral;
    public double errorDerivative;

    public double deltaMeasure = 0.0;

    public double previousMeasure = 0.0;

    public double pTerm;
    public double iTerm;
    public double dTerm;

    public double pidOutput;
}