using System;
using System.Collections.Generic;
using System.Linq;

namespace Monopoly
{
    public class Bankrupcy
    {
        private readonly List<IFieldRentable> _fieldsRentable;
        private readonly List<PropertyField> _propertyFields;

        public Bankrupcy(Fields fields)
        {
            _fieldsRentable = fields.BuyableFields;
            _propertyFields = fields.PropertyFields;
        }

        public event EventHandler<PlayerLiquidatedEventArgs> PlayerLiquidated;

        public bool IsBankrupt(Player player)
        {
            return HousesLiquidationValue(player) + MortgageValue(player) + player.Money < 0;
        }

        private int HousesLiquidationValue(Player player)
        {
            var propertiesWithHouses = _propertyFields.Where(p => p.Owner == player && p.Houses > 0);

            return propertiesWithHouses.Sum(field => field.HousePrice / 2);
        }

        private int MortgageValue(Player player)
        {
            var fieldsForMortgage = _fieldsRentable.Where(f => f.Owner == player && !f.UnderMortgage);

            return fieldsForMortgage.Sum(field => field.MortgageValue);
        }

        public void Liquidate(Player player, List<Player> otherPlayers)
        {
            var liquidationValue = HousesLiquidationValue(player) + MortgageValue(player) + player.Money;
            var propertiesWithHouses = _propertyFields.Where(p => p.Owner == player && p.Houses > 0);
            var allPlayerFields = _fieldsRentable.Where(f => f.Owner == player);
            var fieldOwner = _fieldsRentable.First(f => f.FieldIndex == player.Position).Owner;
            var stakeHolders = new List<Player>();

            if (_fieldsRentable.Any(f => f.FieldIndex == player.Position))
            {
                stakeHolders.Add(fieldOwner);
                fieldOwner.Money += liquidationValue;
            }
            else
                stakeHolders.AddRange(otherPlayers);

            OnPlayerLiquidated(player, propertiesWithHouses, allPlayerFields, stakeHolders);
        }

        protected virtual void OnPlayerLiquidated(Player player, IEnumerable<PropertyField> propertyFields,
            IEnumerable<IFieldRentable> allPlayerfields, List<Player> otherPlayers)
        {
            PlayerLiquidated?.Invoke(this, new PlayerLiquidatedEventArgs()
            {
                AllPlayerFields = allPlayerfields,
                PlayerLiquidated = player,
                PropertyFieldsWithHouses = propertyFields,
                StakeHolders = otherPlayers
            });
        }
    }
}