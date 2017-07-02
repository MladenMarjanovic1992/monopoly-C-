using System;
using System.Collections.Generic;

namespace Monopoly
{
    public class CardGetOutOfJail : ICard
    {
        public string CardMessage { get; set; }

        public void DrawCard(Player player, IEnumerable<Player> otherPlayers)
        {
            Console.WriteLine(CardMessage);

            player.GetOutOfJailCards++;
        }
    }
}