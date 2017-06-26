﻿using System;
using System.Collections.Generic;

namespace Monopoly
{
    public class GoToJailField : IField
    {
        public string FieldName { get; set; }
        public int FieldIndex { get; set; }

        public void FieldEffect(Player currentPlayer, List<Player> otherPlayers)
        {
            PrintFieldStats();

            currentPlayer.Position = 10;
            currentPlayer.FieldPosition = FieldName;
        }

        public void PrintFieldStats()
        {
            Console.WriteLine($"Field: {FieldName}");
            Console.WriteLine($"Field position: {FieldIndex}");
            Console.WriteLine();
        }

        public void OnPlayerMoved(object sender, PlayerMovedEventArgs e)
        {
            if (FieldIndex == e.Player.Position)
            {
                e.Player.FieldPosition = FieldName;
                FieldEffect(e.Player, e.OtherPlayers);
            }
        }
    }
}