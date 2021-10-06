using UnityEngine;

[System.Serializable]
public class PIDControllerParameters
{
    // Gains
    public double pGain;
    public double iGain;
    public double dGain;

    public PIDOptions pidOptions;
    public PIDLogOptions logOptions;
    public PIDTimeOptions timeOptions;

    public PIDControllerParameters(double pGain, double iGain, double dGain, 
                                   PIDTimeOptions timeOptions = PIDTimeOptions.UPDATE, 
                                   PIDOptions options = PIDOptions.NONE, 
                                   PIDLogOptions logOptions = PIDLogOptions.NONE)
    {
        this.pGain = pGain;
        this.iGain = iGain;
        this.dGain = dGain;
        
        this.timeOptions = timeOptions;
        this.pidOptions = options;
        this.logOptions = logOptions;
    }
}