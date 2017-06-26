using System;
using System.Collections.Generic;

namespace Monopoly
{
    public class PlayerMovedEventArgs : EventArgs
    {
        public Player Player { get; set; }
        public List<Player> OtherPlayers { get; set; }
    }
}