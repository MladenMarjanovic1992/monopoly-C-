using System;
using System.Collections.Generic;
using System.Linq;

namespace Monopoly
{
    public class CardHousesRepair : ICard
    {
        public string CardMessage { get; set; }
        public int CostPerHouse { get; set; }
        public int CostPerHotel { get; set; }
        private List<FieldProperty> _fields;

        public void DrawCard(Player player, List<Player> otherPlayers)
        {
            Console.WriteLine(CardMessage);

            var fieldsOwned = _fields.Where(f => f.Owner == player);

            foreach (var field in fieldsOwned)
            {
                if (field.Houses == 5)
                    player.Money -= CostPerHotel;
                else
                    player.Money -= field.Houses * CostPerHouse;
            }
        }

        public void AddFields(List<FieldProperty> fields)
        {
            _fields = fields;
        }
    }
}