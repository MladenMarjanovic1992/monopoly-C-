using System;
using System.Collections.Generic;

namespace Monopoly
{
    public interface ICard
    {
        string CardMessage { get; set; }
        void DrawCard(Player player, IEnumerable<Player> otherPlayers);
    }
}