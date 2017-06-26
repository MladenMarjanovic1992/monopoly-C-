using System;

namespace Monopoly
{
    public class DiceEventArgs : EventArgs
    {
        public int Rolled1 { get; set; }
        public int Rolled2 { get; set; }
    }
}