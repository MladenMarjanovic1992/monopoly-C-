using System;

namespace Monopoly
{
    public class HouseBuiltEventArgs : EventArgs
    {
        public IFieldBuildable Field { get; set; }
    }
}