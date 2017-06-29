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

        public void OnPlayerLiquidated(object sender, PlayerLiquidatedEventArgs e)
        {
            foreach (var field in e.AllPlayerFields)
            {
                SellField(e.PlayerLiquidated, field, 0);

                if (e.StakeHolders.Count == 1)
                    BuyField(e.StakeHolders[0], field, 0);
                else
                {
                    foreach (var player in e.StakeHolders)
                    {
                        player.PrintStats();
                    }

                    Console.WriteLine("Auction!");
                    var highestBidder = Prompt.ChoosePlayer(e.StakeHolders, $"Who won the auction for {field.FieldName}?");
                    var highestBid = Prompt.EnterAmount(highestBidder, "What was the highest bid?");

                    BuyField(highestBidder, field, highestBid);
                }
                    
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