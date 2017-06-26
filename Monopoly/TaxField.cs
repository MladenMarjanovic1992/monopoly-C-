using System;
using System.Collections.Generic;

namespace Monopoly
{
    public class TaxField : IField
    {
        public string FieldName { get; set; }
        public int FieldIndex { get; set; }

        public int TaxAmount { get; set; } // implement when doing chance and chest

        public void FieldEffect(Player currentPlayer, List<Player> otherPlayers)
        {
            currentPlayer.Money -= TaxAmount;
            PrintFieldStats();
        }

        public void PrintFieldStats()
        {
            Console.WriteLine();
            Console.WriteLine($"Field: {FieldName}");
            Console.WriteLine($"Field position: {FieldIndex}");
            Console.WriteLine($"Tax amount: {TaxAmount}");
            Console.WriteLine();
        }

        public void OnPlayerMoved(object sender, PlayerMovedEventArgs e)
        {
            if (FieldIndex == e.Player.Position)
            {
                e.Player.FieldPosition = FieldName;
                PrintFieldStats();
                FieldEffect(e.Player, e.OtherPlayers);
            }
        }
    }
}