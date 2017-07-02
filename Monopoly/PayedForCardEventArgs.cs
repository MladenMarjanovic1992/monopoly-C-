using System;

namespace Monopoly
{
    public class PayedForCardEventArgs : EventArgs
    {
        public Player PlayerLiable { get; set; }
        public Player StakeHolder { get; set; }
    }
}