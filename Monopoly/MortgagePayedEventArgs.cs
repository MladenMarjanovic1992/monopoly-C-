using System;

namespace Monopoly
{
    public class MortgagePayedEventArgs : EventArgs
    {
        public IFieldRentable Field { get; set; }
    }
}