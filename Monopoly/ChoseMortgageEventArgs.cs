using System;

namespace Monopoly
{
    public class ChoseMortgageEventArgs : EventArgs
    {
        public IFieldRentable Field { get; set; }
        public Player Player { get; set; }
    }
}