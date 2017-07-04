using System;
using System.Collections.Generic;

namespace Monopoly
{
    public class CardAdvanceToNearest : ICard
    {
        public string CardMessage { get; set; }
        public int[] FieldIndexes { get; set; }
        private Dice _dice;
        private int _mapSize;

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

            return player.Position < nextField ? nextField - player.Position : _mapSize - player.Position + nextField;
        }

        public void DrawCard(Player player, List<Player> otherPlayers)
        {
            Console.WriteLine(CardMessage);

            _dice.Roll(RollToField(player));
        }

        public void AddDiceAndMapSize(Dice dice, int mapSize)
        {
            _dice = dice;
            _mapSize = mapSize;
        }
    }
}