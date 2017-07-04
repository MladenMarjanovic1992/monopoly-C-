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
        private Dice _dice;
        private int _mapSize;

        public void DrawCard(Player player, List<Player> otherPlayers)
        {
            Console.WriteLine(CardMessage);

            int rolled;

            if (WithPassingStart)
            {
                if (player.Position < FieldIndex)
                    rolled = FieldIndex - player.Position;
                else
                    rolled = _mapSize - player.Position + FieldIndex;
            }
            else
            {
                rolled = FieldIndex - player.Position;
            }
            _dice.Roll(rolled);
        }

        public void AddDiceAndMapSize(Dice dice, int mapSize)
        {
            _dice = dice;
            _mapSize = mapSize;
        }
    }
}