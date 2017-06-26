using System;

namespace Monopoly
{
    public class ChoseTradeEventArgs : EventArgs
    {
        public Player Opponent { get; set; }
        public Player CurrentPlayer { get; set; }
        public IFieldRentable BuyField { get; set; }
        public IFieldRentable SellField { get; set; }
        public int OfferMoney { get; set; }
        public int AskMoney { get; set; }
    }
}