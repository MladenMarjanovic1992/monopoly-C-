using System;
using System.Collections.Generic;
using System.Linq;

namespace Monopoly
{
    public class Bankrupcy
    {
        private readonly List<IFieldRentable> _fieldsRentable; // necessary for mortgage
        private readonly List<IFieldBuildable> _propertyFields; // necessary for liquidating houses

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

            return propertiesWithHouses.Sum(field => field.HousePrice / 2); // Rule: Liquidation value for houses is half the house price
        }

        private int MortgageValue(Player player)
        {
            var fieldsForMortgage = _fieldsRentable.Where(f => f.Owner == player && !f.UnderMortgage);

            return fieldsForMortgage.Sum(field => field.MortgageValue);
        }

        // Stakeholders are players which are affected by a player's bankrupcy (can be either 1 player, or all players except the bankrupted player)
        private List<Player> GetStakeHolders(Player player, List<Player> otherPlayers, bool bankruptDuringOwnTurn, int liquidationValue)
        {
            var stakeHolders = new List<Player>();

            if (bankruptDuringOwnTurn) // 2 Possible cases based on game rules
            {
                // Case 1: Player lands on owned field => Rule: All his remaining money and property go to the field owner
                if (_fieldsRentable.Any(f => f.FieldIndex == player.Position))
                {
                    var fieldOwner = _fieldsRentable.First(f => f.FieldIndex == player.Position).Owner;
                    stakeHolders.Add(fieldOwner);
                    fieldOwner.Money += liquidationValue;
                }
                else // Case 2: Player is bankrupted by the bank (Example: Tax field, Chance card etc.) Rule: Bank auctions off all his properties
                    stakeHolders.AddRange(otherPlayers);
            }
            else // Only one Scenario: The Current player has drawn a Card which requires all players to pay him money
            {   
                // Rule: If the player can't afford to pay, all his property and remaining money go to the player who drew the card
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