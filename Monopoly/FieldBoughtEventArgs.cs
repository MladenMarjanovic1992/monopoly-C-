using System;

namespace Monopoly
{
    public class FieldBoughtEventArgs : EventArgs
    {
        public Player NewOwner { get; set; }
        public IFieldRentable Field { get; set; }
    }
}