using System;
using System.Linq;
using UnityEngine;

[System.Flags]
public enum PIDOptions
{
    NONE = (1 << 0),
    USE_DERIVATIVE_ON_MEASUREMENT = (1 << 1),
    USE_PROPORTIONAL_ON_MEASUREMENT = (1 << 2),
    USE_TRAPEZOIDAL_INTEGRATION = (1 << 3),

    ALL = USE_DERIVATIVE_ON_MEASUREMENT | USE_PROPORTIONAL_ON_MEASUREMENT | USE_TRAPEZOIDAL_INTEGRATION,
}

public static class PIDOptionsExtensions
{
    public static PIDOptions AddFlags(this PIDOptions currentMask, PIDOptions flagsToAdd)
    {
        return currentMask | flagsToAdd;
    }

    public static PIDOptions RemoveFlags(this PIDOptions currentMask, PIDOptions flagsToRemove)
    {
        return currentMask & ~flagsToRemove;
    }

    public static bool HasAllFlags(this PIDOptions currentMask, PIDOptions flagsToCheck)
    {
        return ((currentMask & flagsToCheck) == flagsToCheck);
    }

    public static bool HasAnyFlags(this PIDOptions currentMask, PIDOptions flagsToCheck)
    {
        return ((currentMask & flagsToCheck) != 0);
    }

    public static PIDOptions ToggleFlags(this PIDOptions currentMask, PIDOptions FlagsToToggle)
    {
        return (currentMask & ~(currentMask & FlagsToToggle)) | ((currentMask | FlagsToToggle) & ~currentMask);
    }
}