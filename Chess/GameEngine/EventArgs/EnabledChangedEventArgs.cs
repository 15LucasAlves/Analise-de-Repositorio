using System;

class EnabledChangedEventArgs : EventArgs
{
    public bool Enabled { get; private set; }
    public bool PreviousEnabled { get; private set; }

    public EnabledChangedEventArgs(bool enabled, bool previousEnabled)
    {
        Enabled = enabled;
        PreviousEnabled = previousEnabled;
    }
}