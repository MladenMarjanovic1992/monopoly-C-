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
        public Dice Dice { get; set; }

        public void DrawCard(Player player, IEnumerable<Player> otherPlayers)
        {
            Console.WriteLine(CardMessage);

            int rolled;

            if (WithPassingStart)
            {
                if (player.Position < FieldIndex)
                    rolled = FieldIndex - player.Position;
                else
                    rolled = 40 - player.Position + FieldIndex;
            }
            else
            {
                rolled = FieldIndex - player.Position;
            }
            Dice.Roll(rolled);
        }
    }
}