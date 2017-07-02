using System;
using System.Collections.Generic;

namespace Monopoly
{
    public class CardAdvanceXSpaces : ICard
    {
        public string CardMessage { get; set; }
        public int Spaces { get; set; }
        public Dice Dice { get; set; }

        public void DrawCard(Player player, IEnumerable<Player> otherPlayers)
        {
            Console.WriteLine(CardMessage);

            Dice.Roll(Spaces);
        }
    }
}