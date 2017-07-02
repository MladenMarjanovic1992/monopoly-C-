using System;

namespace Monopoly
{
    public class ChoseBuildEventArgs : EventArgs
    {
        public Player Player { get; set; }
        public FieldProperty PropertyField { get; set; }
    }
}