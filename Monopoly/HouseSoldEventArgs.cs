using System;

namespace Monopoly
{
    public class HouseSoldEventArgs : EventArgs
    {
        public IFieldBuildable Field { get; set; }
    }
}