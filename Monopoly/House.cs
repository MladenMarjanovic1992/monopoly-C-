using System;

namespace Monopoly
{
    public class House // handles building houses
    {
        private void BuildHouse(Player player, IFieldBuildable field)
        {
            if (player.Money >= field.HousePrice)
            {
                field.Houses += 1;
                player.Money -= field.HousePrice;
                field.CurrentRent = field.Rent[field.Houses + 1];

                OnHouseBuilt(field);
            }
            else
            {
                Console.WriteLine("Not enough money");
            }
            
        }

        private void SellHouse(Player player, IFieldBuildable field)
        {
            field.Houses -= 1;
            player.Money += field.HousePrice / 2;
            field.CurrentRent = field.Rent[field.Houses + 1];

            OnHouseSold(field);
        }

        public void OnChoseBuildHouse(object sender, ChoseBuildEventArgs e)
        {
            BuildHouse(e.Player, e.PropertyField);
        }

        public void OnChoseSellHouse(object sender, ChoseBuildEventArgs e)
        {
            SellHouse(e.Player, e.PropertyField);
        }

        public void OnPlayerLiquidated(object sender, PlayerLiquidatedEventArgs e)
        {
            foreach (var field in e.PropertyFieldsWithHouses)
            {
                SellHouse(e.PlayerLiquidated, field);
            }
        }

        public event EventHandler<HouseBuiltEventArgs> HouseBuilt;
        public event EventHandler<HouseSoldEventArgs> HouseSold;

        protected virtual void OnHouseBuilt(IFieldBuildable field)
        {
            HouseBuilt?.Invoke(this, new HouseBuiltEventArgs(){Field = field});
        }

        protected virtual void OnHouseSold(IFieldBuildable field)
        {
            HouseSold?.Invoke(this, new HouseSoldEventArgs() { Field = field });
        }
    }
}