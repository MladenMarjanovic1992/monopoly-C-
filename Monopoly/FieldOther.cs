﻿using System;
using System.Collections.Generic;

namespace Monopoly
{
    public class FieldOther : IField
    {
        public string FieldName { get; set; }
        public int FieldIndex { get; set; }

        // These fields have no effect on the player
        public void FieldEffect(Player currentPlayer, List<Player> otherPlayers)
        {
            PrintFieldStats();
        }

        public void PrintFieldStats()
        {
            Console.WriteLine($"Field: {FieldName}");
            Console.WriteLine($"Field position: {FieldIndex}");
            Console.WriteLine();
        }
    }
}