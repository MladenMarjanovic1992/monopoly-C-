using System;
using System.Collections.Generic;
using System.Linq;

namespace Monopoly
{
    public class Bankrupcy
    {
        private readonly List<IFieldRentable> _fieldsRentable;
        private readonly List<IFieldBuildable> _propertyFields;

        public Bankrupcy(Fields fields)
        {
            _fieldsRentable = fields.BuyableFields;
            _propertyFields = fields.BuildableFields;
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

        private List<Player> GetStakeHolders(Player player, List<Player> otherPlayers, bool bankruptDuringOwnTurn, int liquidationValue)
        {
            var stakeHolders = new List<Player>();

            if (bankruptDuringOwnTurn)
            {
                if (_fieldsRentable.Any(f => f.FieldIndex == player.Position))
                {
                    var fieldOwner = _fieldsRentable.First(f => f.FieldIndex == player.Position).Owner;
                    stakeHolders.Add(fieldOwner);
                    fieldOwner.Money += liquidationValue;
                }
                else
                    stakeHolders.AddRange(otherPlayers);
            }
            else
            {
                stakeHolders.AddRange(otherPlayers);
                otherPlayers[0].Money += liquidationValue;
            }

            return stakeHolders;
        }

        public void Liquidate(Player player, List<Player> otherPlayers, bool bankruptDuringOwnTurn)
        {
            var liquidationValue = HousesLiquidationValue(player) + MortgageValue(player) + player.Money;
            var propertiesWithHouses = _propertyFields.Where(p => p.Owner == player && p.Houses > 0);
            var allPlayerFields = _fieldsRentable.Where(f => f.Owner == player);
            var stakeHolders = GetStakeHolders(player, otherPlayers, bankruptDuringOwnTurn, liquidationValue);

            OnPlayerLiquidated(player, propertiesWithHouses, allPlayerFields, stakeHolders, bankruptDuringOwnTurn);
        }

        protected virtual void OnPlayerLiquidated(Player player, IEnumerable<IFieldBuildable> propertyFields,
            IEnumerable<IFieldRentable> allPlayerfields, List<Player> otherPlayers, bool bankruptDuringOwnTurn)
        {
            PlayerLiquidated?.Invoke(this, new PlayerLiquidatedEventArgs()
            {
                AllPlayerFields = allPlayerfields,
                PlayerLiquidated = player,
                PropertyFieldsWithHouses = propertyFields,
                StakeHolders = otherPlayers,
                BankruptDuringOwnTurn = bankruptDuringOwnTurn
            });
        }
    }
}