using System;
using System.Collections.Generic;

namespace Monopoly
{
    public class CardPayOnlyPlayer : ICard
    {
        public string CardMessage { get; set; }
        public int Ammount { get; set; }

        public void DrawCard(Player player, IEnumerable<Player> otherPlayers)
        {
            Console.WriteLine(CardMessage);

            player.Money += Ammount;
        }
    }
}