using System;

namespace Monopoly
{
    public class FieldMortgagedEventArgs : EventArgs
    {
        public IFieldRentable Field { get; set; }
    }
}