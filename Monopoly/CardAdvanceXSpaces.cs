using System;
using System.Collections.Generic;

namespace Monopoly
{
    public class CardAdvanceXSpaces : ICard
    {
        public string CardMessage { get; set; }
        public int Spaces { get; set; }
        private Dice _dice;

        public void DrawCard(Player player, List<Player> otherPlayers)
        {
            Console.WriteLine(CardMessage);

            _dice.Roll(Spaces);
        }

        public void AddDice(Dice dice)
        {
            _dice = dice;
        }
    }
}