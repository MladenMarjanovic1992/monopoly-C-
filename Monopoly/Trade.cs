using System;

namespace Monopoly
{
    public class Trade
    {
        public void BuyField(Player player, IFieldRentable field, int price)
        {
            player.Money -= price;
            field.Owner = player;
            field.CurrentRent = field.Rent[0];
            OnFieldBought(player, field);
        }

        public void SellField(Player player, IFieldRentable field, int price)
        {
            player.Money += price;
            field.Owner = null;
            OnFieldSold(player, field);
        }

        public event EventHandler<FieldBoughtEventArgs> FieldBought;
        public event EventHandler<FieldSoldEventArgs> FieldSold;

        public void OnChoseTrade(object sender, ChoseTradeEventArgs e)
        {
            if (e.BuyField != null)
            {
                SellField(e.Opponent, e.BuyField, e.OfferMoney);
                BuyField(e.CurrentPlayer, e.BuyField, e.OfferMoney);
            }
            if (e.SellField != null)
            {
                SellField(e.CurrentPlayer, e.SellField, e.AskMoney);
                BuyField(e.Opponent, e.SellField, e.AskMoney);
            }
        }

        protected virtual void OnFieldBought(Player player, IFieldRentable field)
        {
            FieldBought?.Invoke(this, new FieldBoughtEventArgs(){ NewOwner = player, Field = field });
        }

        protected virtual void OnFieldSold(Player player, IFieldRentable field)
        {
            FieldSold?.Invoke(this, new FieldSoldEventArgs(){ FormerOwner = player, Field = field });
        }
    }
}