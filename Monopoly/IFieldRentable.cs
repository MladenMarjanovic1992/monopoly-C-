using System;

namespace Monopoly
{
    public interface IFieldRentable : IField
    {
        int Price { get; }
        bool UnderMortgage { get; set; }
        int MortgageValue { get; }
        Player Owner { get; set; }
        int[] Rent { get; set; }
        int CurrentRent { get; set; }
        string Color { get; set; }
        bool CanMortgage { get; set; }
        bool CanTrade { get; set; }

        //void OnPlayerMoved(object sender, PlayerMovedEventArgs e);
    }
}