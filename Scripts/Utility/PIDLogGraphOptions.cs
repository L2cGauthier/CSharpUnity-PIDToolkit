namespace PIDToolkit
{
    using System;

    [Flags]
    public enum PIDLogOptions
    {
        NONE = 0,
        P_TERM = (1 << 0),
        I_TERM = (1 << 1),
        D_TERM = (1 << 2),
        PID_OUTPUT = (1 << 3),

        ALL = P_TERM | I_TERM | D_TERM | PID_OUTPUT,
    }

    public static class PIDLogGraphOptionsExtensions
    {
        public static PIDLogOptions AddFlags(this PIDLogOptions currentMask, PIDLogOptions flagsToAdd)
        {
            return currentMask | flagsToAdd;
        }

        public static PIDLogOptions RemoveFlags(this PIDLogOptions currentMask, PIDLogOptions flagsToRemove)
        {
            return currentMask & ~flagsToRemove;
        }

        public static bool HasAllFlags(this PIDLogOptions currentMask, PIDLogOptions flagsToCheck)
        {
            return ((currentMask & flagsToCheck) == flagsToCheck);
        }

        public static bool HasAnyFlags(this PIDLogOptions currentMask, PIDLogOptions flagsToCheck)
        {
            return ((currentMask & flagsToCheck) != 0);
        }

        public static PIDLogOptions RestrictToAuthorisedFlags(this PIDLogOptions currentMask, PIDLogOptions authorisedFlags)
        {
            return currentMask & authorisedFlags;
        }

        public static PIDLogOptions ToggleFlags(this PIDLogOptions currentMask, PIDLogOptions FlagsToToggle)
        {
            return (currentMask & ~(currentMask & FlagsToToggle)) | ((currentMask | FlagsToToggle) & ~currentMask);
        }
    }
}