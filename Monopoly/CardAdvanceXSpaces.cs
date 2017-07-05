using System;
using System.Collections.Generic;

namespace Monopoly
{
    public class CardAdvanceXSpaces : ICard
    {
        public string CardMessage { get; set; }
        public int Spaces { get; set; }

        public event EventHandler<MovedByCardEventArgs> MovedByCard;

        public void DrawCard(Player player, List<Player> otherPlayers)
        {
            Console.WriteLine(CardMessage);

            OnMovedByCard(Spaces);
        }

        protected virtual void OnMovedByCard(int rollToNearestField)
        {
            MovedByCard?.Invoke(this, new MovedByCardEventArgs() { RollToField = rollToNearestField });
        }
    }
}