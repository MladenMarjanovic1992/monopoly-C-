using System;
using System.Collections.Generic;

namespace Monopoly
{
    public class GoToJailField : IField
    {
        public string FieldName { get; set; }
        public int FieldIndex { get; set; }

        public event EventHandler WentToJail; 

        public void FieldEffect(Player currentPlayer, List<Player> otherPlayers)
        {
            PrintFieldStats();

            OnWentToJail();
        }

        public void PrintFieldStats()
        {
            Console.WriteLine($"Field: {FieldName}");
            Console.WriteLine($"Field position: {FieldIndex}");
            Console.WriteLine();
        }

        protected virtual void OnWentToJail()
        {
            WentToJail?.Invoke(this, EventArgs.Empty);
        }
    }
}