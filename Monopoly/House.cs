using System;

namespace Monopoly
{
    public class House
    {
        public void BuildHouse(Player player, IFieldBuildable field)
        {
            field.Houses += 1;
            player.Money -= field.HousePrice;
            field.CurrentRent = field.Rent[field.Houses + 1];

            OnHouseBuilt(field);
        }

        public void SellHouse(Player player, IFieldBuildable field)
        {
            field.Houses -= 1;
            player.Money += field.HousePrice / 2;
            field.CurrentRent = field.Rent[field.Houses + 1];

            OnHouseSold(field);
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