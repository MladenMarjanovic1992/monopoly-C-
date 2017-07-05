using System;
using System.Collections.Generic;

namespace Monopoly
{
    public class CardAdvanceToNearest : ICard // These cards send the player to the nearest field of specified kind (example FieldStation, FieldUtility)
    {
        public string CardMessage { get; set; }
        public int[] FieldIndexes { get; set; } // Indices of specified Fields
        private int _mapSize;

        public event EventHandler<MovedByCardEventArgs> MovedByCard; 

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

            OnMovedByCard(RollToField(player));
        }

        public void AddMapSize(int mapSize)
        {
            _mapSize = mapSize;
        }

        protected virtual void OnMovedByCard(int rollToNearestField)
        {
            MovedByCard?.Invoke(this, new MovedByCardEventArgs(){RollToField = rollToNearestField});
        }
    }
}