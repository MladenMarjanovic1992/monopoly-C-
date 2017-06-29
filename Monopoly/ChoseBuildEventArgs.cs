using System;

namespace Monopoly
{
    public class ChoseBuildEventArgs : EventArgs
    {
        public Player Player { get; set; }
        public PropertyField PropertyField { get; set; }
    }
}