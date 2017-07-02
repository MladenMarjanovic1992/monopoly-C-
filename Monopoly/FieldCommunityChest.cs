using System;
using System.Collections.Generic;

namespace Monopoly
{
    public class FieldCommunityChest : IField
    {
        public string FieldName { get; set; }
        public int FieldIndex { get; set; }

        public void FieldEffect(Player currentPlayer, List<Player> otherPlayers)
        {
            Console.WriteLine("Community Chest!");
        }

        public void PrintFieldStats()
        {
            Console.WriteLine($"Field: {FieldName}");
            Console.WriteLine($"Field position: {FieldIndex}");
            Console.WriteLine();
        }
    }
}