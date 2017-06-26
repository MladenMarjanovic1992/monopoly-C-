using System;

namespace Monopoly
{
    public class FieldSoldEventArgs : EventArgs
    {
        public Player FormerOwner { get; set; }
        public IFieldRentable Field { get; set; }
    }
}