using System;
using System.Collections.Generic;

namespace Monopoly
{
    public class MovedByCardEventArgs : EventArgs
    {
        public int FieldIndex { get; set; }
        public List<Player> OtherPlayers { get; set; }
        public bool WithPassingStart { get; set; }
    }
}