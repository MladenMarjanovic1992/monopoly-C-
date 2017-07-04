using System;

namespace Monopoly
{
    public class ChoseBuildEventArgs : EventArgs
    {
        public Player Player { get; set; }
        public IFieldBuildable PropertyField { get; set; }
    }
}