using System;
using System.Collections.Generic;

namespace Monopoly
{
    public class FieldChance : IField
    {
        public string FieldName { get; set; }
        public int FieldIndex { get; set; }
        public Cards Cards { get; set; }

        public void FieldEffect(Player currentPlayer, List<Player> otherPlayers)
        {
            PrintFieldStats();

            Cards.DrawNext(currentPlayer, otherPlayers);
        }

        public void PrintFieldStats()
        {
            Console.WriteLine($"Field: {FieldName}");
            Console.WriteLine($"Field position: {FieldIndex}");
            Console.WriteLine();
        }
    }
}