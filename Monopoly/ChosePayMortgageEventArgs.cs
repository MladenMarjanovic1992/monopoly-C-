using System;

namespace Monopoly
{
    public class ChosePayMortgageEventArgs : EventArgs
    {
        public Player Player { get; set; }
        public IFieldRentable Field { get; set; }
    }
}