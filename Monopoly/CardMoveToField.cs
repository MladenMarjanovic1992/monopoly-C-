using System;
using System.Collections.Generic;
using System.Linq;

namespace Monopoly
{
    public class CardMoveToField : ICard
    {
        public string CardMessage { get; set; }
        public int FieldIndex { get; set; }
        public bool WithPassingStart { get; set; }
        private int _mapSize;

        public event EventHandler<MovedByCardEventArgs> MovedByCard;

        private int RollToField(Player player)
        {
            if (WithPassingStart) // Some Cards move the player by letting him pass start, others move him directly to the field
            {
                if (player.Position < FieldIndex)
                    return FieldIndex - player.Position;
                return _mapSize - player.Position + FieldIndex;
            }
            return FieldIndex - player.Position;
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
            MovedByCard?.Invoke(this, new MovedByCardEventArgs() { RollToField = rollToNearestField });
        }
    }
}