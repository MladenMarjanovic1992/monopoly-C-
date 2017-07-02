using System;
using System.Collections.Generic;

namespace Monopoly
{
    public class CardAdvanceToNearest : ICard
    {
        public string CardMessage { get; set; }
        public int[] FieldIndexes { get; set; }
        public Dice Dice { get; set; }

        private int RollToField(Player player)
        {
            var nextField = FieldIndexes[0];

            foreach (var i in FieldIndexes)
            {
                if (player.Position < i)
                {
                    nextField = i;
                    break;
                }
            }

            return player.Position < nextField ? nextField - player.Position : 40 - player.Position + nextField;
        }

        public void DrawCard(Player player, IEnumerable<Player> otherPlayers)
        {
            Console.WriteLine(CardMessage);

            Dice.Roll(RollToField(player));
        }
    }
}