using System;

namespace Monopoly
{
    public class MovedByCardEventArgs : EventArgs
    {
        public int RollToField { get; set; }
    }
}